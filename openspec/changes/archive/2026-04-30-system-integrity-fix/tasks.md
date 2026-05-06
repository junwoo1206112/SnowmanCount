# Tasks: System Integrity & Stability Fix

## 1. Core & Distance Management
- [ ] 1.1 Add `[RuntimeInitializeOnLoadMethod]` to `GameManager.cs` to reset `currentLevel = 0` and `carryOverCrowdCount = -1`.
- [ ] 1.2 Move `TotalDistanceTraveled` increment logic into `GameManager.Update()` (only if state is `Play` or `BonusRound`).
- [ ] 1.3 Remove all distance calculation logic from `WorldMover.cs`.

## 2. World Flow & Geometry
- [ ] 2.1 Update `WorldMover.Update` to allow movement in `BonusRound`.
- [ ] 2.2 Update `GroundScroller.Update` to allow scrolling in `BonusRound`.
- [ ] 2.3 Update `LevelLoader.SpawnLevel` to extend road generation 150m past the last object.

## 3. Optimization & Physics
- [ ] 3.1 Update `SwerveMovement.cs` to store `CurrentGroundY` via a single raycast.
- [ ] 3.2 Update `CrowdController.cs` to read `CurrentGroundY` from the leader instead of raycasting per follower.
- [ ] 3.3 Ensure Raycasts use a `LayerMask` for "Ground" (Layer 0 for now, or Layer 7 if set).

## 4. Compliance & Cleanup
- [ ] 4.1 Reformat `LevelLoader.cs` and `CrowdController.cs` to strict Allman style.
- [ ] 4.2 Fix `UI.asmdef` and `Core.asmdef` references (remove unnecessary Gameplay refs if possible).

## 5. Verification
- [ ] 5.1 Play Level 1: Verify smooth transition to Level 2.
- [ ] 5.2 Verify progress bar moves during Bonus Round.
- [ ] 5.3 Verify zero unit-climbing bugs.
