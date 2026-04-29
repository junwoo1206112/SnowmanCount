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
        private List<GameObject> allObjects = new List<GameObject>();

        public int ActiveCount 
        {
            get {
                int active = 0;
                foreach(var obj in allObjects) if(obj.activeInHierarchy) active++;
                return active;
            }
        }
        public int AvailableCount => poolQueue.Count;

        private void Awake()
        {
            if (prefab == null)
            {
                Debug.LogError($"[ObjectPooler] Prefab is not assigned on {gameObject.name}");
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

            Debug.Log($"[ObjectPooler] Initialized pool on {gameObject.name} with {initialPoolSize} instances");
        }

        private GameObject CreateNewInstance()
        {
            GameObject obj = Instantiate(prefab, transform);
            obj.name = $"{prefab.name}_{allObjects.Count}";
            allObjects.Add(obj);
            return obj;
        }

        public GameObject GetPooledObject()
        {
            GameObject obj;

            if (poolQueue.Count > 0)
            {
                obj = poolQueue.Dequeue();
            }
            else if (autoExpand)
            {
                obj = CreateNewInstance();
            }
            else
            {
                Debug.LogWarning($"[ObjectPooler] Pool exhausted on {gameObject.name}");
                return null;
            }

            obj.SetActive(true);
            return obj;
        }

        public void ReturnToPool(GameObject obj)
        {
            if (obj == null) return;

            obj.SetActive(false);
            obj.transform.SetParent(transform);
            poolQueue.Enqueue(obj);
        }
    }
}
