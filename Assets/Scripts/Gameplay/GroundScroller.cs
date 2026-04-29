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

        private void Start()
        {
            if (syncWithPlayerSpeed && playerMovement == null)
            {
                playerMovement = FindFirstObjectByType<SwerveMovement>();
            }

            AdjustGroundWidth();
        }

        private void AdjustGroundWidth()
        {
            if (playerMovement == null)
            {
                playerMovement = FindFirstObjectByType<SwerveMovement>();
            }

            if (playerMovement != null)
            {
                // 플레이어의 이동 범위 (예: xBound가 4라면 전체 폭은 8)
                float targetWidth = playerMovement.XBound * 2f;
                Vector3 currentScale = transform.localScale;

                // 유니티 기본 Plane은 1단위가 10m이므로 10으로 나눔
                if (gameObject.name.ToLower().Contains("plane"))
                {
                    currentScale.x = targetWidth / 10f;
                }
                else
                {
                    // 일반 큐브 등의 메쉬는 1단위가 1m이므로 그대로 적용
                    currentScale.x = targetWidth;
                }

                transform.localScale = currentScale;
                
                // 텍스처 타일링도 폭에 맞춰 조절하면 더 자연스럽습니다.
                if (groundMaterial != null)
                {
                    Vector2 tiling = groundMaterial.GetTextureScale(textureProperty);
                    tiling.x = currentScale.x * (gameObject.name.ToLower().Contains("plane") ? 10f : 1f);
                    groundMaterial.SetTextureScale(textureProperty, tiling);
                }

                Debug.Log($"[GroundScroller] Ground width forced to {targetWidth} (Scale.x: {currentScale.x}) to match Player xBound: {playerMovement.XBound}");
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
