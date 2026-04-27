using UnityEngine;
using UnityEngine.SceneManagement;
using SnowmanCount.Data;

namespace SnowmanCount.Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public ILevelDataProvider LevelDataProvider { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            RegisterServices();
        }

        private void RegisterServices()
        {
            LevelDataProvider = new NpoiLevelDataProvider();
            Debug.Log("[GameManager] Services registered");
        }

        public void OnCrowdDepleted()
        {
            Debug.Log("[GameManager] Crowd depleted - Game Over");

            Canvas canvas = FindFirstObjectByType<Canvas>();
            if (canvas != null)
            {
                Transform gameOverTxt = canvas.transform.Find("GameOverText");
                if (gameOverTxt != null)
                {
                    gameOverTxt.gameObject.SetActive(true);
                }

                Transform retryBtn = canvas.transform.Find("RetryButton");
                if (retryBtn != null)
                {
                    retryBtn.gameObject.SetActive(true);
                    UnityEngine.UI.Button btn = retryBtn.GetComponent<UnityEngine.UI.Button>();
                    if (btn != null)
                    {
                        btn.onClick.RemoveAllListeners();
                        btn.onClick.AddListener(RetryGame);
                    }
                }
            }

            Time.timeScale = 0f;
        }

        public void OnCrowdCountChanged(int newCount)
        {
            Debug.Log($"[GameManager] Crowd count: {newCount}");
        }

        private void ShowRetryButton()
        {
            UnityEngine.UI.Button[] allButtons = FindObjectsByType<UnityEngine.UI.Button>(FindObjectsSortMode.None);
            foreach (UnityEngine.UI.Button btn in allButtons)
            {
                if (btn.gameObject.name == "RetryButton")
                {
                    btn.gameObject.SetActive(true);
                    btn.onClick.RemoveAllListeners();
                    btn.onClick.AddListener(RetryGame);
                }
            }
        }

        public void RetryGame()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public bool IsReady => LevelDataProvider != null;

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }
    }
}
