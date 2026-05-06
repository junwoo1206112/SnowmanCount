# Design

## EnemyGroup Responsibility
`EnemyGroup` acts as a wave container and remaining-enemy counter. It does not resolve the whole battle on a single parent trigger. Instead, child `EnemyMinion` objects report when they are defeated.

## EnemyMinion Responsibility
Each enemy minion owns one trigger collision. When it collides with a `FollowerComponent`, it removes that follower and returns/destroys itself. When it collides with the leader, it removes one crowd member through `CrowdController.RemoveCrowd(1)`.

## LevelLoader Responsibility
When spawning an enemy row, `LevelLoader` creates a parent `EnemyGroup` and child minions. Minions are distributed in a small local formation so they appear as a midrun enemy pack. Their colliders and scripts remain active.

## Level Clear
`EnemyAllClearDetector` remains the level-clear gate. Each enemy group registers once. The group unregisters when its remaining minion count reaches zero.
