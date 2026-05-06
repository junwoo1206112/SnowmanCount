## Context

`GameManager.currentLevel` and `carryOverCrowdCount` are static. In Unity Editor sessions where static state survives, `currentLevel` can remain above the highest sheet in `Levels.xlsx`. `LevelLoader` treats a missing sheet as all levels cleared, which makes the retry UI appear immediately.

The all-levels-clear retry button currently calls `RetryGame()`, but that method only clears carry-over count and reloads the scene. It does not reset `currentLevel`, so retry can loop back into the missing-sheet path.

## Goals / Non-Goals

**Goals:**
- Make a fresh Play session start at Level 1.
- Make all-levels-clear retry start at Level 1.
- Keep GameOver retry on the current level.
- Clear stale GameOver, LevelClear, and Retry UI when returning to Ready.

**Non-Goals:**
- Do not change level data format.
- Do not change level progression timing.
- Do not redesign UI prefabs or scene structure.

## Decisions

- Use `RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)` to reset static state at runtime startup. This is the Unity-safe hook for static reset when domain reload behavior varies.
- Add a dedicated `RetryAllLevels()` method for the all-clear button so GameOver retry and all-clear retry can have different behavior.
- Hide end-state UI in `OnSceneLoaded()` before setting Ready state, so any scene default or prior active state is normalized.
- Gate `OnLevelCleared()` and `OnCrowdDepleted()` to Play state. This prevents late enemy cleanup from changing GameOver into LevelClear, and prevents stale collision callbacks from reopening GameOver UI after a transition.

## Risks / Trade-offs

- Resetting static state on runtime startup means editor Play sessions always begin at Level 1 -> this matches the expected testing behavior and does not affect scene-to-scene progression.
- `FindFirstObjectByType<Canvas>()` remains the existing UI lookup pattern -> this keeps the change scoped to the current scene structure.
- Late callbacks are ignored outside Play -> this makes state transitions deterministic but means debugging logs must be used if a gameplay object fires after transition.
