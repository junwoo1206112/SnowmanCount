using System;
using System.Collections.Generic;
using UnityEngine;

using SnowmanCount.Core;


namespace SnowmanCount.Gameplay
{
    public class CrowdController : MonoBehaviour
    {
        [Header("Crowd Settings")]
        [SerializeField] private int initialCount = 5;
        [SerializeField] private float unitRadius = 0.5f;
        [SerializeField] private float lerpSpeed = 5f;
        [SerializeField] private ObjectPooler objectPooler;

        [Header("Formation Density")]
        [SerializeField] private float minRadiusMultiplier = 0.6f;
        [SerializeField] private int squeezeStartCount = 50;
        [SerializeField] private int maxSqueezeCount = 200;

        private List<GameObject> activeCrowd = new List<GameObject>();
        private Transform playerPivot;
        private bool canUpdate;

        public event Action<int> OnCrowdCountChanged;
        public event Action OnCrowdDepleted;

        public int CurrentCount => activeCrowd.Count;

        private void OnDisable()
        {
            if (GameStateManager.Instance != null)
            {
                GameStateManager.Instance.OnStateChanged -= OnGameStateChanged;
            }
        }

        private void OnGameStateChanged(GameState newState)
        {
            canUpdate = newState == GameState.Play;
        }

        private void Start()
        {
            if (GameStateManager.Instance != null)
            {
                GameStateManager.Instance.OnStateChanged += OnGameStateChanged;
                canUpdate = GameStateManager.Instance.CurrentState == GameState.Play;
            }
            else
            {
                canUpdate = false;
            }

            playerPivot = transform;

            if (objectPooler == null)
            {
                objectPooler = FindFirstObjectByType<ObjectPooler>();
                if (objectPooler == null)
                {
                    Debug.LogError("[CrowdController] ObjectPooler not found");
                    return;
                }
            }

            SpawnInitialCrowd();
        }

        private void SpawnInitialCrowd()
        {
            int targetCount = initialCount;

            if (GameManager.carryOverCrowdCount > 0)
            {
                targetCount = GameManager.carryOverCrowdCount;
                GameManager.carryOverCrowdCount = -1;
            }

            SpawnFollowers(targetCount);

            Debug.Log($"[CrowdController] Initial crowd spawned: {CurrentCount}");
            OnCrowdCountChanged?.Invoke(CurrentCount);
        }
        private void SpawnFollowers(int count)
        {
            int startIndex = activeCrowd.Count;

            for (int i = 0; i < count; i++)
            {
                GameObject follower = objectPooler.GetPooledObject();
                if (follower == null) continue;

                SetupFollower(follower);

                follower.transform.SetParent(null);
                follower.transform.position = Vector3.zero;
                activeCrowd.Add(follower);
            }

            // 유닛이 추가될 때 전체 대형을 다시 계산하여 중간의 빈틈을 메꿈 (Compact & Fill)
            RedistributeAngles();

            for (int i = startIndex; i < activeCrowd.Count; i++)
            {
                GameObject follower = activeCrowd[i];
                FollowerComponent fc = follower.GetComponent<FollowerComponent>();
                if (fc != null)
                {
                    follower.transform.position = playerPivot.position + GetPolarOffset(fc.targetAngle, fc.targetRadius);
                }
            }
        }

        private void SetupFollower(GameObject obj)
        {
            if (obj.GetComponent<Collider>() == null)
            {
                CapsuleCollider collider = obj.AddComponent<CapsuleCollider>();
                collider.isTrigger = true;
                collider.radius = 0.3f;
                collider.height = 1f;
            }

            if (obj.GetComponent<Rigidbody>() == null)
            {
                Rigidbody rb = obj.AddComponent<Rigidbody>();
                rb.isKinematic = true;
                rb.useGravity = false;
            }

            if (obj.GetComponent<FollowerComponent>() == null)
            {
                obj.AddComponent<FollowerComponent>();
            }
        }

        private Vector3 GetPolarOffset(float angle, float radius)
        {
            float rad = angle * Mathf.Deg2Rad;
            return new Vector3(Mathf.Cos(rad) * radius, -0.5f, Mathf.Sin(rad) * radius);
        }

        private float GetDynamicRadius(int count)
        {
            if (count <= squeezeStartCount) return unitRadius;

            float t = (float)(count - squeezeStartCount) / (maxSqueezeCount - squeezeStartCount);
            t = Mathf.Clamp01(t);

            float multiplier = Mathf.Lerp(1.0f, minRadiusMultiplier, t);
            return unitRadius * multiplier;
        }

        private void RedistributeAngles()
        {
            int count = activeCrowd.Count;
            if (count == 0) return;

            // 현재 전체 인원수에 따른 반지름 계산
            float dynamicRadius = GetDynamicRadius(count);
            int tempIndex = 0;

            // --- 중심부 비우기 및 순차적 슬롯 할당 ---
            // 리스트의 인덱스 순서대로 좌표를 부여하므로, 
            // 앞에서 이미 자리를 잡은 유닛들은 인덱스가 바뀌지 않는 한 자리를 유지함.
            for (int ring = 1; ring < 100 && tempIndex < count; ring++)
            {
                int slotsInRing = GetSlotsInRing(ring, dynamicRadius);

                for (int i = 0; i < slotsInRing && tempIndex < count; i++)
                {
                    GameObject follower = activeCrowd[tempIndex];
                    FollowerComponent fc = follower.GetComponent<FollowerComponent>();

                    if (fc != null)
                    {
                        fc.targetAngle = (360f / slotsInRing) * i;
                        fc.targetRadius = ring * dynamicRadius * 2f;
                    }

                    tempIndex++;
                }
            }
        }

