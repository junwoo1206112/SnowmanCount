using UnityEngine;
using UnityEngine.UI;

using SnowmanCount.Gameplay;

namespace SnowmanCount.UI
{
    public class UIProgressBar : MonoBehaviour
    {
        public static UIProgressBar Instance { get; private set; }

        [Header("UI References")]
        [SerializeField] private Image progressFill;

        [Header("Settings")]
        [SerializeField] private float levelLength = 100f;

        private void Awake()
        {
            Instance = this;
        }

        public void SetLevelLength(float length)
        {
            levelLength = length;

            if (levelLength <= 0f)
            {
                levelLength = 100f;
            }
        }

        private void Update()
        {
            if (progressFill == null) return;

            if (levelLength <= 0f) return;

            float progress = Mathf.Clamp01(Gameplay.WorldMover.totalDistanceTraveled / levelLength);
            progressFill.fillAmount = progress;
        }
    }
}
