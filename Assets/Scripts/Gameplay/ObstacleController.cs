using UnityEngine;

namespace SnowmanCount.Gameplay
{
    public class ObstacleController : MonoBehaviour
    {
        [Header("Obstacle Settings")]
        [SerializeField] private string obstacleType = "Saw";
        [SerializeField] private int damagePerHit = 1;

        private CrowdController crowdController;
        private bool hasTriggered;

        public void SetDamage(int damage)
        {
            damagePerHit = damage;
        }

        private void Start()
        {
            crowdController = FindFirstObjectByType<CrowdController>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (hasTriggered) return;
            if (!other.CompareTag("Player")) return;

            hasTriggered = true;

            if (crowdController != null)
            {
                int damage = Mathf.Max(1, Mathf.RoundToInt(crowdController.CurrentCount * 0.1f));
                crowdController.ApplyMathOperation("-", damage);
                Debug.Log($"[ObstacleController] {obstacleType} dealt {damage} damage (10% of {crowdController.CurrentCount})");
            }

            Destroy(gameObject);
        }
    }
}
