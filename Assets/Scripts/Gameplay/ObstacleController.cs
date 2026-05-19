using UnityEngine;

namespace SnowmanCount.Gameplay
{
    public enum ObstacleType
    {
        Generic,
        Saw,
        Wall,
        Spinner,
        Hammer
    }

    public class ObstacleController : MonoBehaviour
    {
        [Header("Obstacle Settings")]
        [SerializeField] private ObstacleType obstacleType = ObstacleType.Generic;
        [SerializeField] private int damagePerHit = 1;

        private CrowdController crowdController;

        public void SetDamage(int damage)
        {
            damagePerHit = damage;
        }

        public void SetObstacleType(ObstacleType type)
        {
            obstacleType = type;
        }

        private void Start()
        {
            crowdController = FindFirstObjectByType<CrowdController>();
        }

        private void OnTriggerEnter(Collider other)
        {
            FollowerComponent follower = other.GetComponent<FollowerComponent>();
            if (follower == null) return;

            if (crowdController != null)
            {
                crowdController.RemoveSpecificFollower(other.gameObject);
                crowdController.NotifyCountChanged();

                Debug.Log($"[ObstacleController] {obstacleType} hit follower. Removed 1 follower.");
            }
        }
    }
}
