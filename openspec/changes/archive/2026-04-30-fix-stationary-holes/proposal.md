# Proposal: Fix Stationary Player and Deadly Holes

## Problem Statement
1.  **Movement Error:** The player and followers are moving forward on the Z-axis, but the project uses a "World Scroller" where objects move towards a stationary player.
2.  **Vertical Jank:** Units move up and down unexpectedly (Raycast logic), which was intended for stairs but feels "strange" and causes them to "respawn" above holes instead of falling.
3.  **Hole Bug:** Falling into a hole doesn't trigger Game Over for the leader; units just appear back on the road.

## Proposed Changes

### 1. Stationary Player (Core/Gameplay)
- **SwerveMovement.cs:** Remove `pos.z += forwardSpeed * Time.deltaTime;`. The player should stay at Z=0.
- Ensure `forwardSpeed` is only used for UI/Logic if needed, but not for physical movement.

### 2. Deadly Holes (Gameplay)
- **HoleController.cs:** Add detection for the "Player" tag. If the leader enters the trigger, call `GameManager.Instance.OnLeaderDied()`.
- **SwerveMovement.cs:** Add a `bool isFalling` flag. If falling, disable the height-correction logic so the player actually falls.

### 3. Controlled Verticality (Gameplay)
- **SwerveMovement.cs & CrowdController.cs:** 
    - Only perform the Raycast height adjustment if `GameState == BonusRound`.
    - During normal `Play` state, keep the height fixed at `Y = 0.5f`.
    - This prevents the "climbing on self" or "teleporting out of holes" bugs.

## Success Criteria
- Player stays at Z=0 while world moves.
- Falling into a hole results in Game Over for the leader.
- No vertical movement happens during normal play.
- Bonus stairs still work during the Bonus Round.
