using UnityEngine;

using SnowmanCount.Core;

namespace SnowmanCount.Gameplay
{
    public class EnemyGroup : MonoBehaviour
    {
        [Header("Enemy Settings")]
        [SerializeField] private int enemyCount = 3;
        private int remainingMinions;

        private EnemyAllClearDetector clearDetector;
        private bool isRegistered = false;

        public void SetEnemyCount(int count)
        {
            enemyCount = count;
            remainingMinions = count;
        }

        public void RegisterAsWave()
        {
            clearDetector = FindFirstObjectByType<EnemyAllClearDetector>();
            if (clearDetector != null)
            {
                clearDetector.RegisterEnemy();
                isRegistered = true;
            }
        }

        private void Start()
        {
            // 부모의 트리거는 이제 필요 없음 (개별 미니언이 처리)
            Collider col = GetComponent<Collider>();
            if (col != null) col.enabled = false;
        }

        public void OnMinionDefeated()
        {
            remainingMinions--;

            Transform label = transform.Find("EnemyCountLabel");
            if (label != null)
            {
                TextMesh tm = label.GetComponent<TextMesh>();
                if (tm != null) tm.text = remainingMinions.ToString();
            }

            if (remainingMinions <= 0)
            {
                if (isRegistered && clearDetector != null)
                {
                    clearDetector.UnregisterEnemy();
                }
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            // 아직 살아있는 상태에서 파괴될 경우(예: 레벨 전환)
            if (remainingMinions > 0 && isRegistered && clearDetector != null)
            {
                // clearDetector.UnregisterEnemy(); // 필요 시 추가
            }
        }
    }
}
