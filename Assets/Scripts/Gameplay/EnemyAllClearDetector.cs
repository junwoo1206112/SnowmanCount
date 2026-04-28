using UnityEngine;

using SnowmanCount.Core;

namespace SnowmanCount.Gameplay
{
    public class EnemyAllClearDetector : MonoBehaviour
    {
        private int activeEnemyCount;

        public void RegisterEnemy()
        {
            activeEnemyCount++;
        }

        public void UnregisterEnemy()
        {
            activeEnemyCount--;

            if (activeEnemyCount <= 0)
            {
                CrowdController crowd = FindFirstObjectByType<CrowdController>();
                int crowdCount = crowd != null ? crowd.CurrentCount : -1;

                if (GameManager.Instance != null)
                {
                    GameManager.Instance.OnLevelCleared(crowdCount);
                }
            }
        }
    }
}