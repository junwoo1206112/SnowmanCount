using UnityEngine;

namespace SnowmanCount.Gameplay
{
    public class EnemyGroup : MonoBehaviour
    {
        [Header("Enemy Settings")]
        [SerializeField] private int enemyCount = 3;
        [SerializeField] private float spreadRadius = 2f;
        [SerializeField] private string enemyType = "Flame";

        private CrowdController crowdController;
        private EnemyAllClearDetector clearDetector;
        private bool hasTriggered;

        public void SetEnemyCount(int count)
        {
            enemyCount = count;
        }

        private void Start()
        {
            crowdController = FindFirstObjectByType<CrowdController>();
            clearDetector = FindFirstObjectByType<EnemyAllClearDetector>();

            for (int i = 0; i < transform.childCount && i < enemyCount; i++)
            {
                Vector3 offset = new Vector3(
                    UnityEngine.Random.Range(-spreadRadius, spreadRadius),
                    0f,
                    UnityEngine.Random.Range(-spreadRadius, spreadRadius)
                );
                transform.GetChild(i).localPosition = offset;
            }
        }

        private void OnDestroy()
        {
            if (clearDetector != null)
            {
                clearDetector.UnregisterEnemy();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (hasTriggered) return;
            if (!other.CompareTag("Player")) return;

            hasTriggered = true;
            Debug.Log($"[EnemyGroup] Collided! Faction: {enemyType}, Count: {enemyCount}");

            if (crowdController != null)
            {
                int damage = Mathf.Max(1, Mathf.RoundToInt(crowdController.CurrentCount * 0.1f));
                crowdController.ApplyMathOperation("-", damage);
            }

            Destroy(gameObject);
        }
    }
}
