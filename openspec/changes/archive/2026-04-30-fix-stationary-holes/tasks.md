# Tasks: Fix Stationary Player and Deadly Holes

## 1. Hole Logic (HoleController.cs)
- [ ] 1.1 Detect `Player` tag in `OnTriggerEnter`.
- [ ] 1.2 Implement `FellInHoleLeaderRoutine` for the player.
- [ ] 1.3 Call `GameManager.Instance.OnLeaderDied()` after fall duration.

## 2. Player Movement (SwerveMovement.cs)
- [ ] 2.1 Remove `pos.z += forwardSpeed * Time.deltaTime;`.
- [ ] 2.2 Add `bool isFalling` flag.
- [ ] 2.3 Restrict Raycast height adjustment to `BonusRound`.
- [ ] 2.4 If `isFalling`, stop all swerve/height logic and move downwards.

## 3. Crowd Logic (CrowdController.cs)
- [ ] 3.1 Restrict follower Raycast height adjustment to `BonusRound`.

## 4. Verification
- [ ] 4.1 Verify player Z stays at 0.
- [ ] 4.2 Verify falling into a hole triggers Game Over.
- [ ] 4.3 Verify no vertical jank during normal play.
