using UnityEngine;
using SnowmanCount.Core;
using SnowmanCount.Data;
using SnowmanCount.Data.Models;

namespace SnowmanCount.Gameplay
{
    public class LevelLoader : MonoBehaviour
    {
        [Header("Prefab References")]
        [SerializeField] private GameObject gatePrefab;
        [SerializeField] private GameObject finishPrefab;

        private ILevelDataProvider dataProvider;

        private void Awake()
        {
        }

        private void Start()
        {
            dataProvider = GameManager.Instance.LevelDataProvider;

            if (dataProvider == null)
            {
                Debug.LogError("[LevelLoader] GameManager not ready. Level will not load.");
                return;
            }

            LoadLevel(1);
        }

        public void LoadLevel(int levelNumber)
        {
            if (dataProvider == null)
            {
                Debug.LogError("[LevelLoader] Cannot load level: dataProvider is null");
                return;
            }

            LevelData data = dataProvider.LoadLevel(levelNumber);

            if (data == null)
            {
                Debug.LogError($"[LevelLoader] Failed to load level {levelNumber}");
                return;
            }

            foreach (LevelRow row in data.rows)
            {
                SpawnObject(row);
            }

            SpawnWalls();
            Debug.Log($"[LevelLoader] Level {levelNumber} loaded with {data.rows.Count} objects");
        }

        private void SpawnWalls()
        {
        }

        private void SpawnObject(LevelRow row)
        {
            Vector3 position = new Vector3(0f, 0.5f, row.distance);

            switch (row.objectType.ToLower())
            {
                case "gate":
                    SpawnGate(position, row.value, row.subValue);
                    break;
                case "enemy":
                    SpawnEnemy(position, row.value, int.TryParse(row.subValue, out int eCount) ? eCount : 3);
                    break;
                case "obstacle":
                    SpawnObstacle(position, row.value);
                    break;
                case "finish":
                    SpawnFinish(position);
                    break;
                default:
                    Debug.LogWarning($"[LevelLoader] Unknown object type: {row.objectType}");
                    break;
            }
        }

        private void SpawnGate(Vector3 position, string leftGate, string rightGate)
        {
            if (gatePrefab == null)
            {
                Debug.LogWarning("[LevelLoader] Gate prefab not assigned. Skipping gate spawn.");
                return;
            }

            float spacing = 2f;

            SpawnSingleGate(new Vector3(-spacing, position.y, position.z), leftGate);
            SpawnSingleGate(new Vector3(spacing, position.y, position.z), rightGate);
        }

        private void SpawnSingleGate(Vector3 position, string gateData)
        {
            string op = gateData.Length > 0 ? gateData[0].ToString() : "+";
            int val = gateData.Length > 1 && int.TryParse(gateData.Substring(1), out int v) ? v : 5;

            GameObject gate = Instantiate(gatePrefab, position, Quaternion.identity);
            gate.transform.localScale = new Vector3(3f, 4f, 1f);
            gate.AddComponent<WorldMover>();

            GateController gateCtrl = gate.GetComponent<GateController>();
            if (gateCtrl != null)
            {
                gateCtrl.SetGateData(op, val);
            }

            GameObject label = new GameObject($"GateLabel_{gateData}");
            label.transform.position = position + new Vector3(0f, 2.5f, 0f);
            label.AddComponent<WorldMover>();

            TextMesh textMesh = label.AddComponent<TextMesh>();
            textMesh.text = $"{op}{val}";
            textMesh.fontSize = 80;
            textMesh.anchor = TextAnchor.MiddleCenter;
            textMesh.alignment = TextAlignment.Center;
            textMesh.color = op == "+" || op == "x" || op == "*" ? Color.blue : Color.red;
            textMesh.characterSize = 0.08f;
            label.transform.rotation = Quaternion.identity;
        }

        private void SpawnEnemy(Vector3 position, string enemyType, int count)
        {
            GameObject enemyGroup = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            enemyGroup.name = $"Enemy_{enemyType}";
            enemyGroup.transform.position = position;
            enemyGroup.transform.localScale = new Vector3(1.5f, 2f, 1.5f);

            Renderer renderer = enemyGroup.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = Color.red;
            }

            Collider enemyCollider = enemyGroup.GetComponent<Collider>();
            if (enemyCollider != null)
            {
                enemyCollider.isTrigger = true;
            }

            EnemyGroup enemy = enemyGroup.AddComponent<EnemyGroup>();
            enemyGroup.AddComponent<WorldMover>();

            for (int i = 0; i < count; i++)
            {
                GameObject minion = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                minion.name = $"EnemyMinion_{i}";
                minion.transform.SetParent(enemyGroup.transform);
                minion.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);

                Renderer minionRenderer = minion.GetComponent<Renderer>();
                if (minionRenderer != null)
                {
                    minionRenderer.material.color = new Color(1f, 0.5f, 0f);
                }
            }

            Debug.Log($"[LevelLoader] Enemy at {position}: {enemyType} x{count}");
        }

        private void SpawnObstacle(Vector3 position, string obstacleType)
        {
            GameObject obstacle = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            obstacle.name = $"Obstacle_{obstacleType}";
            obstacle.transform.position = position;
            obstacle.transform.localScale = new Vector3(2f, 1f, 2f);

            Renderer renderer = obstacle.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = Color.gray;
            }

            Collider obstacleCollider = obstacle.GetComponent<Collider>();
            if (obstacleCollider != null)
            {
                obstacleCollider.isTrigger = true;
            }

            ObstacleController obstacleCtrl = obstacle.AddComponent<ObstacleController>();
            obstacle.AddComponent<WorldMover>();

            Debug.Log($"[LevelLoader] Obstacle at {position}: {obstacleType}");
        }

        private void SpawnFinish(Vector3 position)
        {
            if (finishPrefab != null)
            {
                GameObject finish = Instantiate(finishPrefab, position, Quaternion.identity);
                finish.name = "Finish";
                finish.AddComponent<WorldMover>();
                return;
            }

            GameObject finishObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            finishObj.name = "Finish";
            finishObj.transform.position = position;
            finishObj.transform.localScale = new Vector3(8f, 0.5f, 1f);

            Renderer renderer = finishObj.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = Color.green;
            }

            BoxCollider collider = finishObj.GetComponent<BoxCollider>();
            if (collider != null)
            {
                collider.isTrigger = true;
            }

            finishObj.AddComponent<WorldMover>();

            Debug.Log($"[LevelLoader] Finish line at {position}");
        }
    }
}
