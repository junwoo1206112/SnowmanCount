# Tasks: Fix Premature Retry Loop

## 1. GameManager Fixes
- [ ] 1.1 In `GameManager.ShowAllLevelsClear()`, change `btn.onClick.AddListener(RetryGame)` to `btn.onClick.AddListener(RetryFromClear)`.
- [ ] 1.2 Create `HideEndGameUI()` method in `GameManager` to disable "GameOverText", "LevelClearText", and "RetryButton".
- [ ] 1.3 Call `HideEndGameUI()` inside `GameManager.OnSceneLoaded()`.

## 2. Verification
- [ ] 2.1 Play test Level 1: Ensure no Retry button on start.
- [ ] 2.2 Verify full clearing resets to Level 1.
