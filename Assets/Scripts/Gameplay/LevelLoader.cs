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

        private ILevelDataProvider dataProvider;

        private void Start()
        {
            if (GameManager.Instance == null)
            {
                Debug.LogError("[LevelLoader] GameManager not found.");
                return;
            }

            dataProvider = GameManager.Instance.LevelDataProvider;

            if (dataProvider == null)
            {
                Debug.LogError("[LevelLoader] GameManager not ready. Level will not load.");
                return;
            }

            LevelData data = dataProvider.LoadLevel(GameManager.currentLevel);

            if (data == null)
            {
                Debug.Log("[LevelLoader] No more levels. All levels cleared!");

                if (GameManager.Instance != null)
                {
                    GameManager.Instance.ShowAllLevelsClear();
                }

                return;
            }

            WorldMover.ResetDistance();
            SpawnLevel(data);
        }

        private void SpawnLevel(LevelData data)
        {
            if (data == null)
            {
                return;
            }

            float maxDistance = 0f;

            CreateAllClearDetector();

            foreach (LevelRow row in data.rows)
            {
                SpawnObject(row);

                if (row.distance > maxDistance)
                {
                    maxDistance = row.distance;
                }
            }

            int waveCount = GameManager.currentLevel * 10;
            SpawnEnemyWave(maxDistance + 30f, waveCount);

            SetProgressBarLength(maxDistance);

            Debug.Log($"[LevelLoader] Level {data.levelNumber} loaded with {data.rows.Count} objects, length: {maxDistance}, enemy wave: {waveCount}");
        }

        private void CreateAllClearDetector()
        {
            GameObject detector = new GameObject("EnemyAllClearDetector");
            detector.AddComponent<EnemyAllClearDetector>();
        }

        private void SpawnEnemyWave(float positionZ, int enemyCount)
        {
            Vector3 position = new Vector3(0f, 0.5f, positionZ);

            GameObject enemyGroup = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            enemyGroup.name = "EnemyWave";
            enemyGroup.transform.position = position;
            enemyGroup.transform.localScale = new Vector3(2f, 2.5f, 2f);

            Renderer renderer = enemyGroup.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = new Color(0.8f, 0f, 0f);
            }

            Collider enemyCollider = enemyGroup.GetComponent<Collider>();
            if (enemyCollider != null)
            {
                enemyCollider.isTrigger = true;
            }

            EnemyGroup enemy = enemyGroup.AddComponent<EnemyGroup>();
            enemy.SetEnemyCount(enemyCount);
            enemyGroup.AddComponent<WorldMover>();

            for (int i = 0; i < enemyCount; i++)
            {
                GameObject minion = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                minion.name = $"WaveMinion_{i}";
                minion.transform.SetParent(enemyGroup.transform);
                minion.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);

                Renderer minionRenderer = minion.GetComponent<Renderer>();
                if (minionRenderer != null)
                {
                    minionRenderer.material.color = new Color(1f, 0.3f, 0f);
                }
            }

            EnemyAllClearDetector detector = FindFirstObjectByType<EnemyAllClearDetector>();
            if (detector != null)
            {
                detector.RegisterEnemy();
            }

            Debug.Log($"[LevelLoader] Enemy wave at {positionZ}: {enemyCount} enemies");
        }

        private void SetProgressBarLength(float maxDistance)
        {
            GameObject barObj = GameObject.Find("ProgressBar");

            if (barObj != null)
            {
                barObj.SendMessage("SetLevelLength", maxDistance, SendMessageOptions.DontRequireReceiver);
            }
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
                    SpawnObstacle(position, row.value, row.subValue);
                    break;
                case "hole":
                    SpawnHole(position);
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
            enemy.SetEnemyCount(count);
            enemyGroup.AddComponent<WorldMover>();

            EnemyAllClearDetector detector = FindFirstObjectByType<EnemyAllClearDetector>();
            if (detector != null)
            {
                detector.RegisterEnemy();
            }

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

        private void SpawnObstacle(Vector3 position, string obstacleType, string damageValue)
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

            if (int.TryParse(damageValue, out int damage))
            {
                obstacleCtrl.SetDamage(damage);
            }

            obstacle.AddComponent<WorldMover>();

            Debug.Log($"[LevelLoader] Obstacle at {position}: {obstacleType}, damage: {damageValue}");
        }

        private void SpawnHole(Vector3 position)
        {
            GameObject hole = GameObject.CreatePrimitive(PrimitiveType.Cube);
            hole.name = "Hole";
            hole.transform.position = new Vector3(position.x, -0.4f, position.z);
            hole.transform.localScale = new Vector3(2f, 0.1f, 2f);

            Renderer renderer = hole.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = Color.black;
            }

            BoxCollider collider = hole.GetComponent<BoxCollider>();
            if (collider != null)
            {
                collider.isTrigger = true;
            }

            hole.AddComponent<HoleController>();
            hole.AddComponent<WorldMover>();

            Debug.Log($"[LevelLoader] Hole at {position}");
        }
    }
}
