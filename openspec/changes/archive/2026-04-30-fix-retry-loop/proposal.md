# Proposal: Fix Premature Retry Loop at Start

## Problem Statement
The player gets stuck in an infinite loop of "Retry" buttons when clearing the final level, or sees the button prematurely at the start.

## Solution
1. Bind the Retry button in `ShowAllLevelsClear` to `RetryFromClear()` so it resets `currentLevel` to 1.
2. Add a `HideEndGameUI()` method to ensure all end-game UI elements are disabled when a scene loads or restarts.

## Success Criteria
- Resetting from "All Levels Clear" correctly starts Level 1.
- No "Retry" UI is visible at the beginning of any level.
