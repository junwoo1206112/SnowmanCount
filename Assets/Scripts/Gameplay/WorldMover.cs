using UnityEngine;

using SnowmanCount.Core;

namespace SnowmanCount.Gameplay
{
    public class WorldMover : MonoBehaviour
    {
        public static float totalDistanceTraveled { get; private set; }

        public static void ResetDistance()
        {
            totalDistanceTraveled = 0f;
        }

        [Header("Movement")]
        [SerializeField] private float moveSpeed = 15f;
        [SerializeField] private float destroyZ = -20f;

        private bool canMove;

        public void SetSpeed(float speed)
        {
            moveSpeed = speed;
        }

        private void Start()
        {
            if (GameStateManager.Instance != null)
            {
                GameStateManager.Instance.OnStateChanged += OnGameStateChanged;
                canMove = GameStateManager.Instance.CurrentState == GameState.Play;
            }
            else
            {
                canMove = false;
            }
        }

        private void OnDisable()
        {
            if (GameStateManager.Instance != null)
            {
                GameStateManager.Instance.OnStateChanged -= OnGameStateChanged;
            }
        }

        private void OnGameStateChanged(GameState newState)
        {
            canMove = newState == GameState.Play;
        }

        private void Update()
        {
            if (!canMove) return;

            float step = moveSpeed * Time.deltaTime;
            transform.Translate(Vector3.back * step, Space.World);
            totalDistanceTraveled += step;

            if (transform.position.z < destroyZ)
            {
                Destroy(gameObject);
            }
        }
    }
}
