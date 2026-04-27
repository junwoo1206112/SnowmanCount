using System.Collections.Generic;
using UnityEngine;

namespace SnowmanCount.Gameplay
{
    public class ObjectPooler : MonoBehaviour
    {
        [Header("Pool Settings")]
        [SerializeField] private GameObject prefab;
        [SerializeField] private int initialPoolSize = 50;
        [SerializeField] private bool autoExpand = true;

        private Queue<GameObject> poolQueue = new Queue<GameObject>();

        public int ActiveCount => initialPoolSize - poolQueue.Count;
        public int AvailableCount => poolQueue.Count;

        private void Awake()
        {
            if (prefab == null)
            {
                Debug.LogError("[ObjectPooler] Prefab is not assigned");
                return;
            }

            InitializePool();
        }

        private void InitializePool()
        {
            for (int i = 0; i < initialPoolSize; i++)
            {
                GameObject obj = CreateNewInstance();
                obj.SetActive(false);
                poolQueue.Enqueue(obj);
            }

            Debug.Log($"[ObjectPooler] Initialized pool with {initialPoolSize} instances");
        }

        private GameObject CreateNewInstance()
        {
            GameObject obj = Instantiate(prefab, transform);
            obj.name = $"{prefab.name}_{poolQueue.Count}";
            return obj;
        }

        public GameObject GetPooledObject()
        {
            if (poolQueue.Count > 0)
            {
                GameObject obj = poolQueue.Dequeue();
                obj.SetActive(true);
                return obj;
            }

            if (autoExpand)
            {
                GameObject obj = CreateNewInstance();
                obj.SetActive(true);
                Debug.Log($"[ObjectPooler] Pool expanded. New size: {poolQueue.Count + 1}");
                return obj;
            }

            Debug.LogWarning("[ObjectPooler] Pool exhausted and autoExpand is disabled");
            return null;
        }

        public void ReturnToPool(GameObject obj)
        {
            if (obj == null)
            {
                Debug.LogWarning("[ObjectPooler] Attempted to return null object");
                return;
            }

            obj.SetActive(false);
            obj.transform.SetParent(transform);
            poolQueue.Enqueue(obj);
        }
    }
}
