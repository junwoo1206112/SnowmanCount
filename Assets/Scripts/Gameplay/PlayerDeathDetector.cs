using UnityEngine;

using SnowmanCount.Core;

namespace SnowmanCount.Gameplay
{
    [RequireComponent(typeof(Collider))]
    public class PlayerDeathDetector : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Enemy"))
            {
                Debug.Log("[PlayerDeathDetector] Leader collided with hazard - Game Over");

                if (GameManager.Instance != null)
                {
                    GameManager.Instance.OnLeaderDied();
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Obstacle") || other.CompareTag("Enemy"))
            {
                Debug.Log("[PlayerDeathDetector] Leader entered hazard trigger - Game Over");

                if (GameManager.Instance != null)
                {
                    GameManager.Instance.OnLeaderDied();
                }
            }
        }
    }
}
