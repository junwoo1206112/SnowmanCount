# Count Masters One-To-One Contact

## Why
Enemy minions should behave closer to Count Masters combat: when enemy and player units meet, each enemy removes exactly one player unit and disappears. Relying only on trigger callbacks can miss fast-moving or tightly packed units.

## What Changes
- Add lane-based one-to-one contact resolution to `EnemyMinion`.
- Match each enemy minion against the closest follower in its forward lane before falling back to trigger contact.
- Move matched enemy/follower pairs toward a shared clash point before removing them.
- Spawn enemies in shallow duel lines instead of compact square blocks.
- Keep trigger callbacks as a fallback.

## Impact
- More reliable midrun unit-vs-enemy combat.
- Combat reads closer to Count Masters: front units meet enemy units one by one in visible lines.
- No level data changes required.
