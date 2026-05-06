# Proposal: Robust Level Start & UI Management

## Problem Statement
The "Retry" button appears prematurely at game start due to:
1.  **Persistent Statics:** `GameManager.currentLevel` remains at its last value in the Unity Editor across play sessions.
2.  **Fragile UI Paths:** `transform.Find()` fails if UI elements are moved inside panels.
3.  **Execution Order:** `LevelLoader` can trigger "All Clear" before `GameManager` has finished hiding UI.

## Proposed Changes

### 1. Reset Statics on App Start (Core)
- **GameManager.cs:** Reset `currentLevel` to 1 in `Awake()` (or a dedicated `Init` method) to ensure every new play session starts from the beginning.
- Change `currentLevel` to be a normal property if global persistence isn't required, or keep it static but explicitly reset it.

### 2. Robust UI Helper (Core)
- **GameManager.cs:** Add a private helper method `FindDeepChild(Transform parent, string name)` to recursively find UI elements even if they are nested inside panels.
- Update `HideEndGameUI`, `ShowGameOverUI`, etc., to use this deep search.

### 3. Hazard Spawn Guard (Gameplay)
- **LevelLoader.cs:** Add a check in `SpawnObject()` to ensure no "enemy" or "obstacle" is spawned at a distance (Z) less than 5m from the player's start position (Z=0).

## Success Criteria
- Starting the game in the Unity Editor ALWAYS shows the "Ready" screen.
- UI elements are hidden/shown correctly regardless of their depth in the Canvas hierarchy.
- No instant Game Over from hazards at the origin.
