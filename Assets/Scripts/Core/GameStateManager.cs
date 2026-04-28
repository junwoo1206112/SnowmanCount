using UnityEngine;

namespace SnowmanCount.Core
{
    public class GameStateManager : MonoBehaviour
    {
        public static GameStateManager Instance { get; private set; }

        public GameState CurrentState { get; private set; } = GameState.Ready;

        public delegate void StateChangedHandler(GameState newState);
        public event StateChangedHandler OnStateChanged;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        public void SetState(GameState newState)
        {
            if (CurrentState == newState) return;

            CurrentState = newState;
            Debug.Log($"[GameStateManager] State: {newState}");

            OnStateChanged?.Invoke(newState);
        }
    }
}
