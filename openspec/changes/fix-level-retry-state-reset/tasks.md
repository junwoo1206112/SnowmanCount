## 1. Level State Reset

- [x] 1.1 Add a runtime static reset hook for `currentLevel` and `carryOverCrowdCount`
- [x] 1.2 Add all-levels-clear retry behavior that resets to Level 1
- [x] 1.3 Keep normal GameOver retry on the current level
- [x] 1.4 Ignore late LevelClear/GameOver callbacks outside Play state

## 2. UI Normalization

- [x] 2.1 Hide stale GameOver, LevelClear, and Retry UI on scene load
- [x] 2.2 Wire all-levels-clear Retry to the reset-to-Level-1 path

## 3. Verification

- [x] 3.1 Confirm static reset and retry wiring via static search
- [x] 3.2 Check Unity Editor log for compile success or script errors
