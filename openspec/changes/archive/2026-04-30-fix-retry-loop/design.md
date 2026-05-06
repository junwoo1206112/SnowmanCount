# Design: Fix Premature Retry Loop

## Key Changes
- **GameManager.cs**:
    - Update `ShowAllLevelsClear` button listener.
    - Implement `HideEndGameUI()` to hide `GameOverText`, `LevelClearText`, and `RetryButton`.
    - Call `HideEndGameUI()` in `OnSceneLoaded()` and `HideReadyText()`.

## Verification Plan
- Clear Level 2.
- Confirm Level 3 (missing) failure triggers "All Levels Clear".
- Confirm Retry button starts Level 1.
- Confirm HUD is clean on Start.
