## 1. Follower Collider + Rigidbody + Tag

- [x] 1.1 CapsuleCollider(isTrigger=true) + Rigidbody(isKinematic=true) via ObjectPooler
- [x] 1.2 FollowerComponent.cs created for component-based detection
- [x] 1.3 GetPooledObject assigns Collider, Rigidbody, FollowerComponent
- [x] 1.4 ReturnToPool preserves all components

## 2. Follower Radial Movement (CrowdController)

- [x] 2.1 Even angle distribution via FollowerComponent.targetAngle
- [x] 2.2 Ring-based radius (Ring 0 = 0 slots — leader only)
- [x] 2.3 RedistributeAngles() on crowd change
- [x] 2.4 Lerp movement with immediate teleport if far (>1f)
- [x] 2.5 Removed ring formation dead code
- [x] 2.6 Retained events + public AddCrowd/NotifyCountChanged

## 3. Gate Controller (Leader-Only, Count Masters Style)

- [x] 3.1 OnTriggerEnter detects "Player" tag only
- [x] 3.2 ApplyMathOperation(operatorType, value) for entire crowd
- [x] 3.3 Gate disables after leader triggers
- [x] 3.4 No per-follower gate detection

## 4. Enemy System (Number Comparison, Count Masters Style)

- [x] 4.1 EnemyGroup uses parent Collider(isTrigger=true) for detection
- [x] 4.2 OnTriggerEnter: crowdCount > enemyCount → win (crowd -= enemyCount)
- [x] 4.3 OnTriggerEnter: crowdCount <= enemyCount → lose (crowd = 0, game over)
- [x] 4.4 Minion children are visual-only (no Collider, no script)
- [x] 4.5 EnemyAllClearDetector notified on group destruction

## 5. Obstacle Controller

- [x] 5.1 OnTriggerEnter detects FollowerComponent
- [x] 5.2 RemoveSpecificFollower + NotifyCountChanged + destroy
- [x] 5.3 "Player" fallback: RemoveSpecificFollower + destroy
- [x] 5.4 Percentage damage removed

## 6. LevelLoader

- [x] 6.1 EnemyGroup parent Collider preserved (isTrigger=true)
- [x] 6.2 Minion visuals have no Collider (parent handles detection)
- [x] 6.3 No EnemyMinion script on minions

## 7. Leader Death (Separate from Crowd Depletion)

- [x] 7.1 SwerveMovement.OnCollisionEnter/OnTriggerEnter detects hazards
- [x] 7.2 GameManager.OnLeaderDied() sets GameOver state
- [x] 7.3 Leader death independent of crowd count

## 8. Verification

- [ ] 8.1 Play test: leader passes gate → operator+value applied to whole crowd
- [ ] 8.2 Play test: crowd touches EnemyGroup → count comparison (win/lose)
- [ ] 8.3 Play test: follower touches obstacle → 1 follower removed
- [ ] 8.4 Play test: crowd=0 → GameOver (not leader death)
- [ ] 8.5 Play test: leader collides with hazard → GameOver (regardless of crowd)
- [ ] 8.6 Play test: enemy wave cleared → all enemies removed, level continues
- [ ] 8.7 Verify no MissingReferenceException or NullReferenceException
