using UnityEngine;
using UnityEngine.InputSystem;

using SnowmanCount.Core;

namespace SnowmanCount.Gameplay
{
    public class SwerveMovement : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float forwardSpeed = 5f;
        [SerializeField] private float swerveSpeed = 20f;
        [SerializeField] private float xBound = 30f;
        public float XBound => xBound;

        private PlayerInput playerInput;
        private InputAction moveAction;
        private float swerveAmount;
        private bool isDragging;
        private bool canSwerve;

        public float CurrentSpeed => forwardSpeed;

        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>();

            if (playerInput != null)
            {
                moveAction = playerInput.actions["Move"];
            }
        }

        private void OnEnable()
        {
            if (moveAction != null)
            {
                moveAction.started += OnMoveStarted;
                moveAction.performed += OnMovePerformed;
                moveAction.canceled += OnMoveCanceled;
            }
        }

        private void Start()
        {
            if (GameStateManager.Instance != null)
            {
                GameStateManager.Instance.OnStateChanged += OnGameStateChanged;
                canSwerve = GameStateManager.Instance.CurrentState == GameState.Play;
            }
            else
            {
                canSwerve = false;
            }
        }

        private void OnDisable()
        {
            if (moveAction != null)
            {
                moveAction.started -= OnMoveStarted;
                moveAction.performed -= OnMovePerformed;
                moveAction.canceled -= OnMoveCanceled;
            }

            if (GameStateManager.Instance != null)
            {
                GameStateManager.Instance.OnStateChanged -= OnGameStateChanged;
            }
        }

        private void OnGameStateChanged(GameState newState)
        {
            canSwerve = newState == GameState.Play;
        }

        private void OnMoveStarted(InputAction.CallbackContext context)
        {
            if (!canSwerve) return;

            isDragging = true;
        }

        private void OnMovePerformed(InputAction.CallbackContext context)
        {
            if (!isDragging || !canSwerve) return;

            Vector2 input = context.ReadValue<Vector2>();
            swerveAmount = input.x * swerveSpeed;
        }

        private void OnMoveCanceled(InputAction.CallbackContext context)
        {
            swerveAmount = 0f;
            isDragging = false;
        }

        private void Update()
        {
            ApplySwerveOnly();
        }

        private void ApplySwerveOnly()
        {
            if (!canSwerve) return;

            Vector3 pos = transform.position;
            pos.x += swerveAmount * Time.deltaTime;
            pos.x = Mathf.Clamp(pos.x, -xBound, xBound);
            pos.y = 0.5f; // 도로(Y=0) 위에 발이 닿도록 높이 수정
            transform.position = pos;
        }
    }
}
