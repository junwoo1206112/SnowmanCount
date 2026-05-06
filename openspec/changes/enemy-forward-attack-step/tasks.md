## 1. Enemy Attack Step

- [x] 1.1 Add serialized attack step timing and distance settings to `EnemyMinion`
- [x] 1.2 Replace immediate follower removal with an enemy-forward attack coroutine
- [x] 1.3 Disable the enemy collider while attack resolution is pending
- [x] 1.4 Centralize enemy cleanup so pooled and non-pooled minions resolve consistently

## 2. Verification

- [x] 2.1 Run a static search to confirm the attack coroutine and cleanup path exist
- [x] 2.2 Check Unity compile errors in the Unity Editor log
