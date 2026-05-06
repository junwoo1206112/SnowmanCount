# Design

## Contact Resolution
`EnemyMinion` asks `CrowdController` for the closest active follower inside a narrow X/Z duel window. This makes each enemy minion pair with a nearby unit in the same forward lane instead of consuming an arbitrary collider inside a spherical radius.

## One-To-One Rule
Each `EnemyMinion` can resolve only once. A follower collision removes that exact follower. A leader collision removes one crowd count.

## Clash Movement
When an enemy minion finds a follower in its duel lane, the follower is reserved so other enemies cannot target it. Both units move toward a shared clash point for a short clash beat, then the follower and enemy are removed together once they meet.

The clash point is pulled toward the player group's center line instead of using only the raw midpoint. This gives the Count Masters style effect where opposing units compress toward the middle of the fight before disappearing.

## Enemy Formation
`LevelLoader` lays enemy minions out in shallow rows with a capped column count. Small enemy groups appear as a visible line, and larger groups become multiple forward rows.

## Trigger Fallback
`OnTriggerEnter` still calls the same resolver so normal Unity trigger behavior continues to work.
