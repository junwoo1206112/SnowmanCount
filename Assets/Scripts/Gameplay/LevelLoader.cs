using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using SnowmanCount.Core;
using SnowmanCount.Data;
using SnowmanCount.Data.Models;

namespace SnowmanCount.Gameplay
{
    public class LevelLoader : MonoBehaviour
    {
        [Header("Prefab References")]
        [SerializeField] private GameObject gatePrefab;
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private GameObject bossPrefab;
        [Header("Road Settings")]
        [SerializeField] private Material roadMaterial;
        [SerializeField] private float roadWidthMultiplier = 3.0f;

        private ILevelDataProvider dataProvider;
        private LevelData currentLevelData;
        private bool hasBossInLevel;
        private GameObject lastFinishLineObject;
        private List<float> stepWidths = new List<float>();
        private List<float> stepSpacingsX = new List<float>();
        private List<int> stepCols = new List<int>();
        private List<float> stepDepths = new List<float>();
        private float highestMultiplier;

        private static Material goldEmissionMaterial;
        private static Material goldDarkMaterial;
        private static Material goldAccentMaterial;

        private static Material GetGoldEmissionMaterial()
        {
            if (goldEmissionMaterial == null)
            {
                goldEmissionMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                goldEmissionMaterial.color = new Color(1f, 0.78f, 0.06f);
                goldEmissionMaterial.SetFloat("_Metallic", 0.6f);
                goldEmissionMaterial.SetFloat("_Smoothness", 0.4f);
                goldEmissionMaterial.EnableKeyword("_EMISSION");
                goldEmissionMaterial.SetColor("_EmissionColor", new Color(1f, 0.78f, 0.06f) * 0.3f);
            }
            return goldEmissionMaterial;
        }

        private static Material GetGoldDarkMaterial()
        {
            if (goldDarkMaterial == null)
            {
                goldDarkMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                goldDarkMaterial.color = new Color(0.6f, 0.4f, 0.05f);
                goldDarkMaterial.SetFloat("_Metallic", 0.3f);
                goldDarkMaterial.SetFloat("_Smoothness", 0.2f);
            }
            return goldDarkMaterial;
        }

        private static Material GetGoldAccentMaterial()
        {
            if (goldAccentMaterial == null)
            {
                goldAccentMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                goldAccentMaterial.color = new Color(1f, 0.9f, 0.28f);
                goldAccentMaterial.SetFloat("_Metallic", 0.45f);
                goldAccentMaterial.SetFloat("_Smoothness", 0.55f);
                goldAccentMaterial.EnableKeyword("_EMISSION");
                goldAccentMaterial.SetColor("_EmissionColor", new Color(1f, 0.75f, 0.12f) * 0.18f);
            }
            return goldAccentMaterial;
        }

        private static GameObject CreateStairCube(string objectName, Vector3 scale, Vector3 position, Material material)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.name = objectName;
            cube.transform.localScale = scale;
            cube.transform.position = position;

            Renderer renderer = cube.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = material;
            }

