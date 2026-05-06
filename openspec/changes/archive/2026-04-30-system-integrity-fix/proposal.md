# Proposal: System Integrity & Stability Fix

## Problem Statement
The game feels "strange" and inconsistent due to:
1.  **Frozen Finale:** World movement (`WorldMover`, `GroundScroller`) stops in `BonusRound`, freezing the game when it should be finishing.
2.  **Progress Tracking Failure:** Distance tracking stops because the "Master" object is destroyed as it moves past the camera.
3.  **Geometry Gaps:** The road ends before the bonus stairs and castle, leaving them floating in the air.
4.  **Inefficient Physics:** Per-follower raycasts are heavy and risk feedback loops (units climbing each other).
5.  **State Leakage:** Static variables (Level Number) don't reset correctly between Editor sessions.

## Proposed Changes

### 1. Robust Lifecycle & State (Core)
- **GameManager.cs:** 
    - Implement `[RuntimeInitializeOnLoadMethod]` to reset all static variables (`currentLevel`, `carryOverCrowdCount`) to ensure a fresh start in the Editor.
    - Centralize `totalDistanceTraveled` as a non-static, managed property.
- **GameStateManager.cs:** Ensure `BonusRound` transition is robust.

### 2. Unified World Movement (Gameplay)
- **WorldMover.cs & GroundScroller.cs:** 
    - Update `canMove` / `canUpdate` logic to allow movement in **both** `Play` and `BonusRound` states.
- **WorldMover.cs:**
    - Remove the transient "Master" logic. 
    - Create a `DistanceTracker.cs` (or handle in `LevelLoader`) that increments a global distance based on time and speed, independent of object destruction.

### 3. Extended Geometry (Gameplay)
- **LevelLoader.cs:** 
    - Recalculate `roadLimitZ` to be `maxDistance + bonusOffset + 150m` to ensure a continuous floor under the stairs and castle.

### 4. Optimized Physics & Height (Gameplay)
- **Layer Setup:** Add a "Ground" layer (Layer 7).
- **SwerveMovement.cs:** Perform a single raycast to determine the "Ground Height" and store it in a public property.
- **CrowdController.cs:** Followers read the shared "Ground Height" from the leader instead of performing individual raycasts.

## Success Criteria
- The game flows seamlessly from start to the final castle without freezing.
- The progress bar reflects the full level length including the bonus round.
- No objects appear floating in the void.
- Zero "Follower climbed other Follower" bugs.
- Editor play sessions always start from Level 1.
