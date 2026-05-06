# Current Changes - 2026-05-06 (Updated)

## Summary

This note records the current gameplay fixes implemented in this workspace.

## 1. Count Masters Style One-To-One Combat (Clash System)

OpenSpec change:
- `openspec/changes/count-masters-one-to-one-contact`
- `openspec/changes/enemy-forward-attack-step` (superseded by clash system)

Files changed:
- `Assets/Scripts/Gameplay/CrowdController.cs`
- `Assets/Scripts/Gameplay/FollowerComponent.cs`
- `Assets/Scripts/Gameplay/EnemyMinion.cs`
- `Assets/Scripts/Gameplay/LevelLoader.cs`

Problems found:
- A simple trigger or sphere-radius contact made combat feel like instant deletion.
- Enemies could remove a nearby arbitrary unit instead of the unit that visually looked like it was fighting.
- Count Masters combat reads more like enemy units and player units compress into the middle and disappear one-to-one.

What changed in `CrowdController`:
- Added `FindClosestFollowerForDuel()` — finds nearest non-dueling follower within X/Z range.
- Added `TryReserveClosestFollowerForDuel()` — reserves the found follower by setting `isDueling`.
- Added `TryRemoveClosestFollowerForDuel()` — reserve-and-remove in one call.
- Dueling followers are skipped by normal formation movement (`Update()` skips `isDueling` followers).

What changed in `FollowerComponent`:
- Added `isDueling` field (reset to `false` on `OnEnable()`).

What changed in `EnemyMinion`:
- Replaced old `AttackAndRespawnRoutine`/`RespawnRoutine` with a clash state.
- Added `ClashRoutine()` coroutine — enemy and follower move toward a shared midpoint.
- The midpoint X is pulled toward the player group's center line with `clashCenterPull`.
- Added `FaceEachOther()` — both units rotate to face each other during clash movement.
- Units are removed only after they reach `clashResolveDistance`.
- Enemy is destroyed (not respawned) after clash; `parentGroup.OnMinionDefeated()` is called.

Current combat tuning:
- `duelXRange = 1.1f`
- `duelZRange = 3.0f`
- `clashMoveSpeed = 8f`
- `clashResolveDistance = 0.22f`
- `clashCenterPull = 0.55f`

What changed in `LevelLoader`:
- Enemy groups (both Excel enemies and waves) use `SpawnPolarFormation()` — polar coordinate circular placement.
- Formula: `(360 / count) * i` degrees per unit, at a radius based on road width.
- Enemies are evenly distributed around the group center in a circle/spiral.

Expected behavior:
- Enemies and player units start combat when they touch.
- Each enemy reserves one player follower for a one-to-one duel.
- Both units move toward a central clash point, face each other, and disappear together.
- Enemy formation appears as a polar (circular) cluster rather than a line or block.

## 2. Level Retry State Reset Fix

OpenSpec change:
- `openspec/changes/fix-level-retry-state-reset`

Files changed:
- `Assets/Scripts/Core/GameManager.cs`
- `Assets/Scripts/Gameplay/LevelLoader.cs`

Problem found:
- The Unity Editor log showed `Sheet 'Level3' not found`.
- `GameManager.currentLevel` is static, so it can remain above the available level sheets between repeated Play sessions or retry flows.
- `ShowAllLevelsClear()` wired Retry to `RetryGame()`, but `RetryGame()` did not reset `currentLevel` to 1.
- Late combat callbacks could also let GameOver be overwritten by LevelClear after the crowd was already depleted.

What changed:
- Added `ResetRuntimeState()` with `RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)`.
- Runtime startup now resets:
  - `currentLevel = 1`
  - `carryOverCrowdCount = -1`
