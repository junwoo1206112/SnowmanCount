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

            FollowerComponent follower = other.GetComponent<FollowerComponent>();
            if (follower == null && !other.CompareTag("Player")) return;

            hasTriggered = true;

            if (crowdController != null)
            {
                if (follower != null)
                {
                    crowdController.RemoveSpecificFollower(other.gameObject);
                    crowdController.NotifyCountChanged();
                }
                else if (other.CompareTag("Player"))
                {
                    crowdController.ApplyMathOperation("-", Mathf.Max(1, damagePerHit));
                }

                Debug.Log($"[ObstacleController] {obstacleType} hit {other.name}. Removed 1 follower.");
            }

            Destroy(gameObject);
        }
    }
}
