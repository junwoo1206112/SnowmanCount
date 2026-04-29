## Why

Currently, only the leader (PlayerPivot) has collision detection — all crowd units are decorative Lerp followers. When the leader touches a Gate/Enemy/Obstacle, math operations are applied to the entire crowd at once. This doesn't match Count Masters, where each individual unit has its own collision, moves independently, and can die or trigger gates one-by-one. This refactor makes each snowman a real unit with individual behavior.

## What Changes

- **BREAKING**: Each crowd unit (follower) gets its own Collider (isTrigger), "Follower" tag, and independent movement
- **BREAKING**: Gates detect individual follower collisions instead of leader-only — each follower passing through triggers the operation once
- **BREAKING**: Enemy minions fight 1:1 with followers — each collision destroys one enemy and one follower
- **BREAKING**: Obstacles damage followers one-by-one instead of dealing percentage damage to the whole crowd
- CrowdController refactored: Remove ring-based Lerp formation, followers move freely toward leader
- ObjectPooler adds "Follower" tag on spawn
- GateController listens for both "Player" and "Follower" tags
- EnemyGroup minions get individual Colliders for 1:1 combat

## Capabilities

### New Capabilities
- `follower-collision`: Each follower has its own trigger Collider and "Follower" tag, enabling individual collision detection with gates, enemies, and obstacles
- `follower-individual-movement`: Followers move independently toward leader position without rigid ring formation, allowing organic spreading
- `follower-individual-gate`: Each follower passing through a gate triggers the math operation individually (one unit at a time)
- `follower-enemy-1v1`: Follower collides with enemy minion → both destroyed. 1:1 combat instead of crowd percentage damage.
- `follower-obstacle-damage`: Obstacle damages one follower at a time on contact, rather than dealing damage to the whole crowd.

### Modified Capabilities
- (none — existing specs will be updated inline)

## Impact

- **CrowdController.cs**: Major rewrite — remove ring formation, remove ApplyMathOperation, add per-unit movement and health
- **GateController.cs**: Change OnTriggerEnter to detect both "Player" and "Follower" tags; disable gate after all units pass or after leader passes
- **EnemyGroup.cs**: Add individual Collider to each minion; OnTriggerEnter for "Follower" tag → destroy both
- **ObstacleController.cs**: Change from leader-only detection to follower detection; destroy one follower at a time
- **ObjectPooler.cs**: Add tag assignment on GetPooledObject
- **LevelLoader.cs**: Add Collider to spawned enemy minions
- **Follower prefab**: Must have Collider (isTrigger=true) and tag "Follower"
