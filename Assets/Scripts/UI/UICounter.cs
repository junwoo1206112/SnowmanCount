using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace SnowmanCount.UI
{
    public class UICounter : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Text counterText;
        [SerializeField] private Text gameOverText;
        [SerializeField] private string prefix = "Snow Power: ";

        [Header("Animation")]
        [SerializeField] private float popScale = 1.3f;
        [SerializeField] private float popDuration = 0.2f;

        private Coroutine popRoutine;

        private void Start()
        {
            if (counterText == null)
            {
                counterText = GetComponent<Text>();

                if (counterText == null)
                {
                    Debug.LogError("[UICounter] No Text component found");
                }
            }

            if (gameOverText != null)
            {
                gameOverText.gameObject.SetActive(false);
            }

            UpdateText(0);
        }

        public void UpdateCount(int newCount)
        {
            UpdateText(newCount);

            if (popRoutine != null)
            {
                StopCoroutine(popRoutine);
            }

            popRoutine = StartCoroutine(PlayPopRoutine());
        }

        private void UpdateText(int count)
        {
            if (counterText != null)
            {
                counterText.text = $"{prefix}{count}";
            }
        }

        private IEnumerator PlayPopRoutine()
        {
            if (counterText == null) yield break;

            float elapsed = 0f;

            while (elapsed < popDuration)
            {
                float t = elapsed / popDuration;
                float scale = Mathf.Lerp(1f, popScale, t);
                counterText.transform.localScale = Vector3.one * scale;
                elapsed += Time.deltaTime;
                yield return null;
            }

            counterText.transform.localScale = Vector3.one;
        }

        public void ShowGameOver()
        {
            if (gameOverText != null)
            {
                Debug.Log("[UICounter] Game Over displayed");
                gameOverText.gameObject.SetActive(true);
            }
        }
    }
}
