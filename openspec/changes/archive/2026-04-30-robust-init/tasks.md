# Tasks: Robust Level Start & UI Management

## 1. GameManager Resilience
- [ ] 1.1 Reset `currentLevel = 1` and `carryOverCrowdCount = -1` in `GameManager.Awake()`.
- [ ] 1.2 Implement `FindDeepChild()` recursive search in `GameManager.cs`.
- [ ] 1.3 Update `HideEndGameUI()`, `ShowLevelClearUI()`, `ShowGameOverUI()`, `ShowAllLevelsClear()`, and `UpdateLevelNumberDisplay()` to use `FindDeepChild()`.

## 2. Gameplay Safety
- [ ] 2.1 Add a check in `LevelLoader.SpawnObject()` to prevent hazards from spawning within 5m of the player.

## 3. Verification
- [ ] 3.1 Verify clean start in Unity Editor across multiple play/stop sessions.
- [ ] 3.2 Move UI elements into a "Panel" in Unity Editor and verify they still work.
- [ ] 3.3 Test with a dummy Excel row having an enemy at Z=0.