- Added `RetryAllLevels()` for the all-levels-clear retry path.
- `ShowAllLevelsClear()` now wires Retry to `RetryAllLevels()`.
- `OnSceneLoaded()` calls `HideEndStateUI()` before returning to Ready.
- `OnLevelCleared()` now ignores calls when the current state is not `GameState.Play`.
- `OnCrowdDepleted()` now ignores calls when the current state is not `GameState.Play`.
- `LevelLoader` missing-data logging now includes the requested level number.

Expected behavior:
- Fresh Play sessions start from Level 1.
- All-levels-clear Retry restarts from Level 1.
- Normal GameOver Retry remains on the current level.
- Stale GameOver, LevelClear, and Retry UI are hidden when the scene reloads.
- A late enemy-clear event cannot overwrite GameOver after the crowd has already depleted.

## 3. Player Crowd Formation And Ground Alignment

Files changed:
- `Assets/Scripts/Gameplay/CrowdController.cs`

Problems found:
- Followers were intended to spawn around the player by polar coordinates, but their Z target was being flattened, which made units appear in a horizontal line.
- The player/pivot was on the road, but follower units appeared to float because follower target Y and collider setup did not match the road surface.

What changed:
- Follower target positions now keep the polar X/Z offset around the player pivot.
- New followers snap to their computed target positions when spawned.
- `GetPolarOffset` Y is `0f` for normal road movement.
- Follower capsule collider center is normalized to `(0, 0.5, 0)`.

Expected behavior:
- Player units form a circular/polar crowd around the player pivot instead of a straight horizontal row.
- Followers stay attached to the road height instead of floating above it.

## 4. Midrun Enemies From Excel Data

OpenSpec changes:
- `openspec/changes/midrun-one-to-one-enemies`
- `openspec/changes/excel-level-data-repository`

Files changed:
- `Assets/Scripts/Gameplay/LevelLoader.cs`
- `Assets/Scripts/Gameplay/EnemyGroup.cs`
- `Assets/Scripts/Gameplay/EnemyMinion.cs`
- `Assets/Scripts/Core/GameManager.cs`
- `Assets/Scripts/Data/ILevelDataRepository.cs`
- `Assets/Scripts/Data/NpoiLevelDataProvider.cs`
- `Assets/StreamingAssets/Levels/Levels.xlsx`

What changed:
- Added support for spawning enemy groups from Excel rows where `ObjectType` is `Enemy`.
- Enemy rows were added to `Levels.xlsx`.
- `SnowmanSmall` prefab is connected as the enemy unit prefab (via `enemyPrefab` field in LevelLoader).
- `EnemyGroup` tracks remaining enemy minions and registers waves.
- `EnemyMinion` handles one-to-one combat via clash system.

Excel data now includes enemy entries:
- Level 1: enemy groups at distances 170, 310, and 470.
- Level 2: enemy groups at distances 120, 220, 340, and 460.

Expected behavior:
- Enemies appear midrun from Excel-driven level data, placed in polar coordinate formation.
- The designer can keep using the NPOI Excel workflow instead of hardcoding enemy placement.

## 5. DI And NPOI Level Repository Expansion

OpenSpec change:
- `openspec/changes/excel-level-data-repository`

Files changed:
- `Assets/Scripts/Core/GameManager.cs`
- `Assets/Scripts/Data/ILevelDataRepository.cs`
- `Assets/Scripts/Data/NpoiLevelDataProvider.cs`

Problem discussed:
- Since the project uses DI through `GameManager`, read/write/update level data services should be exposed through interfaces.
- NPOI was added so Excel data can be extended and saved, not only read.

What changed:
- Added `ILevelDataRepository` extending `ILevelDataProvider` with write operations: `SaveLevel`, `AddRow`, `UpdateRow`, `DeleteRow`.
- `NpoiLevelDataProvider` now implements both `ILevelDataProvider` and `ILevelDataRepository`.
- `GameManager` now exposes both:
  - `LevelDataProvider` (read-only)
  - `LevelDataRepository` (read-write)
