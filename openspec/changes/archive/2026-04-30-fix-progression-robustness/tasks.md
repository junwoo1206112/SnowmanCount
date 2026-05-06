# Tasks: Fix Level Progression and Robust Level Loading

## 1. Core Fixes
- [ ] 1.1 Update `GameStateManager.cs` with `DontDestroyOnLoad` and singleton guard.
- [ ] 1.2 Update `GameManager.cs` initialization to avoid resetting `currentLevel` if already set.
- [ ] 1.3 Add null check for `GameStateManager.Instance` in `GameManager.OnSceneLoaded`.

## 2. Data Provider Fixes
- [ ] 2.1 Implement `FindSheetCaseInsensitive` in `NpoiLevelDataProvider.cs`.
- [ ] 2.2 Update `LoadLevel` to use the new fuzzy lookup.

## 3. Verification
- [ ] 3.1 Play Level 1 to completion.
- [ ] 3.2 Verify Level 2 loads and UI displays "Level 2".
- [ ] 3.3 Rename "Level2" in Excel to "level 2" and verify it still loads.
