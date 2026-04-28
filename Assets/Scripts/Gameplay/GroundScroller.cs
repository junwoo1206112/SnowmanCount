using UnityEngine;

using SnowmanCount.Core;

namespace SnowmanCount.Gameplay
{
    public class GroundScroller : MonoBehaviour
    {
        [Header("Scroll Settings")]
        [SerializeField] private float baseScrollSpeed = 1.5f;
        [SerializeField] private string textureProperty = "_BaseMap";
        [SerializeField] private bool syncWithPlayerSpeed = true;

        private Material groundMaterial;
        private Vector2 offset;
        private SwerveMovement playerMovement;

        private void Awake()
        {
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null)
            {
                groundMaterial = renderer.material;
            }
            else
            {
                Debug.LogError("[GroundScroller] No Renderer found on Ground");
            }

            if (syncWithPlayerSpeed)
            {
                playerMovement = FindFirstObjectByType<SwerveMovement>();
            }
        }

        private void Update()
        {
            if (groundMaterial == null) return;

            if (GameStateManager.Instance != null &&
                GameStateManager.Instance.CurrentState != GameState.Play)
            {
                return;
            }

            float currentSpeed = baseScrollSpeed;

            if (syncWithPlayerSpeed && playerMovement != null)
            {
                currentSpeed = playerMovement.CurrentSpeed * 0.1f;
            }

            offset.y -= currentSpeed * Time.deltaTime;
            groundMaterial.SetTextureOffset(textureProperty, offset);
        }
    }
}
