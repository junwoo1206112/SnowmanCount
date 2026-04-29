## Context

Current SnowmanCount architecture uses a single leader (PlayerPivot) for all collision detection. Crowd units are decorative: they Lerp to ring-formation positions but have no Colliders, no tags, and no individual behavior. All Gate/Enemy/Obstacle interactions apply math operations to the entire crowd at once via CrowdController.ApplyMathOperation().

Count Masters-style gameplay requires:
- Each follower unit has its own Collider for obstacle/enemy damage
- Gates: leader only triggers the gate (Count Masters: whole crowd passes together)
- Enemies: number comparison (crowdCount > enemyCount = win), NOT 1:1 combat
- Obstacles: individual units that touch are removed
- Leader death = game over (separate from crowd depletion)

## Goals / Non-Goals

**Goals:**
- Every spawned follower has a Collider (isTrigger=true) + Rigidbody for physics trigger
- Followers move independently toward leader using radial offset (even angle distribution)
- Gates: leader-only trigger (entire crowd affected with operatorType + value)
- Enemies: number comparison system (not 1:1). crowdCount > enemyCount → win with loss of enemyCount units. crowdCount <= enemyCount → lose (game over)
- Obstacles: one follower removed per collision
- Leader death (PlayerPivot collision with hazards) = game over, independent of crowd count
- Existing SwerveMovement, WorldMover preserved
- Game flow (Ready → Play → GameOver/LevelClear) preserved

**Non-Goals:**
- No network/multiplayer support
- No UI changes in this phase
- No visual model replacement (still Capsule/SnowGirl based)
- LevelLoader spawning structure unchanged

## Decisions

### Decision 1: Followers get Collider + Kinematic Rigidbody
- **Chosen**: CapsuleCollider (isTrigger=true) + Rigidbody (isKinematic=true, useGravity=false)
- **Rationale**: OnTriggerEnter requires at least one participant to have a Rigidbody. Kinematic Rigidbody doesn't affect existing Lerp movement.
- **Alternative considered**: Collider only, no Rigidbody → OnTriggerEnter doesn't fire between two trigger colliders

### Decision 2: Follower radial offset with even angle distribution
- **Chosen**: Followers distributed around leader with even angles, ring-based radius. Ring 0 = 0 slots (no follower at center, only leader).
- **Rationale**: Clean visual formation that naturally spreads as crowd grows.
- **Alternative considered**: Random distribution → looks chaotic

### Decision 3: Gate triggers on leader only
- **Chosen**: GateController.OnTriggerEnter checks only "Player" tag. Leader passing applies operator+value to entire crowd, then gate disables.
- **Rationale**: Count Masters gates affect the entire crowd as one group. Individual follower gate detection is unnecessary and causes balance issues.
- **Alternative considered**: Per-follower gate detection → redundant, complex, not needed

### Decision 4: Enemy = number comparison (not 1:1 combat)
- **Chosen**: EnemyGroup has a trigger Collider. When Follower/Player enters:
  - If crowdCount > enemyCount: crowdCount -= enemyCount, enemy group destroyed (win)
  - If crowdCount <= enemyCount: crowdCount = 0, game over (lose)
- **Rationale**: Matches Count Masters where enemy encounters are total-count comparisons.
- **Alternative considered**: 1:1 individual combat → too complex, doesn't match Count Masters

### Decision 5: Obstacle damages one follower at a time
- **Chosen**: ObstacleController.OnTriggerEnter removes one follower via RemoveSpecificFollower, then destroys itself.
- **Rationale**: Simple, matches Count Masters obstacle behavior.
- **Alternative considered**: Percentage damage → already existed, removed

### Decision 6: Leader death = game over
- **Chosen**: PlayerPivot's CapsuleCollider (non-trigger, solid) detects collision with obstacles/enemies. On collision, game over immediately regardless of crowd count.
- **Rationale**: Count Masters: leader is the player character; if leader dies, level fails.
- **Implementation**: Add collision detection to PlayerPivot or check leader proximity to hazards.

## Risks / Trade-offs

- **[Performance]** 100+ followers with Collider + Rigidbody → physics overhead. Mitigation: Kinematic Rigidbody is cheap; keep pool size reasonable.
- **[Enemy balance]** Number comparison changes difficulty. Mitigation: adjust enemyCount in Excel data.
- **[Leader death]** Leader collision detection may be hard to wire up cleanly. Mitigation: use PlayerPivot's existing CapsuleCollider with OnCollisionEnter.
- **[Gate balance]** Leader-only gates simplify strategy. Mitigation: gate placement (two lanes) provides enough choice.
