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
        private float highestMultiplier;

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

        private void SpawnLevel(LevelData data)
        {
            if (data == null) return;
            currentLevelData = data;

            float maxDistance = 0f;
            foreach (var row in data.rows) if (row.distance > maxDistance) maxDistance = row.distance;
            // 마지막 적 이후로도 조금 더 도로를 깔아줌
            maxDistance += 50f;

            CreateAllClearDetector();

            // --- 도로 타일 생성 (카운트 마스터 방식) ---
            float segmentLength = 5f; // 5m 단위로 도로 조각 생성
            for (float z = 0; z < maxDistance; z += segmentLength)
            {
                HoleType hole = GetHoleTypeAt(z, data.rows);
                
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
                if (row.objectType.ToLower() == "hole") continue; // 구멍은 타일 시스템에서 처리
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

            float xBound = 15f;
            SwerveMovement sm = FindFirstObjectByType<SwerveMovement>();
            if (sm != null) xBound = sm.XBound;

            HoleType hole = GetHoleTypeAt(position.z, currentLevelData.rows);
            
            float leftX, rightX;
            float targetY = 2.0f;

            if (hole == HoleType.Left)
            {
                leftX = xBound * 0.2f;
                rightX = xBound * 0.8f;
            }
            else if (hole == HoleType.Right)
            {
                leftX = -xBound * 0.8f;
                rightX = -xBound * 0.2f;
            }
            else
            {
                float spacing = xBound * 0.5f; 
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
            if (obstacleType.ToLower() == "hammer")
            {
                SpawnHammer(position, damageValue);
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
            StartCoroutine(VictoryStaircaseRoutine());
        }

        private IEnumerator VictoryStaircaseRoutine()
        {
            yield return new WaitForSecondsRealtime(0.3f);

            if (hasBossInLevel)
            {
                yield return new WaitForSecondsRealtime(1f);
                Time.timeScale = 1f;
                GameManager.AdvanceLevel();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                yield break;
            }

            CrowdController crowd = FindFirstObjectByType<CrowdController>();
            int totalFollowers = crowd != null ? crowd.CurrentCount : 0;
            int totalFollowerCount = crowd != null ? crowd.TotalCount : 0;

            if (crowd == null || totalFollowers <= 0)
            {
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
            float stepRise = 0.55f;
            float stepBack = 0.8f;
            float unitSpacingZ = 0.55f;

            int stepsNeeded = 0;
            int rem = totalFollowers;
            while (rem > 0 && stepsNeeded < 16)
            {
                int cols = Mathf.Max(1, 14 - stepsNeeded);
                int rows = Mathf.Max(1, 6 - stepsNeeded / 2);
                rem -= cols * rows;
                stepsNeeded++;
            }

            if (GameManager.Instance != null)
            {
                GameManager.Instance.StartCoroutine(ZoomOutForStaircase(stepsNeeded));
            }

            float maxCols = 14f;
            for (int s = 0; s < stepsNeeded; s++)
            {
                int cols = Mathf.Max(1, 14 - s);
                int rows = Mathf.Max(1, 6 - s / 2);
                float ratio = cols / maxCols;
                float stepW = roadW * 0.85f * ratio;
                float stepD = Mathf.Max(0.6f, rows * unitSpacingZ + 0.2f);
                float centerZ = pivotZ + s * stepBack;
                float spacingX = stepW / (cols + 1);

                GameObject step = GameObject.CreatePrimitive(PrimitiveType.Cube);
                step.name = $"StairStep_{s}";
                step.transform.localScale = new Vector3(stepW, 0.2f, stepD);
                step.transform.position = new Vector3(pivotX, pivotY + s * stepRise, centerZ);

                Renderer r = step.GetComponent<Renderer>();
                if (r != null)
                {
                    r.material.color = new Color(1f, 0.78f, 0.06f);
                    r.material.SetFloat("_Metallic", 0.6f);
                }
                Destroy(step.GetComponent<Collider>());

                float multVal = 1.0f + (s + 1) * 0.1f;
                GameObject labelObj = new GameObject($"StepLabel_{s}");
                labelObj.transform.SetParent(step.transform);
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
                yield return new WaitForSecondsRealtime(0.06f);
            }

            yield return new WaitForSecondsRealtime(0.2f);

            int followerIdx = 0;
            highestMultiplier = 0;
            for (int s = 0; s < stepsNeeded; s++)
            {
                int cols = stepCols[s];
                int rows = Mathf.Max(1, 6 - s / 2);
                float centerZ = pivotZ + s * stepBack;
                float stepDuration = Mathf.Lerp(0.15f, 0.35f, (float)s / stepsNeeded);
                float spacingX = stepSpacingsX[s];

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
                        float targetY = pivotY + s * stepRise + 0.1f;

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

                        StartCoroutine(HopToPosition(f, endPos, stepDuration));
                        yield return new WaitForSecondsRealtime(0.02f);
                    }
                }
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
            float pyramidHeight = stepCount * 0.6f;
            float pyramidDepth = stepCount * 0.9f;
            Vector3 targetPos = startPos + new Vector3(0f, pyramidHeight * 0.7f + 2f, -4f);
            Vector3 targetAngles = new Vector3(
                Mathf.Lerp(startAngles.x, 10f, 0.5f),
                startAngles.y,
                startAngles.z
            );
            float duration = 1.5f;
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