        private int GetSlotsInRing(int ring, float radius)
        {
            if (ring == 0) return 1; // 중심 1명

            // 링의 반지름: ring 1 = 2*radius, ring 2 = 4*radius...
            float ringRadius = ring * radius * 2f;
            float circumference = 2f * Mathf.PI * ringRadius;

            // 해당 링에 들어갈 수 있는 최대 유닛 수 계산
            return Mathf.Max(1, Mathf.FloorToInt(circumference / (radius * 2f)));
        }

        private void Update()
        {
            if (!canUpdate) return;

            for (int i = activeCrowd.Count - 1; i >= 0; i--)
            {
                if (activeCrowd[i] == null)
                {
                    activeCrowd.RemoveAt(i);
                    continue;
                }

                GameObject follower = activeCrowd[i];
                FollowerComponent fc = follower.GetComponent<FollowerComponent>();

                if (fc != null && fc.isFalling)
                {
                    // 추락 중인 유닛은 이동 로직에서 제외
                    continue;
                }

                MoveFollower(follower);
            }
        }

        private void MoveFollower(GameObject follower)
        {
            FollowerComponent fc = follower.GetComponent<FollowerComponent>();
            if (fc == null) return;

            Vector3 targetPos = playerPivot.position + GetPolarOffset(fc.targetAngle, fc.targetRadius);

            Vector3 currentPos = follower.transform.position;
            float distance = Vector3.Distance(currentPos, targetPos);

            if (distance > 1f)
            {
                follower.transform.position = targetPos;
            }
            else
            {
                follower.transform.position = Vector3.Lerp(
                    currentPos,
                    targetPos,
                    lerpSpeed * Time.deltaTime
                );
            }
        }

        public void NotifyCountChanged()
        {
            OnCrowdCountChanged?.Invoke(CurrentCount);
        }

        private void NotifyDepleted()
        {
            OnCrowdDepleted?.Invoke();

            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnCrowdDepleted();
            }
        }

        public void ApplyMathOperation(string operatorType, int value)
        {
            if (string.IsNullOrEmpty(operatorType))
            {
                Debug.LogWarning("[CrowdController] Empty operator received");
                return;
            }

            switch (operatorType)
            {
                case "+":
                    AddCrowd(value);
                    break;
                case "-":
                    RemoveCrowd(value);
                    break;
                case "x":
                case "*":
                    MultiplyCrowd(value);
                    break;
                case "÷":
                case "/":
                    DivideCrowd(value);
                    break;
                default:
                    Debug.LogWarning($"[CrowdController] Unknown operator: {operatorType}");
                    return;
            }

            NotifyCountChanged();
        }

        public void AddCrowd(int count)
        {
            SpawnFollowers(count);

            Debug.Log($"[CrowdController] Added {count}. Total: {CurrentCount}");
        }

        public void RemoveCrowd(int count)
        {
            int removeCount = Mathf.Min(count, activeCrowd.Count);

            for (int i = 0; i < removeCount; i++)
            {
                RemoveLastFollower();
            }

            Debug.Log($"[CrowdController] Removed {count}. Total: {CurrentCount}");

            if (CurrentCount <= 0)
            {
                NotifyDepleted();
            }
        }

        private void MultiplyCrowd(int multiplier)
        {
            int currentCount = activeCrowd.Count;
            int targetCount = currentCount * multiplier - currentCount;

            if (targetCount > 0)
            {
                AddCrowd(targetCount);
            }
        }

        private void DivideCrowd(int divisor)
        {
            if (divisor <= 0)
            {
                Debug.LogWarning("[CrowdController] Cannot divide by zero or negative");
                return;
            }

            int currentCount = activeCrowd.Count;
            int targetCount = Mathf.Max(0, currentCount / divisor);
            int removeCount = currentCount - targetCount;

            if (removeCount > 0)
            {
                RemoveCrowd(removeCount);
            }
        }

        private void RemoveLastFollower()
        {
            if (activeCrowd.Count == 0) return;

            int lastIndex = activeCrowd.Count - 1;
            GameObject follower = activeCrowd[lastIndex];

            activeCrowd.RemoveAt(lastIndex);
            objectPooler.ReturnToPool(follower);

            // 유닛 제거 시에는 대형을 재계산하지 않음 (카운트 마스터 스타일)
        }

        public void RemoveSpecificFollower(GameObject follower)
        {
            if (follower == null) return;
            if (!activeCrowd.Remove(follower)) return;

            objectPooler.ReturnToPool(follower);
            // 대형 재계산 제거
            NotifyCountChanged();

            if (CurrentCount <= 0)
            {
                NotifyDepleted();
            }
        }

        public void RemoveFromList(GameObject follower)
        {
            if (follower == null) return;
            if (!activeCrowd.Remove(follower)) return;

            // 대형 재계산 제거
            NotifyCountChanged();

            if (CurrentCount <= 0)
            {
                NotifyDepleted();
            }
        }

        public void ReturnFollowerToPool(GameObject follower)
        {
            if (follower == null) return;
            objectPooler.ReturnToPool(follower);
        }
    }
}