- `GameManager.RegisterServices()` creates one `NpoiLevelDataProvider` instance and registers it through both interfaces.
- `WriteWorkbook()` helper handles file locking, workbook open/create/write/close lifecycle.

Expected behavior:
- Gameplay code can keep reading through `ILevelDataProvider`.
- Editor/tools or future data workflows can use `ILevelDataRepository` for save/update operations.

## 6. SnowmanSmall Enemy Prefab And Visual Color

Files changed:
- `Assets/Scenes/GameScene.unity`
- `Assets/Scripts/Gameplay/LevelLoader.cs`
- `Assets/Scripts/Gameplay/EnemyMinion.cs`

Problem found:
- The enemy unit color looked different from the original prefab because earlier enemy code applied a runtime material tint.

What changed:
- `SnowmanSmall` is used as the enemy minion prefab (via `enemyPrefab` serialized field).
- Runtime enemy color tinting was removed — the prefab keeps its original look.
- `LevelLoader.SpawnEnemyMinion()` creates minions from `enemyPrefab` with a PrimitiveSphere fallback.
- Enemy minions use the Default layer, not the Unit layer, so they interact with all collision logic.

Expected behavior:
- Enemy units keep the original `SnowmanSmall` prefab appearance.
- Enemy and player units can interact through combat logic without layer mismatch.

## 7. Count Masters Combat Redesign (2026-05-06)

### 7.1 Y Position Unification

Files changed:
- `Assets/Scripts/Gameplay/SwerveMovement.cs` — removed Y=0.5 force from Awake() and ApplySwerveOnly()
- `Assets/Scenes/GameScene.unity` — SnowGirl_2 local Y -0.5 → 0, ObjectPooler → PlayerPivot child
- `Assets/Scripts/Gameplay/CrowdController.cs` — removed followerYOffset, GetPolarOffset Y=0, removed TryReserveFollowerByAngle
- `Assets/Scripts/Gameplay/LevelLoader.cs` — enemy Y 0.5 → 0

Problems:
- PlayerPivot was at Y=0.5 (forced by SwerveMovement) while followers were at Y=0 (via GetPolarOffset -0.5)
- SnowGirl_2 had local Y=-0.5 to compensate → world Y=0
- ObjectPooler was at Y=-0.5 as a separate root object
- Three separate Y offsets created confusing coordinate system

Fix:
- PlayerPivot: scene Y=0, no runtime override
- SnowGirl_2: local Y 0 (no offset)
- Followers: target Y = playerPivot.y + 0 = 0
- ObjectPooler: Y=0, child of PlayerPivot
- All units now share Y=0: one unified area on the road surface

### 7.2 Enemy Multi-Ring Formation

Files changed:
- `Assets/Scripts/Gameplay/LevelLoader.cs`

Old: `SpawnPolarFormation()` — single ring, all enemies at same radius
New: `SpawnMultiRingFormation()` — multiple rings like CrowdController

Ring structure:
- Ring 0: 1 unit (center)
- Ring 1: 2π×1.5/1.5 ≈ 6 units
- Ring 2: 2π×3.0/1.5 ≈ 12 units
- Ring N: circumference/unitSpacing(1.5) slots

Enemy Y: 0.5 → 0 (stand on road surface)

### 7.3 Enemy Combat (Swarm + Individual Targeting + Separation)

Files changed:
- `Assets/Scripts/Gameplay/EnemyMinion.cs` — complete rewrite
- `Assets/Scripts/Gameplay/CrowdController.cs` — added FindNearestAvailableFollower(), GetFollowerAtIndex()

Flow:
1. Enemy group approaches with WorldMover
2. Distance < 30m → aggro: `SetParent(null)`, start individual movement
3. Each enemy finds nearest available follower via `FindNearestAvailableFollower()`
4. Moves toward that follower (not center)
5. Separation: `Physics.OverlapSphere(separationRadius=2m)` pushes enemies apart (strength=5)
6. Velocity clamped to max 2x swarmSpeed
7. OnTriggerEnter (radius 1.5m) → instant clash: isDueling → RemoveSpecificFollower → Destroy
8. No coroutine, no pause, no dash, no angle matching

