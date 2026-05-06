## 1. Enemy 1v1 Combat

- [x] 1.1 Change `EnemyGroup` from parent-trigger count comparison to remaining-minion tracking
- [x] 1.2 Make `EnemyMinion` notify its parent when it disappears
- [x] 1.3 Ensure each enemy minion removes exactly one follower or one crowd count on collision

## 2. Midrun Enemy Spawning

- [x] 2.1 Keep enemy minion colliders enabled
- [x] 2.2 Keep/add `EnemyMinion` on each spawned enemy minion
- [x] 2.3 Spread enemy minions across the road in a readable pack
- [x] 2.4 Use `SnowmanSmall` as the enemy minion prefab in `GameScene`

## 3. Verification

- [x] 3.1 Static search confirms minion scripts are not removed by `LevelLoader`
- [x] 3.2 Static search confirms `EnemyGroup.OnMinionDefeated()` is called
- [x] 3.3 Unity log check shows no C# compile errors
