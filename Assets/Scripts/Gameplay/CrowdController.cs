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

        private List<GameObject> activeCrowd = new List<GameObject>();
        private Dictionary<GameObject, float> angles = new Dictionary<GameObject, float>();
        private Dictionary<GameObject, float> radii = new Dictionary<GameObject, float>();
        private int totalSpawned; // 누적 스폰 수 (ring 계산용)
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

            for (int i = 0; i < targetCount; i++)
            {
                SpawnFollower();
            }

            Debug.Log($"[CrowdController] Initial crowd spawned: {CurrentCount}");
            OnCrowdCountChanged?.Invoke(CurrentCount);
        }

        private void SpawnFollower()
        {
            GameObject follower = objectPooler.GetPooledObject();

            if (follower == null) return;

            float angle = 0f;
            float radius = unitRadius;
            int ring = 0;
            int accumulated = 0;

            for (int r = 0; r < 100; r++)
            {
                int slots = GetSlotsInRing(r);

                if (totalSpawned < accumulated + slots)
                {
                    ring = r;
                    int indexInRing = totalSpawned - accumulated;
                    angle = (360f / slots) * indexInRing;
                    radius = ring == 0 ? unitRadius : unitRadius + ring * unitRadius * 2f;
                    break;
                }

                accumulated += slots;
            }

            totalSpawned++;

            follower.transform.position = playerPivot.position + GetPolarOffset(angle, radius);
            follower.transform.SetParent(null);
            activeCrowd.Add(follower);
            angles[follower] = angle;
            radii[follower] = radius;
        }

        private int GetSlotsInRing(int ring)
        {
            if (ring == 0) return 1;

            float circumference = 2f * Mathf.PI * (unitRadius + ring * unitRadius * 2f);

            return Mathf.FloorToInt(circumference / (unitRadius * 2f));
        }

        private int GetSlotsPerRing(int ring)
        {
            int total = 0;

            for (int i = 0; i <= ring; i++)
            {
                total += GetSlotsInRing(i);
            }

            return total;
        }

        private Vector3 GetPolarOffset(float angle, float radius)
        {
            float rad = angle * Mathf.Deg2Rad;
            return new Vector3(Mathf.Cos(rad) * radius, -0.5f, Mathf.Sin(rad) * radius);
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

                MoveFollower(activeCrowd[i]);
            }
        }

        private void MoveFollower(GameObject follower)
        {
            Vector3 targetPos = playerPivot.position + GetPolarOffset(angles[follower], radii[follower]);

            follower.transform.position = Vector3.Lerp(
                follower.transform.position,
                targetPos,
                lerpSpeed * Time.deltaTime
            );
        }

        private void NotifyCountChanged()
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

        private void AddCrowd(int count)
        {
            for (int i = 0; i < count; i++)
            {
                SpawnFollower();
            }

            Debug.Log($"[CrowdController] Added {count}. Total: {CurrentCount}");
        }

        private void RemoveCrowd(int count)
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
            angles.Remove(follower);
            radii.Remove(follower);
            objectPooler.ReturnToPool(follower);
            totalSpawned--;

            RebuildFormation();
        }

        private void RebuildFormation()
        {
            int count = activeCrowd.Count;
            if (count == 0) return;

            int tempIndex = 0;

            for (int r = 0; r < 100 && tempIndex < count; r++)
            {
                int slots = GetSlotsInRing(r);

                for (int i = 0; i < slots && tempIndex < count; i++)
                {
                    GameObject follower = activeCrowd[tempIndex];
                    float angle = (360f / slots) * i;
                    float radius = r == 0 ? unitRadius : unitRadius + r * unitRadius * 2f;

                    angles[follower] = angle;
                    radii[follower] = radius;
                    tempIndex++;
                }
            }
        }
    }
}