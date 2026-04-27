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
        [SerializeField] private float followRadius = 3f;
        [SerializeField] private float lerpSpeed = 8f;
        [SerializeField] private ObjectPooler objectPooler;

        private List<GameObject> activeCrowd = new List<GameObject>();
        private Transform playerPivot;

        public event Action<int> OnCrowdCountChanged;
        public event Action OnCrowdDepleted;

        public int CurrentCount => activeCrowd.Count;

        private void Start()
        {
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
            for (int i = 0; i < initialCount; i++)
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

            follower.transform.position = GetRandomPositionAroundPivot();
            follower.transform.SetParent(null);
            activeCrowd.Add(follower);
        }

        private Vector3 GetRandomPositionAroundPivot()
        {
            Vector2 randomCircle = UnityEngine.Random.insideUnitCircle * followRadius;
            return playerPivot.position + new Vector3(randomCircle.x, 0f, randomCircle.y);
        }

        private void Update()
        {
            for (int i = activeCrowd.Count - 1; i >= 0; i--)
            {
                if (activeCrowd[i] == null)
                {
                    activeCrowd.RemoveAt(i);
                    continue;
                }

                MoveFollower(activeCrowd[i], i);
            }
        }

        private void MoveFollower(GameObject follower, int index)
        {
            Vector3 targetPos = playerPivot.position + GetOffsetForIndex(index);
            follower.transform.position = Vector3.Lerp(
                follower.transform.position,
                targetPos,
                lerpSpeed * Time.deltaTime
            );
        }

        private Vector3 GetOffsetForIndex(int index)
        {
            if (activeCrowd.Count == 0) return Vector3.zero;

            float angle = (360f / activeCrowd.Count) * index;
            float radius = Mathf.Min(followRadius, 1f + activeCrowd.Count * 0.15f);
            float rad = angle * Mathf.Deg2Rad;

            return new Vector3(Mathf.Cos(rad) * radius, 0f, Mathf.Sin(rad) * radius);
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

            OnCrowdCountChanged?.Invoke(CurrentCount);

            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnCrowdCountChanged(CurrentCount);
            }
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
                OnCrowdDepleted?.Invoke();

                if (GameManager.Instance != null)
                {
                    GameManager.Instance.OnCrowdDepleted();
                }
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
        }
    }
}
