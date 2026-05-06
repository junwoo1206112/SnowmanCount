using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

using SnowmanCount.Data;

namespace SnowmanCount.Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public static int currentLevel { get; private set; } = 1;
        public static int carryOverCrowdCount { get; set; } = -1;

        public static void AdvanceLevel()
        {
            currentLevel++;
        }

        public ILevelDataProvider LevelDataProvider { get; private set; }
        public ILevelDataRepository LevelDataRepository { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetRuntimeState()
        {
            currentLevel = 1;
            carryOverCrowdCount = -1;
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            tapRegistered = false;

            RegisterServices();

            if (GameStateManager.Instance == null)
            {
                gameObject.AddComponent<GameStateManager>();
            }

            GameStateManager.Instance.OnStateChanged += HandleStateChanged;
            GameStateManager.Instance.SetState(GameState.Ready);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void RegisterServices()
        {
            NpoiLevelDataProvider provider = new NpoiLevelDataProvider();
            LevelDataProvider = provider;
            LevelDataRepository = provider;
            Debug.Log("[GameManager] Services registered");
        }

        private bool tapRegistered;

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            tapRegistered = false;
            HideEndStateUI();
            GameStateManager.Instance.SetState(GameState.Ready);
            UpdateLevelNumberDisplay();
        }

        private void Update()
        {
            if (GameStateManager.Instance == null) return;

            if (GameStateManager.Instance.CurrentState == GameState.Ready
                && !tapRegistered
                && (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame
                    || Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame))
            {
                tapRegistered = true;
                GameStateManager.Instance.SetState(GameState.Play);
            }
        }

        private void HandleStateChanged(GameState newState)
        {
            Debug.Log($"[GameManager] Game state changed to: {newState}");

            if (newState == GameState.Play)
            {
                HideReadyText();
            }
            else if (newState == GameState.GameOver)
            {
                ShowGameOverUI();
            }
            else if (newState == GameState.LevelClear)
            {
                ShowLevelClearUI(carryOverCrowdCount);
            }
        }

        public void OnLevelCleared(int currentCrowdCount = -1)
        {
            if (GameStateManager.Instance == null || GameStateManager.Instance.CurrentState != GameState.Play)
            {
                Debug.LogWarning("[GameManager] Level clear ignored because the game is not in Play state");
                return;
            }

            Debug.Log("[GameManager] Level cleared!");

            if (currentCrowdCount >= 0)
            {
                carryOverCrowdCount = currentCrowdCount;
            }

            if (GameStateManager.Instance != null)
            {
                GameStateManager.Instance.SetState(GameState.LevelClear);
            }

            GameObject loader = GameObject.Find("LevelLoader");
            if (loader != null)
            {
                loader.SendMessage("OnVictorySequence");
            }
            else
            {
                StartCoroutine(LoadNextLevelDelayed());
            }
        }

        private IEnumerator LoadNextLevelDelayed()
        {
            yield return new WaitForSecondsRealtime(1.5f);

            Time.timeScale = 1f;
            currentLevel++;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void RetryFromClear()
        {
            Time.timeScale = 1f;
            carryOverCrowdCount = -1;
            currentLevel = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void HideReadyText()
        {
            Canvas canvas = FindFirstObjectByType<Canvas>();

            if (canvas != null)
            {
                Transform readyText = canvas.transform.Find("ReadyText");

                if (readyText != null)
                {
                    readyText.gameObject.SetActive(false);
                }
            }
        }

        private void ShowLevelClearUI(int crowdCount)
        {
            Canvas canvas = FindFirstObjectByType<Canvas>();

            if (canvas != null)
            {
                Transform clearText = canvas.transform.Find("LevelClearText");

                if (clearText != null)
                {
                    UnityEngine.UI.Text textComp = clearText.GetComponent<UnityEngine.UI.Text>();
                    if (textComp != null)
                    {
                        textComp.text = $"Snow Power: {crowdCount}";
                    }
                    clearText.gameObject.SetActive(true);
                }
            }

            Time.timeScale = 0f;
        }

        public void OnCrowdDepleted()
        {
            if (GameStateManager.Instance != null && GameStateManager.Instance.CurrentState != GameState.Play)
            {
                Debug.LogWarning("[GameManager] Crowd depleted ignored because the game is not in Play state");
                return;
            }

            Debug.Log("[GameManager] Crowd depleted - Game Over");

            if (GameStateManager.Instance != null)
            {
                GameStateManager.Instance.SetState(GameState.GameOver);
            }
        }

        public void OnLeaderDied()
        {
            Debug.Log("[GameManager] Leader died - Removing one follower");

            if (GameStateManager.Instance != null)
            {
                GameStateManager.Instance.SetState(GameState.GameOver);
            }
        }

        public void OnBossDefeated()
        {
            Debug.Log("[GameManager] Boss defeated - Level cleared!");

            carryOverCrowdCount = -1;

            if (GameStateManager.Instance != null)
            {
                GameStateManager.Instance.SetState(GameState.LevelClear);
            }

            GameObject loader = GameObject.Find("LevelLoader");
            if (loader != null)
            {
                loader.SendMessage("OnVictorySequence");
            }
            else
            {
                StartCoroutine(LoadNextLevelDelayed());
            }
        }

        private void ShowGameOverUI()
        {
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

        public void RetryGame()
        {
            Time.timeScale = 1f;
            carryOverCrowdCount = -1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void RetryAllLevels()
        {
            Time.timeScale = 1f;
            carryOverCrowdCount = -1;
            currentLevel = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void ShowAllLevelsClear()
        {
            Canvas canvas = FindFirstObjectByType<Canvas>();

            if (canvas != null)
            {
                Transform allClearText = canvas.transform.Find("LevelClearText");
                if (allClearText != null)
                {
                    UnityEngine.UI.Text textComp = allClearText.GetComponent<UnityEngine.UI.Text>();
                    if (textComp != null)
                    {
                        textComp.text = "All Levels Clear!";
                    }
                    allClearText.gameObject.SetActive(true);
                }

                Transform retryBtn = canvas.transform.Find("RetryButton");
                if (retryBtn != null)
                {
                    retryBtn.gameObject.SetActive(true);
                    UnityEngine.UI.Button btn = retryBtn.GetComponent<UnityEngine.UI.Button>();
                    if (btn != null)
                    {
                        btn.onClick.RemoveAllListeners();
                        btn.onClick.AddListener(RetryAllLevels);
                    }
                }
            }

            Time.timeScale = 0f;
        }

        private void HideEndStateUI()
        {
            Canvas canvas = FindFirstObjectByType<Canvas>();

            if (canvas == null) return;

            Transform gameOverTxt = canvas.transform.Find("GameOverText");
            if (gameOverTxt != null)
            {
                gameOverTxt.gameObject.SetActive(false);
            }

            Transform clearText = canvas.transform.Find("LevelClearText");
            if (clearText != null)
            {
                clearText.gameObject.SetActive(false);
            }

            Transform retryBtn = canvas.transform.Find("RetryButton");
            if (retryBtn != null)
            {
                retryBtn.gameObject.SetActive(false);
            }
        }

        public void UpdateLevelNumberDisplay()
        {
            Canvas canvas = FindFirstObjectByType<Canvas>();

            if (canvas != null)
            {
                Transform levelText = canvas.transform.Find("LevelText");
                if (levelText != null)
                {
                    UnityEngine.UI.Text textComp = levelText.GetComponent<UnityEngine.UI.Text>();
                    if (textComp != null)
                    {
                        textComp.text = $"Level {currentLevel}";
                    }
                }
            }
        }

        public bool IsReady => LevelDataProvider != null;

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;

            if (GameStateManager.Instance != null)
            {
                GameStateManager.Instance.OnStateChanged -= HandleStateChanged;
            }

            if (Instance == this)
            {
                Instance = null;
            }
        }
    }
}