### 7.4 Boss Simplification

Files changed:
- `Assets/Scripts/Gameplay/BossController.cs` — removed all minion code
- `Assets/Scripts/Gameplay/BossMinionController.cs` — deleted (unused)
- `Assets/Scripts/Gameplay/LevelLoader.cs` — added bossPrefab field, SpawnBoss with collider fallback
- `Assets/Scenes/GameScene.unity` — bossPrefab serialized to SnowmanSmall

Changes:
- BossMinionController removed (Count Masters has no boss minions)
- Boss prefab: SnowmanSmall (2x scale) instead of Capsule primitve
- Collider: SphereCollider radius 1.5 (added if prefab has none)
- Combat: OnTriggerEnter → follower removed, boss takes 1 damage
- Phase tracking kept (visual only, no minion spawning)

### 7.5 Victory Staircase (Pyramid)

Files changed:
- `Assets/Scripts/Core/GameManager.cs` — added AdvanceLevel(), SendMessage to LevelLoader
- `Assets/Scripts/Gameplay/LevelLoader.cs` — added OnVictorySequence(), VictoryStaircaseRoutine(), ClimbToPosition()
- `Assets/Scripts/Gameplay/CrowdController.cs` — added GetFollowerAtIndex()

Flow:
1. All enemies/boss defeated → GameManager sets LevelClear state
2. SendMessage("OnVictorySequence") to LevelLoader
3. WorldMover.SetSpeed(0) — freeze all movement
4. Build staircase: 8 steps (bottom→top), stepHeight=0.6, stepDepth=1.2, baseWidth=3 + i*0.5
5. Pattern 8877665544332211: each step holds 2 followers (left+right)
6. Followers climb one by one (0.06s interval), arc trajectory (peak → land)
7. Show Snow Power count → 1.5s → next level

### 7.6 Excel Data Update

- Boss rows added: Level1(560/Boss/30/6), Level2(550/Boss/50/8)
- Hole rows: Value changed from empty to "left"
- Wave enemy spawn position: now before boss (bossDistance - 30f) instead of after (maxDistance - 20f)

### 7.7 Cleanup

Deleted files:
- `Assets/Scripts/Gameplay/BossMinionController.cs` (unused after boss simplification)
- `Assets/Scripts/Gameplay/EnemyFormationController.cs` (dead code)
- `RebuildXlsx.cs`, `CreateFinalXlsx.cs`, `temp_xlsx.js` (temp scripts)

Removed code:
- `CrowdController.TryReserveFollowerByAngle()` (unused)
- `CrowdController.followerYOffset` field (unused)
- `EnemyMinion` Update-based angle matching (replaced by simple OnTriggerEnter)
- `EnemyMinion` FaceEachOther/ClashRoutine coroutine (instant clash)
- `GameManager` staircase code → moved to `LevelLoader`

---

## Verification Performed

- Static search confirmed all method names exist in current source:
  - `ClashRoutine`, `FaceEachOther`, `DefeatEnemy` (EnemyMinion)
  - `FindClosestFollowerForDuel`, `TryReserveClosestFollowerForDuel`, `TryRemoveClosestFollowerForDuel` (CrowdController)
  - `isDueling` (FollowerComponent)
  - `SpawnPolarFormation` (LevelLoader)
- All OpenSpec changes are marked complete: `enemy-forward-attack-step`, `fix-level-retry-state-reset`, `count-masters-one-to-one-contact`, `excel-level-data-repository`, `midrun-one-to-one-enemies`.

## Verification Limitation

- The installed `unity-cli` executable in this environment is not the documented Unity Editor control CLI, so direct commands like `unity-cli editor play --wait` and `unity-cli console --type error` could not be used reliably.
- Verification was done through static search and code review instead.