            Destroy(cube.GetComponent<Collider>());
            return cube;
        }

        private static int GetStairCols(int stepIndex)
        {
            return Mathf.Max(2, 8 - stepIndex / 6);
        }

        private static int GetStairRows(int stepIndex)
        {
            return Mathf.Max(1, 2 - stepIndex / 36);
        }

        private static float GetStairWidthRatio(int stepIndex, int stepCount)
        {
            if (stepCount <= 1)
            {
                return 0.7f;
            }

            float t = stepIndex / (float)(stepCount - 1);
            return Mathf.Lerp(0.64f, 0.16f, t);
        }

        private static int GetMinimumVisibleStairCount(int totalFollowers)
        {
            return totalFollowers >= 80 ? 42 : 24;
        }

        private static void BuildStairBlockDetails(
            int stepIndex,
            int cols,
            int rows,
            float pivotX,
            float centerZ,
            float stepY,
            float stepW,
            float stepD,
            float stepRise,
            float unitSpacingZ)
        {
            float tileW = Mathf.Min(0.45f, stepW / (cols + 1) * 0.82f);
            float tileD = Mathf.Min(0.48f, unitSpacingZ * 0.78f);
            float tileH = 0.08f;
            float usableZ = Mathf.Max(0.1f, stepD - 0.18f);
            float spacingX = stepW / (cols + 1);
            float spacingZ = rows <= 1 ? 0f : usableZ / rows;
            Material accentMaterial = GetGoldAccentMaterial();
            Material darkMaterial = GetGoldDarkMaterial();

            for (int r = 0; r < rows; r++)
            {
                float zOff = (r - (rows - 1) * 0.5f) * (rows <= 1 ? 0f : spacingZ);

                for (int c = 0; c < cols; c++)
                {
                    float xOff = (c - (cols - 1) * 0.5f) * spacingX;
                    CreateStairCube(
                        $"StairTile_{stepIndex}_{r}_{c}",
                        new Vector3(tileW, tileH, tileD),
                        new Vector3(pivotX + xOff, stepY + 0.14f, centerZ + zOff),
                        accentMaterial);
                }
            }

            float riserTileH = Mathf.Max(0.12f, stepRise * 0.36f);
            float riserZ = centerZ - stepD / 2f - 0.04f;
            float lowerY = stepY - stepRise * 0.3f;
            float upperY = stepY - stepRise * 0.72f;

            for (int c = 0; c < cols; c++)
            {
                float xOff = (c - (cols - 1) * 0.5f) * spacingX;
                Material material = c % 2 == 0 ? accentMaterial : darkMaterial;
                CreateStairCube(
                    $"StairFace_{stepIndex}_{c}",
                    new Vector3(tileW, riserTileH, 0.1f),
                    new Vector3(pivotX + xOff, lowerY, riserZ),
                    material);

                if (stepIndex > 0 && c % 2 == 0)
                {
                    CreateStairCube(
                        $"StairFaceInset_{stepIndex}_{c}",
                        new Vector3(tileW * 0.72f, riserTileH * 0.75f, 0.12f),
                        new Vector3(pivotX + xOff, upperY, riserZ - 0.02f),
                        darkMaterial);
                }
            }
        }

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
                Debug.Log($"[LevelLoader] No level data for Level {GameManager.currentLevel}. Showing all levels clear.");

                if (GameManager.Instance != null)
                {
                    GameManager.Instance.ShowAllLevelsClear();
                }

                return;
            }

            WorldMover.ResetDistance();
            SpawnLevel(data);
        }

        private enum HoleType { None, Left, Right, Full }

        [Header("Level Spacing")]
        [SerializeField] private float distanceStartOffset = 12f;
        [SerializeField] private float distanceSpacingMultiplier = 1.5f;

        private void SpawnLevel(LevelData data)
        {
            if (data == null) return;
            currentLevelData = data;

            float maxDistance = 0f;
            foreach (var row in data.rows) if (row.distance > maxDistance) maxDistance = row.distance;
            maxDistance = maxDistance * distanceSpacingMultiplier + distanceStartOffset + 80f;

            CreateAllClearDetector();

            float segmentLength = 5f;
            for (float z = 0; z < maxDistance; z += segmentLength)
            {
                HoleType hole = z < distanceStartOffset
                    ? HoleType.None
                    : GetHoleTypeAt((z - distanceStartOffset) / distanceSpacingMultiplier, data.rows);
                
                if (hole != HoleType.Full)
                {
                    SpawnRoadSegment(z, segmentLength, hole);
                }

                if (hole != HoleType.None)
                {
                    SpawnHoleTrigger(z, segmentLength, hole);
                }
            }

            foreach (LevelRow row in data.rows)
            {
                if (row.objectType.ToLower() == "hole") continue;
                SpawnObject(row);
            }

            SetProgressBarLength(maxDistance);
        }

        private HoleType GetHoleTypeAt(float z, System.Collections.Generic.List<LevelRow> rows)
        {
            if (GameManager.currentLevel == 1 && Mathf.Abs(z - 25f) < 3f) return HoleType.Left;
            if (GameManager.currentLevel == 2 && Mathf.Abs(z - 20f) < 3f) return HoleType.Right;

            // 엑셀 데이터의 구멍 확인
            foreach (var row in rows)
            {
                if (row.objectType.ToLower() == "hole" && Mathf.Abs(row.distance - z) < 3f)
                {
                    string val = row.value.ToLower();
                    if (val == "left") return HoleType.Left;
                    if (val == "right") return HoleType.Right;
                    return HoleType.Left;
                }
            }
            return HoleType.None;
        }

        private float GetTotalWidth()
        {
            SwerveMovement sm = FindFirstObjectByType<SwerveMovement>();
            float baseWidth = (sm != null) ? sm.XBound * 2f : 30f;
            return baseWidth * roadWidthMultiplier;
        }

        private void SpawnRoadSegment(float z, float length, HoleType hole)
        {
            float totalWidth = GetTotalWidth();
            float currentWidth = totalWidth;
            float xPos = 0f;

            if (hole == HoleType.Left)
            {
                currentWidth = totalWidth / 2f;
                xPos = totalWidth / 4f;
            }
            else if (hole == HoleType.Right)
            {
                currentWidth = totalWidth / 2f;
                xPos = -totalWidth / 4f;
            }

            GameObject road = GameObject.CreatePrimitive(PrimitiveType.Cube);
            road.name = $"Road_{z}";
            road.transform.position = new Vector3(xPos, -0.1f, z + length / 2f);
            road.transform.localScale = new Vector3(currentWidth, 0.2f, length);

            Renderer renderer = road.GetComponent<Renderer>();
            if (renderer != null)
            {
                if (roadMaterial != null) renderer.material = roadMaterial;
                bool isEven = Mathf.RoundToInt(z / length) % 2 == 0;
                renderer.material.color = isEven ? new Color(0.85f, 0.95f, 1f) : new Color(0.8f, 0.9f, 1f);
            }

            road.AddComponent<WorldMover>();

            if (hole != HoleType.Left) SpawnSideRail(z, length, totalWidth, true);
            if (hole != HoleType.Right) SpawnSideRail(z, length, totalWidth, false);
        }

        private void SpawnHoleTrigger(float z, float length, HoleType hole)
        {
            float totalWidth = GetTotalWidth();
            float triggerWidth = totalWidth;
            float xPos = 0f;

            if (hole == HoleType.Left)
            {
                triggerWidth = totalWidth / 2f;
                xPos = -totalWidth / 4f;
            }
            else if (hole == HoleType.Right)
            {
                triggerWidth = totalWidth / 2f;
                xPos = totalWidth / 4f;
            }

            GameObject holeTrigger = new GameObject($"Hole_Trigger_{z}");
            holeTrigger.transform.position = new Vector3(xPos, 0f, z + length / 2f);
            
            BoxCollider collider = holeTrigger.AddComponent<BoxCollider>();
            collider.isTrigger = true;
            collider.size = new Vector3(triggerWidth, 10f, length);

            holeTrigger.AddComponent<HoleController>();
            holeTrigger.AddComponent<WorldMover>();
        }

        private void SpawnSideRail(float z, float length, float currentRoadWidth, bool isLeft)
        {
            GameObject rail = GameObject.CreatePrimitive(PrimitiveType.Cube);
            rail.name = isLeft ? $"Rail_L_{z}" : $"Rail_R_{z}";
            
            float xPos = (currentRoadWidth / 2f + 0.15f) * (isLeft ? -1f : 1f);
            rail.transform.position = new Vector3(xPos, 0.1f, z + length / 2f);
            rail.transform.localScale = new Vector3(0.3f, 0.6f, length);

            Renderer renderer = rail.GetComponent<Renderer>();
            if (renderer != null) renderer.material.color = Color.white;

            rail.AddComponent<WorldMover>();
        }

        private void CreateAllClearDetector()
        {
            GameObject detector = new GameObject("EnemyAllClearDetector");
            detector.AddComponent<EnemyAllClearDetector>();
        }

        private GameObject SpawnEnemyMinion()
        {
            GameObject minion;
            if (enemyPrefab != null)
            {
                minion = Instantiate(enemyPrefab);
            }
            else
            {
                minion = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                minion.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                Renderer r = minion.GetComponent<Renderer>();
                if (r != null) r.material.color = new Color(1f, 0.3f, 0f);
            }

            Collider col = minion.GetComponent<Collider>();
            if (col != null) col.isTrigger = true;

            return minion;
        }

        private void SpawnEnemyWave(float positionZ, int enemyCount)
        {
            Vector3 position = new Vector3(0f, 0f, positionZ);

            GameObject enemyGroupObj = new GameObject("EnemyWave");
            enemyGroupObj.transform.position = position;

            EnemyGroup enemy = enemyGroupObj.AddComponent<EnemyGroup>();
            enemy.SetEnemyCount(enemyCount);
            enemyGroupObj.AddComponent<WorldMover>();

            enemy.RegisterAsWave();

            SpawnMultiRingFormation(enemyGroupObj.transform, enemyCount, enemy);

            // Enemy centralized count label
            GameObject enemyCountLabel = new GameObject("EnemyCountLabel");
            enemyCountLabel.transform.SetParent(enemyGroupObj.transform);
            enemyCountLabel.transform.localPosition = new Vector3(0f, 2.5f, 0f);
            TextMesh enemyCountText = enemyCountLabel.AddComponent<TextMesh>();
            enemyCountText.text = enemyCount.ToString();
            enemyCountText.fontSize = 80;
            enemyCountText.characterSize = 0.08f;
            enemyCountText.anchor = TextAnchor.MiddleCenter;
            enemyCountText.alignment = TextAlignment.Center;
            enemyCountText.fontStyle = FontStyle.Bold;
            enemyCountText.color = Color.red;

            Debug.Log($"[LevelLoader] Enemy wave at {positionZ}: {enemyCount} enemies");
        }

        private void SpawnMultiRingFormation(Transform parent, int count, EnemyGroup group)
        {
            float unitSpacing = 1.5f;
            int tempIndex = 0;

            for (int ring = 0; ring < 100 && tempIndex < count; ring++)
            {
                int slotsInRing;
                float ringRadius;

                if (ring == 0)
                {
                    slotsInRing = 1;
                    ringRadius = 0f;
                }
                else
                {
                    ringRadius = ring * unitSpacing;
                    float circumference = 2f * Mathf.PI * ringRadius;
                    slotsInRing = Mathf.Max(1, Mathf.FloorToInt(circumference / unitSpacing));
                }

                for (int i = 0; i < slotsInRing && tempIndex < count; i++)
                {
                    float angle = (360f / slotsInRing) * i;
                    float rad = angle * Mathf.Deg2Rad;

                    GameObject minion = SpawnEnemyMinion();
                    minion.name = $"Minion_{tempIndex}";
                    minion.transform.SetParent(parent);
                    minion.transform.localPosition = new Vector3(
                        Mathf.Cos(rad) * ringRadius,
                        0f,
                        Mathf.Sin(rad) * ringRadius
                    );

                    EnemyMinion minionCtrl = minion.GetComponent<EnemyMinion>();
                    if (minionCtrl == null) minionCtrl = minion.AddComponent<EnemyMinion>();
                    minionCtrl.Setup(group);

                    tempIndex++;
                }
            }
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
            Vector3 position = new Vector3(0f, 0.5f, distanceStartOffset + row.distance * distanceSpacingMultiplier);

            switch (row.objectType.ToLower())
            {
                case "gate":
                    SpawnGate(position, row.value, row.subValue);
                    break;
                case "enemy":
                    SpawnEnemy(position, row.value, int.TryParse(row.subValue, out int eCount) ? eCount : 3);
                    break;
                case "obstacle":
                    if (row.value.ToLower() == "wall")
                    {
                        string[] parts = row.subValue.Split(',');
                        string gapPos = parts.Length > 0 ? parts[0] : "center";
                        string damage = parts.Length > 1 ? parts[1] : "1";
                        SpawnWall(position, gapPos, damage);
                    }
                    else
                    {
                        SpawnObstacle(position, row.value, row.subValue);
                    }
                    break;
                case "hole":
                    break;
                case "boss":
                    hasBossInLevel = true;
                    SpawnBoss(position, row.value);
                    break;
                case "finish":
                    lastFinishLineObject = SpawnFinishLine(position);
                    break;
                default:
                    Debug.LogWarning($"[LevelLoader] Unknown object type: {row.objectType}");
                    break;
            }
        }

        private void SpawnBoss(Vector3 position, string hpValue)
        {
            int hp = int.TryParse(hpValue, out int h) ? h : 30;

            GameObject bossObj;

            if (bossPrefab != null)
            {
                bossObj = Instantiate(bossPrefab, position, Quaternion.identity);
                bossObj.name = "Boss";
                bossObj.transform.localScale = new Vector3(2f, 2f, 2f);
            }
            else
            {
                bossObj = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                bossObj.name = "Boss";
                bossObj.transform.position = position;
                bossObj.transform.localScale = new Vector3(2f, 3f, 2f);

                Renderer renderer = bossObj.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.color = new Color(0.8f, 0.1f, 0.1f);
                }
            }

            Collider col = bossObj.GetComponent<Collider>();
            if (col == null)
            {
                SphereCollider sphere = bossObj.AddComponent<SphereCollider>();
                sphere.radius = 1.5f;
                col = sphere;
            }
            col.isTrigger = true;

            Rigidbody rb = bossObj.GetComponent<Rigidbody>();
            if (rb == null) rb = bossObj.AddComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.useGravity = false;

            BossController boss = bossObj.GetComponent<BossController>();
            if (boss == null) boss = bossObj.AddComponent<BossController>();
            boss.Setup(hp);

            bossObj.AddComponent<WorldMover>();

            Debug.Log($"[LevelLoader] Boss at {position}: HP={hp}");
        }

        private GameObject SpawnFinishLine(Vector3 position)
        {
            position.y = 0f;
            float roadW = GetTotalWidth();

            GameObject finishObj = new GameObject("FinishLine");
            finishObj.transform.position = position;

            BoxCollider col = finishObj.AddComponent<BoxCollider>();
            col.size = new Vector3(roadW + 2f, 6f, 2f);
            col.isTrigger = true;

            FinishLineController flc = finishObj.AddComponent<FinishLineController>();
            flc.SetRoadWidth(roadW);
            finishObj.AddComponent<WorldMover>();

            Debug.Log($"[LevelLoader] Finish line at {position}");

            return finishObj;
        }

        private void SpawnGate(Vector3 position, string leftGate, string rightGate)
        {
            if (gatePrefab == null) return;

            float totalWidth = GetTotalWidth();
            float halfRoad = totalWidth * 0.5f;

            HoleType hole = GetHoleTypeAt(position.z, currentLevelData.rows);
            
            float leftX, rightX;
            float targetY = 2.0f;

            if (hole == HoleType.Left)
            {
                leftX = -halfRoad * 0.15f;
                rightX = halfRoad * 0.7f;
            }
            else if (hole == HoleType.Right)
            {
                leftX = -halfRoad * 0.7f;
                rightX = halfRoad * 0.15f;
            }
            else
            {
                float spacing = halfRoad * 0.55f;
                leftX = -spacing;
                rightX = spacing;
            }

            SpawnSingleGate(new Vector3(leftX, targetY, position.z), leftGate);
            SpawnSingleGate(new Vector3(rightX, targetY, position.z), rightGate);
        }

        private void SpawnSingleGate(Vector3 position, string gateData)
        {
            string op = "+";
            int val = 5;

            if (gateData.Length > 0)
            {
                char first = gateData[0];
                if (first == '+' || first == 'x' || first == '*' || first == '-' || first == '÷' || first == '/')
                {
                    op = first.ToString();
                    val = gateData.Length > 1 && int.TryParse(gateData.Substring(1), out int v) ? v : 5;
                }
                else
                {
                    op = "+";
                    val = int.TryParse(gateData, out int v) ? v : 5;
                }
            }

            GameObject gate = Instantiate(gatePrefab, position, Quaternion.identity);
            gate.transform.localScale = new Vector3(8f, 4f, 1f);
            gate.AddComponent<WorldMover>();

            GateController gateCtrl = gate.GetComponent<GateController>();
            if (gateCtrl != null)
            {
                gateCtrl.SetGateData(op, val);
            }
        }

        private void SpawnEnemy(Vector3 position, string enemyType, int count)
        {
            position.y = 0f;

            GameObject enemyGroupObj = new GameObject($"Enemy_{enemyType}");
            enemyGroupObj.transform.position = position;

            EnemyGroup enemy = enemyGroupObj.AddComponent<EnemyGroup>();
            enemy.SetEnemyCount(count);

            enemyGroupObj.AddComponent<WorldMover>();

            SpawnMultiRingFormation(enemyGroupObj.transform, count, enemy);

            Debug.Log($"[LevelLoader] Enemy at {position}: {enemyType} x{count}");
        }

        private void SpawnObstacle(Vector3 position, string obstacleType, string damageValue)
        {
            string typeLower = obstacleType.ToLower();

            if (typeLower == "hammer")
            {
                SpawnHammer(position, damageValue);
                return;
            }

            if (typeLower == "saw")
            {
                SpawnSaw(position, damageValue);
                return;
            }

            if (typeLower == "spinner")
            {
                SpawnSpinner(position, damageValue);
                return;
            }

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

        private void SpawnSaw(Vector3 position, string damageValue)
        {
            GameObject root = new GameObject("Saw_Obstacle");
            root.transform.position = position;
            root.AddComponent<SawController>();
            root.AddComponent<WorldMover>();

            // 1. Blade body (main disc)
            GameObject blade = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            blade.name = "SawBlade";
            blade.transform.SetParent(root.transform);
            blade.transform.localPosition = Vector3.zero;
            blade.transform.localScale = new Vector3(1.8f, 0.06f, 1.8f);
            Renderer bladeR = blade.GetComponent<Renderer>();
            if (bladeR != null)
            {
                bladeR.material.color = new Color(0.65f, 0.65f, 0.65f);
                bladeR.material.SetFloat("_Metallic", 0.9f);
                bladeR.material.SetFloat("_Smoothness", 0.5f);
            }
            Destroy(blade.GetComponent<Collider>());

            // 2. Blade teeth around the edge
            int toothCount = 12;
            for (int i = 0; i < toothCount; i++)
            {
                float angle = (360f / toothCount) * i * Mathf.Deg2Rad;
                float radius = 0.95f;
                GameObject tooth = GameObject.CreatePrimitive(PrimitiveType.Cube);
                tooth.name = $"SawTooth_{i}";
                tooth.transform.SetParent(root.transform);
                tooth.transform.localPosition = new Vector3(
                    Mathf.Cos(angle) * radius,
                    0f,
                    Mathf.Sin(angle) * radius
                );
                tooth.transform.localScale = new Vector3(0.12f, 0.06f, 0.2f);
                tooth.transform.localRotation = Quaternion.Euler(0f, (360f / toothCount) * i, 0f);
                Renderer toothR = tooth.GetComponent<Renderer>();
                if (toothR != null)
                {
                    toothR.material.color = new Color(0.5f, 0.5f, 0.5f);
                    toothR.material.SetFloat("_Metallic", 0.8f);
                }
                Destroy(tooth.GetComponent<Collider>());
            }

            // 3. Center hub
            GameObject hub = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            hub.name = "SawHub";
            hub.transform.SetParent(root.transform);
            hub.transform.localPosition = new Vector3(0f, 0.04f, 0f);
            hub.transform.localScale = new Vector3(0.3f, 0.08f, 0.3f);
            Renderer hubR = hub.GetComponent<Renderer>();
            if (hubR != null)
            {
                hubR.material.color = new Color(0.3f, 0.3f, 0.3f);
                hubR.material.SetFloat("_Metallic", 0.6f);
            }
            Destroy(hub.GetComponent<Collider>());

            // 4. Inner ring detail (thin cylinder)
            GameObject innerRing = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            innerRing.name = "SawInnerRing";
            innerRing.transform.SetParent(root.transform);
            innerRing.transform.localPosition = new Vector3(0f, -0.03f, 0f);
            innerRing.transform.localScale = new Vector3(1.0f, 0.02f, 1.0f);
            Renderer ringR = innerRing.GetComponent<Renderer>();
            if (ringR != null)
            {
                ringR.material.color = new Color(0.4f, 0.4f, 0.4f);
                ringR.material.SetFloat("_Metallic", 0.7f);
            }
            Destroy(innerRing.GetComponent<Collider>());

            // 5. Trigger collider on root
            SphereCollider trigger = root.AddComponent<SphereCollider>();
            trigger.isTrigger = true;
            trigger.radius = 1f;

            ObstacleController obstacleCtrl = root.AddComponent<ObstacleController>();
            obstacleCtrl.SetObstacleType(ObstacleType.Saw);
            if (int.TryParse(damageValue, out int damage))
            {
                obstacleCtrl.SetDamage(damage);
            }

            Debug.Log($"[LevelLoader] Saw at {position}, damage: {damageValue}");
        }

        private void SpawnWall(Vector3 position, string gapPos, string damageValue)
        {
            float roadW = GetTotalWidth();
            float wallHeight = 2f;
            float wallThickness = 0.4f;
            float gapWidth = roadW * 0.35f;
            float halfRoad = roadW * 0.5f;

            float gapCenter = 0f;
            string gapLower = gapPos.ToLower();
            if (gapLower == "left") gapCenter = -halfRoad + gapWidth;
            else if (gapLower == "right") gapCenter = halfRoad - gapWidth;
            else gapCenter = 0f;

            float leftEdge = gapCenter - gapWidth / 2f;
            float rightEdge = gapCenter + gapWidth / 2f;

            void BuildSegment(float segLeft, float segRight)
            {
                if (segRight <= segLeft) return;
                float segWidth = segRight - segLeft;
                float segCenter = (segLeft + segRight) / 2f;

                GameObject segment = GameObject.CreatePrimitive(PrimitiveType.Cube);
                segment.name = $"WallSegment_{gapPos}";
                segment.transform.position = new Vector3(segCenter, wallHeight / 2f, position.z);
                segment.transform.localScale = new Vector3(segWidth, wallHeight, wallThickness);

                Renderer r = segment.GetComponent<Renderer>();
                if (r != null) r.material.color = new Color(0.25f, 0.25f, 0.25f);

                Collider col = segment.GetComponent<Collider>();
                if (col != null) col.isTrigger = true;

                ObstacleController ctrl = segment.AddComponent<ObstacleController>();
                ctrl.SetObstacleType(ObstacleType.Wall);
                if (int.TryParse(damageValue, out int damage))
                {
                    ctrl.SetDamage(damage);
                }

                segment.AddComponent<WorldMover>();
            }

            BuildSegment(-halfRoad, leftEdge);
            BuildSegment(rightEdge, halfRoad);

            Debug.Log($"[LevelLoader] Wall at {position}, gap: {gapPos}");
        }

        private void SpawnSpinner(Vector3 position, string damageValue)
        {
            float roadW = GetTotalWidth();
            float barLength = roadW * 0.8f;

            GameObject root = new GameObject("Spinner_Obstacle");
            root.transform.position = new Vector3(0f, 1.5f, position.z);
            root.AddComponent<WorldMover>();
            root.AddComponent<SpinnerController>();

            // Bar (visual only, collider on root)
            GameObject bar = GameObject.CreatePrimitive(PrimitiveType.Cube);
            bar.name = "SpinnerBar";
            bar.transform.SetParent(root.transform);
            bar.transform.localPosition = Vector3.zero;
            bar.transform.localScale = new Vector3(barLength, 0.3f, 0.5f);

            Renderer renderer = bar.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = new Color(1f, 0.5f, 0f);
                renderer.material.SetFloat("_Metallic", 0.5f);
            }
            Destroy(bar.GetComponent<Collider>());

            // Trigger + obstacle logic on root
            BoxCollider trigger = root.AddComponent<BoxCollider>();
            trigger.isTrigger = true;
            trigger.size = new Vector3(barLength, 0.6f, 0.8f);

            ObstacleController ctrl = root.AddComponent<ObstacleController>();
            ctrl.SetObstacleType(ObstacleType.Spinner);
            if (int.TryParse(damageValue, out int damage))
            {
                ctrl.SetDamage(damage);
            }

            Debug.Log($"[LevelLoader] Spinner at {position}, damage: {damageValue}");
        }

        private void SpawnHammer(Vector3 position, string damageValue)
        {
            // 1. Root (회전축)
            GameObject hammerRoot = new GameObject("Hammer_Obstacle");
            hammerRoot.transform.position = position + new Vector3(0f, 10f, 0f); // 도로 위쪽에 배치
            hammerRoot.AddComponent<WorldMover>();
            HammerController controller = hammerRoot.AddComponent<HammerController>();

            // 2. Handle (자루)
            GameObject handle = GameObject.CreatePrimitive(PrimitiveType.Cube);
            handle.name = "Handle";
            handle.transform.SetParent(hammerRoot.transform);
            handle.transform.localPosition = new Vector3(0f, -5f, 0f);
            handle.transform.localScale = new Vector3(0.5f, 10f, 0.5f);
            
            Renderer handleRenderer = handle.GetComponent<Renderer>();
            if (handleRenderer != null) handleRenderer.material.color = new Color(0.4f, 0.2f, 0f); // 나무색

            // 3. Head (망치 머리)
            GameObject head = GameObject.CreatePrimitive(PrimitiveType.Cube);
            head.name = "Head";
            head.transform.SetParent(hammerRoot.transform);
            head.transform.localPosition = new Vector3(0f, -10f, 0f);
            
            // 도로 너비(60m)를 고려하여 망치 머리 크기 조절
            float headSize = 6f; 
            head.transform.localScale = new Vector3(headSize, 3f, 3f);

            Renderer headRenderer = head.GetComponent<Renderer>();
            if (headRenderer != null) headRenderer.material.color = Color.gray;

            // 4. 피해 로직 추가
            ObstacleController obstacleCtrl = head.AddComponent<ObstacleController>();
            if (int.TryParse(damageValue, out int damage))
            {
                obstacleCtrl.SetDamage(damage);
            }

            // 회전 각도 조절 (도로 끝까지 닿을 수 있게)
            controller.SetSettings(2.5f, 60f);

            Debug.Log($"[LevelLoader] Hammer spawned at {position}");
        }

        private void OnVictorySequence()
        {
            Debug.Log("[LevelLoader] OnVictorySequence called");
            StartCoroutine(VictoryStaircaseRoutine());
        }

        private IEnumerator VictoryStaircaseRoutine()
        {
            Debug.Log("[LevelLoader] VictoryStaircaseRoutine started");
            yield return new WaitForSecondsRealtime(0.3f);

            if (hasBossInLevel)
            {
                Debug.Log("[LevelLoader] Boss level, skipping staircase");
                yield return new WaitForSecondsRealtime(1f);
                Time.timeScale = 1f;
                GameManager.AdvanceLevel();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                yield break;
            }

            CrowdController crowd = FindFirstObjectByType<CrowdController>();
            int totalFollowers = crowd != null ? crowd.CurrentCount : 0;
            int totalFollowerCount = crowd != null ? crowd.TotalCount : 0;
            Debug.Log($"[LevelLoader] Staircase: crowd={crowd != null}, followers={totalFollowers}, total={totalFollowerCount}");

            if (crowd == null || totalFollowers <= 0)
            {
                Debug.Log("[LevelLoader] No followers, skipping staircase");
                yield return new WaitForSecondsRealtime(1f);
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.SendMessage("ShowLevelClearUI", totalFollowers);
                }
                yield return new WaitForSecondsRealtime(1.5f);
                Time.timeScale = 1f;
                GameManager.AdvanceLevel();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                yield break;
            }

            WorldMover[] movers = FindObjectsByType<WorldMover>(FindObjectsSortMode.None);
            foreach (var m in movers) m.SetSpeed(0f);

            EnemyGroup[] enemyGroups = FindObjectsByType<EnemyGroup>(FindObjectsSortMode.None);
            foreach (var g in enemyGroups) Destroy(g.gameObject);

            EnemyMinion[] minions = FindObjectsByType<EnemyMinion>(FindObjectsSortMode.None);
            foreach (var m in minions) Destroy(m.gameObject);

            ObstacleController[] obstacles = FindObjectsByType<ObstacleController>(FindObjectsSortMode.None);
            foreach (var o in obstacles) Destroy(o.gameObject);

            BossController[] bosses = FindObjectsByType<BossController>(FindObjectsSortMode.None);
            foreach (var b in bosses) Destroy(b.gameObject);

            Vector3 finishPos = lastFinishLineObject != null
                ? lastFinishLineObject.transform.position
                : new Vector3(0f, 0f, 8f);
            float roadW = GetTotalWidth();
            float pivotX = finishPos.x;
            float pivotZ = finishPos.z + roadW * 0.4f;
            float pivotY = 0f;
            float stepRise = 0.5f;
            float stepBack = 0.9f;
            float unitSpacingZ = 0.62f;

            stepWidths.Clear();
            stepSpacingsX.Clear();
            stepCols.Clear();
            stepDepths.Clear();

            int stepsNeeded = 0;
            int rem = totalFollowers;
            while (rem > 0 && stepsNeeded < 60)
            {
                int cols = GetStairCols(stepsNeeded);
                int rows = GetStairRows(stepsNeeded);
                rem -= cols * rows;
                stepsNeeded++;
            }

            stepsNeeded = Mathf.Max(stepsNeeded, GetMinimumVisibleStairCount(totalFollowers));

            Debug.Log($"[LevelLoader] Steps needed: {stepsNeeded}");

            if (GameManager.Instance != null)
            {
                GameManager.Instance.StartCoroutine(ZoomOutForStaircase(stepsNeeded));
            }

            for (int s = 0; s < stepsNeeded; s++)
            {
                int cols = GetStairCols(s);
                int rows = GetStairRows(s);
                float stepW = roadW * GetStairWidthRatio(s, stepsNeeded);
                float stepD = Mathf.Max(0.6f, rows * unitSpacingZ + 0.2f);
                float centerZ = pivotZ + s * stepBack;
                float spacingX = stepW / (cols + 1);
                float stepY = pivotY + s * stepRise;

                // Tread (horizontal surface)
                GameObject tread = GameObject.CreatePrimitive(PrimitiveType.Cube);
                tread.name = $"Tread_{s}";
                tread.transform.localScale = new Vector3(stepW, 0.2f, stepD);
                tread.transform.position = new Vector3(pivotX, stepY, centerZ);
                Renderer treadR = tread.GetComponent<Renderer>();
                if (treadR != null) treadR.material = GetGoldEmissionMaterial();
                Destroy(tread.GetComponent<Collider>());

                // Riser (vertical front face, not for first step which sits on ground)
                if (s > 0)
                {
                    float riserHeight = stepRise;
                    float riserThickness = 0.08f;
                    float riserY = stepY - riserHeight / 2f;
                    float riserZ = centerZ - stepD / 2f;

                    GameObject riser = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    riser.name = $"Riser_{s}";
                    riser.transform.localScale = new Vector3(stepW, riserHeight, riserThickness);
                    riser.transform.position = new Vector3(pivotX, riserY, riserZ);
                    Renderer riserR = riser.GetComponent<Renderer>();
                    if (riserR != null) riserR.material = GetGoldDarkMaterial();
                    Destroy(riser.GetComponent<Collider>());
                }

                // Side rails (left and right)
                float railWidth = 0.2f;
                float railHeight = 0.5f;
                float leftRailX = pivotX - stepW / 2f - railWidth / 2f;
                float rightRailX = pivotX + stepW / 2f + railWidth / 2f;

                GameObject leftRail = GameObject.CreatePrimitive(PrimitiveType.Cube);
                leftRail.name = $"Rail_L_{s}";
                leftRail.transform.localScale = new Vector3(railWidth, railHeight, stepD);
                leftRail.transform.position = new Vector3(leftRailX, stepY + railHeight / 2f, centerZ);
                Renderer lr = leftRail.GetComponent<Renderer>();
                if (lr != null) lr.material = GetGoldDarkMaterial();
                Destroy(leftRail.GetComponent<Collider>());

                GameObject rightRail = GameObject.CreatePrimitive(PrimitiveType.Cube);
                rightRail.name = $"Rail_R_{s}";
                rightRail.transform.localScale = new Vector3(railWidth, railHeight, stepD);
                rightRail.transform.position = new Vector3(rightRailX, stepY + railHeight / 2f, centerZ);
                Renderer rr = rightRail.GetComponent<Renderer>();
                if (rr != null) rr.material = GetGoldDarkMaterial();
                Destroy(rightRail.GetComponent<Collider>());

                BuildStairBlockDetails(s, cols, rows, pivotX, centerZ, stepY, stepW, stepD, stepRise, unitSpacingZ);

                float multVal = 1.0f + (s + 1) * 0.1f;
                GameObject labelObj = new GameObject($"StepLabel_{s}");
                labelObj.transform.SetParent(tread.transform);
                labelObj.transform.localPosition = new Vector3(0f, 0.3f, 0f);
                TextMesh labelText = labelObj.AddComponent<TextMesh>();
                labelText.text = $"\u00D7{multVal:F1}";
                labelText.fontSize = 100;
                labelText.characterSize = 0.12f;
                labelText.anchor = TextAnchor.MiddleCenter;
                labelText.alignment = TextAlignment.Center;
                labelText.fontStyle = FontStyle.Bold;
                labelText.color = Color.white;

                stepWidths.Add(stepW);
                stepSpacingsX.Add(spacingX);
                stepCols.Add(cols);
                stepDepths.Add(stepD);
                yield return new WaitForSecondsRealtime(0.06f);
            }

            yield return new WaitForSecondsRealtime(0.2f);

            int followerIdx = 0;
            highestMultiplier = 0;
            for (int s = 0; s < stepsNeeded; s++)
            {
                int cols = stepCols[s];
                int rows = GetStairRows(s);
                float centerZ = pivotZ + s * stepBack;
                float stepDuration = Mathf.Lerp(0.15f, 0.35f, (float)s / stepsNeeded);
                float spacingX = stepSpacingsX[s];
                float stepY = pivotY + s * stepRise;

                for (int r = 0; r < rows && followerIdx < totalFollowers; r++)
                {
                    int[] colOrder = new int[cols];
                    int mid = cols / 2;
                    for (int i = 0; i < cols; i++)
                    {
                        colOrder[i] = (i % 2 == 0) ? mid + i / 2 : mid - (i + 1) / 2;
                        if (colOrder[i] < 0) colOrder[i] = cols - 1 + colOrder[i];
                        if (colOrder[i] >= cols) colOrder[i] = 0;
                    }

                    for (int ci = 0; ci < cols && followerIdx < totalFollowers; ci++, followerIdx++)
                    {
                        int c = colOrder[ci];
                        GameObject f = crowd.GetFollowerAtIndex(followerIdx);
                        if (f == null) continue;

                        FollowerComponent fc = f.GetComponent<FollowerComponent>();
                        if (fc != null) fc.isDueling = true;

                        float xOff = (c - (cols - 1) * 0.5f) * spacingX;
                        float zOff = (r - (rows - 1) * 0.5f) * unitSpacingZ;
                        float targetY = stepY + 0.1f;

                        float stepMult = 1.0f + (s + 1) * 0.1f;
                        if (stepMult > highestMultiplier)
                        {
                            highestMultiplier = stepMult;
                        }

                        Vector3 endPos = new Vector3(
                            pivotX + xOff,
                            targetY,
                            centerZ + zOff
                        );

                        // Individual follower block
                        GameObject block = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        block.name = $"FollowerBlock_{followerIdx}";
                        float blockSize = 0.35f;
                        float blockHeight = 0.15f;
                        block.transform.localScale = new Vector3(blockSize, blockHeight, blockSize);
                        block.transform.position = new Vector3(endPos.x, stepY, endPos.z);
                        Renderer blockR = block.GetComponent<Renderer>();
                        if (blockR != null) blockR.material = GetGoldEmissionMaterial();
                        Destroy(block.GetComponent<Collider>());

                        StartCoroutine(HopToPosition(f, endPos, stepDuration));
                        yield return new WaitForSecondsRealtime(0.02f);
                    }
                }
            }

            // Crown ornament at the top
            if (stepsNeeded > 0)
            {
                int topS = stepsNeeded - 1;
                float topY = pivotY + topS * stepRise;
                float topZ = pivotZ + topS * stepBack;
                float crownY = topY + 0.3f;

                // Crown base ring
                GameObject crownBase = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                crownBase.name = "Crown_Base";
                crownBase.transform.localScale = new Vector3(1.6f, 0.2f, 1.6f);
                crownBase.transform.position = new Vector3(pivotX, crownY, topZ);
                Renderer cbR = crownBase.GetComponent<Renderer>();
                if (cbR != null) cbR.material = GetGoldEmissionMaterial();
                Destroy(crownBase.GetComponent<Collider>());

                // Crown top ring (smaller)
                GameObject crownTop = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                crownTop.name = "Crown_Top";
                crownTop.transform.localScale = new Vector3(1.1f, 0.15f, 1.1f);
                crownTop.transform.position = new Vector3(pivotX, crownY + 0.35f, topZ);
                Renderer ctR = crownTop.GetComponent<Renderer>();
                if (ctR != null) ctR.material = GetGoldEmissionMaterial();
                Destroy(crownTop.GetComponent<Collider>());

                // Crown spikes
                int spikeCount = 5;
                for (int i = 0; i < spikeCount; i++)
                {
                    float angle = (360f / spikeCount) * i * Mathf.Deg2Rad;
                    float radius = 0.75f;
                    GameObject spike = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    spike.name = $"Crown_Spike_{i}";
                    spike.transform.localScale = new Vector3(0.1f, 0.45f, 0.1f);
                    spike.transform.position = new Vector3(
                        pivotX + Mathf.Cos(angle) * radius,
                        crownY + 0.3f,
                        topZ + Mathf.Sin(angle) * radius
                    );
                    spike.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                    Renderer spikeR = spike.GetComponent<Renderer>();
                    if (spikeR != null) spikeR.material = GetGoldEmissionMaterial();
                    Destroy(spike.GetComponent<Collider>());
                }

                // Crown center orb
                GameObject orb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                orb.name = "Crown_Orb";
                orb.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                orb.transform.position = new Vector3(pivotX, crownY + 0.55f, topZ);
                Renderer orbR = orb.GetComponent<Renderer>();
                if (orbR != null) orbR.material = GetGoldEmissionMaterial();
                Destroy(orb.GetComponent<Collider>());
            }

            yield return new WaitForSecondsRealtime(1f);

            if (GameManager.Instance != null)
            {
                float mult = highestMultiplier > 0 ? highestMultiplier : 1.1f;
                int finalScore = Mathf.RoundToInt(totalFollowerCount * mult);
                GameManager.Instance.ShowLevelClearResult(totalFollowerCount, mult, finalScore);
                GameManager.Instance.SendMessage("UpdateLevelNumberDisplay");
            }

            yield return new WaitForSecondsRealtime(1.5f);

            Time.timeScale = 1f;
            GameManager.AdvanceLevel();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private IEnumerator HopToPosition(GameObject obj, Vector3 land, float duration)
        {
            if (obj == null) yield break;

            Vector3 start = obj.transform.position;
            float heightDiff = Mathf.Abs(land.y - start.y);
            float hopHeight = Mathf.Max(0.8f, heightDiff * 1.5f + 0.5f);
            float elapsed = 0f;

            while (elapsed < duration)
            {
                if (obj == null) yield break;
                elapsed += Time.unscaledDeltaTime;
                float t = Mathf.Clamp01(elapsed / duration);

                Vector3 pos = Vector3.Lerp(start, land, t);
                float arc = hopHeight * Mathf.Sin(t * Mathf.PI);
                arc *= 1f + 0.3f * Mathf.Sin(t * Mathf.PI * 3f);
                pos.y += arc;

                obj.transform.position = pos;

                yield return null;
            }

            if (obj != null) obj.transform.position = land;
        }

        private IEnumerator ZoomOutForStaircase(int stepCount)
        {
            yield return new WaitForSecondsRealtime(0.1f);

            Camera cam = Camera.main;
            if (cam == null) yield break;

            Vector3 startPos = cam.transform.position;
            Vector3 startAngles = cam.transform.eulerAngles;
            float pyramidHeight = stepCount * 0.5f;
            float pyramidDepth = stepCount * 0.9f;
            Vector3 targetPos = startPos + new Vector3(0f, pyramidHeight * 0.65f + 3f, pyramidDepth * -0.38f - 4f);
            Vector3 targetAngles = new Vector3(
                Mathf.Lerp(startAngles.x, 18f, 0.55f),
                startAngles.y,
                startAngles.z
            );
            float duration = 2.4f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                cam.transform.position = Vector3.Lerp(startPos, targetPos, t);
                cam.transform.eulerAngles = Vector3.Lerp(startAngles, targetAngles, t);
                yield return null;
            }

            cam.transform.position = targetPos;
            cam.transform.eulerAngles = targetAngles;
        }
    }
}
