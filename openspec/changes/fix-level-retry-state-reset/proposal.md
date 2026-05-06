## Why

Repeated play sessions and all-levels-clear retry can leave `GameManager.currentLevel` pointing past the available level sheets. When that happens, `LevelLoader` immediately shows the all-clear retry UI instead of starting from Level 1.

## What Changes

- Reset static level progression state when a new runtime session starts.
- Reset `currentLevel` when retrying from the all-levels-clear screen.
- Hide stale end-state UI when a scene is loaded back into Ready state.
- Ignore late level-clear or game-over signals after the game has already left Play state.
- Add clearer logs around missing level data.

## Capabilities

### New Capabilities
- `level-retry-state-reset`: Level progression state resets cleanly for new sessions and all-clear retry.

### Modified Capabilities

## Impact

- Affects `Assets/Scripts/Core/GameManager.cs`.
- Affects `Assets/Scripts/Gameplay/LevelLoader.cs` logging only.
- No data, prefab, dependency, or assembly definition changes required.
