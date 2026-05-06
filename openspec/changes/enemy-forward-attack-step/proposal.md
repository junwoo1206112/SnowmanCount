## Why

Enemy collisions currently remove units immediately, which makes combat feel abrupt. Count Masters-style combat reads better when the enemy minion visibly steps toward the player unit before both units disappear.

## What Changes

- Add a short enemy-forward attack step before resolving follower collisions.
- Disable the enemy collider during the attack step to prevent duplicate contact resolution.
- Remove the matched follower and enemy only after the step animation completes.
- Keep leader contact and fallback cleanup behavior deterministic.

## Capabilities

### New Capabilities
- `enemy-forward-attack-step`: Enemy minions perform a short visible step toward the contacted player unit before one-to-one removal.

### Modified Capabilities

## Impact

- Affects `Assets/Scripts/Gameplay/EnemyMinion.cs`.
- No level data, prefab schema, external dependency, or assembly definition changes required.
