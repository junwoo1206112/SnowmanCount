using UnityEngine;
using UnityEngine.InputSystem;

namespace SnowmanCount.Gameplay
{
    public class SwerveMovement : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float forwardSpeed = 5f;
        [SerializeField] private float swerveSpeed = 8f;
        [SerializeField] private float xBound = 4f;

        private PlayerInput playerInput;
        private InputAction moveAction;
        private float swerveAmount;
        private bool isDragging;

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

        private void OnDisable()
        {
            if (moveAction != null)
            {
                moveAction.started -= OnMoveStarted;
                moveAction.performed -= OnMovePerformed;
                moveAction.canceled -= OnMoveCanceled;
            }
        }

        private void OnMoveStarted(InputAction.CallbackContext context)
        {
            isDragging = true;
        }

        private void OnMovePerformed(InputAction.CallbackContext context)
        {
            if (!isDragging) return;

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
            Vector3 pos = transform.position;
            pos.x += swerveAmount * Time.deltaTime;
            pos.x = Mathf.Clamp(pos.x, -xBound, xBound);
            transform.position = pos;
        }
    }
}
