# Proposal: Giant Boss System

## Problem Statement
The current end-game "wave" consists of many small enemies, which feels repetitive. A "Giant Boss" provides a clear goal and a more epic climax to the level.

## Proposed Solution
1.  **New Boss Component:** Create `BossController.cs` to handle HP, size, and collision logic.
2.  **HP-based Combat:** Each collision with a player follower reduces boss HP by 1 and consumes the follower.
3.  **Procedural Spawning:** Update `LevelLoader.cs` to spawn a single Giant Boss instead of a wave at the end of the level.
4.  **Visual Feedback:** The boss will be scaled up and have a visual indicator of remaining HP.

## Success Criteria
- A giant boss appears at the end of Level 1 and Level 2.
- Each follower hitting the boss reduces its HP.
- Defeating the boss triggers the "Level Clear" state.
