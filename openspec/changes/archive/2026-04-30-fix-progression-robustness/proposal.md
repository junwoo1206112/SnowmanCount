# Proposal: Fix Level Progression and Robust Level Loading

## Problem Statement
The game fails to progress from Level 1 to Level 2. After clearing Level 1, it either stays stuck with the Level 1 UI or shows the "All Levels Clear" screen, which leads back to Level 1.

## Identified Root Causes
1.  **Dead Reference in GameStateManager:** `GameStateManager` is not explicitly marked as `DontDestroyOnLoad`. When scenes reload, `GameStateManager.Instance` can become a dead reference, causing a crash in `GameManager.OnSceneLoaded` and halting the initialization flow.
2.  **Case-Sensitive Sheet Lookup:** `NpoiLevelDataProvider` uses `workbook.GetSheet("Level2")`, which is case-sensitive. If the Excel sheet is named differently (e.g., "level2" or "Level 2"), it fails, triggering the "All Levels Clear" UI.
3.  **Progression Race Condition:** Static variables like `currentLevel` are being reset or handled inconsistently across scene reloads.

## Proposed Changes

### 1. Robust Singleton Persistence (Core)
- **GameStateManager.cs:** Implement standard persistent singleton pattern with `DontDestroyOnLoad` in `Awake`.
- **GameManager.cs:** Ensure `GameStateManager` is correctly referenced and initialized.

### 2. Case-Insensitive Sheet Loading (Data)
- **NpoiLevelDataProvider.cs:** Instead of `GetSheet()`, iterate through all sheets in the workbook and compare names case-insensitively (using `.ToLower().Replace(" ", "")`) to ensure "Level 2" or "level2" still loads.

### 3. Safer Progression (Core)
- **GameManager.cs:** Add safety checks in `OnSceneLoaded` to ensure all necessary instances are valid before proceeding.
- Ensure `currentLevel` increment happens reliably before the reload.

## Success Criteria
- Level 1 clears and correctly loads Level 2.
- UI updates to show "Level 2".
- Excel sheet name variations (case, spaces) no longer break level loading.
- No `MissingReferenceException` in console on scene reload.
