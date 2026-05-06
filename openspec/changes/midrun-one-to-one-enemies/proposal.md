# Midrun One-To-One Enemies

## Why
The current enemy implementation is drifting between two designs: parent `EnemyGroup` count comparison and per-enemy minion collision. To match Count Masters-style midrun combat, enemies should appear during the run and each enemy should remove exactly one player unit when they collide.

## What Changes
- Enemy groups spawn visible enemy minions across the road during the level.
- Each enemy minion has its own trigger collider and `EnemyMinion` component.
- On collision, one player follower and one enemy minion disappear.
- EnemyGroup tracks remaining minions and unregisters from level-clear detection only when all minions are gone.

## Impact
- Updates `LevelLoader`, `EnemyGroup`, and `EnemyMinion`.
- Keeps boss and obstacle behavior separate.
- Aligns active OpenSpec `individual-unit-refactor` intent with actual 1v1 enemy combat behavior.
