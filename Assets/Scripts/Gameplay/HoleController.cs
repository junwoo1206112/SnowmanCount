using UnityEngine;

using SnowmanCount.Core;

namespace SnowmanCount.Gameplay
{
    public class HoleController : MonoBehaviour
    {
        private CrowdController crowdController;
        private bool hasTriggered;

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
                int current = crowdController.CurrentCount;
                int damage = Mathf.Max(1, current / 2);
                crowdController.ApplyMathOperation("-", damage);
                Debug.Log($"[HoleController] Half crowd lost: {damage}");
            }
        }
    }
}