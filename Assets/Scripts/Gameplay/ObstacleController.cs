using UnityEngine;

namespace SnowmanCount.Gameplay
{
    public class ObstacleController : MonoBehaviour
    {
        [Header("Obstacle Settings")]
        [SerializeField] private string obstacleType = "Saw";
        [SerializeField] private int damagePerHit = 1;

        private CrowdController crowdController;

        private void Start()
        {
            crowdController = FindFirstObjectByType<CrowdController>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            if (crowdController != null)
            {
                crowdController.ApplyMathOperation("-", damagePerHit);
                Debug.Log($"[ObstacleController] {obstacleType} dealt {damagePerHit} damage");
            }

            Destroy(gameObject);
        }
    }
}
