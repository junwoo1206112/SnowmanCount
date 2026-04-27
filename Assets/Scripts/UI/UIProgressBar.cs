using UnityEngine;
using UnityEngine.UI;

namespace SnowmanCount.UI
{
    public class UIProgressBar : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Image progressFill;
        [SerializeField] private float levelLength = 100f;

        [Header("Movement")]
        [SerializeField] private Transform playerPivot;

        private void Update()
        {
            if (progressFill == null || playerPivot == null) return;

            float progress = Mathf.Clamp01(playerPivot.position.z / levelLength);
            progressFill.fillAmount = progress;
        }
    }
}
