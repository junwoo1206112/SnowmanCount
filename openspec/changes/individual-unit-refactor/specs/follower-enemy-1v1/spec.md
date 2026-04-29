## ADDED Requirements

### Requirement: Enemy minions have individual Colliders
Each child minion of an EnemyGroup SHALL have its own trigger Collider for individual combat detection.

#### Scenario: Enemy minion is spawned with Collider
- **WHEN** LevelLoader spawns an enemy group with minions
- **THEN** each minion GameObject SHALL have a Collider with `isTrigger = true`

#### Scenario: Follower collides with enemy minion
- **WHEN** a GameObject with tag "Follower" enters a minion's trigger Collider
- **THEN** both the follower and the minion SHALL be destroyed

#### Scenario: EnemyGroup parent does not have its own trigger
- **WHEN** the EnemyGroup parent GameObject has a Collider
- **THEN** the Collider SHALL NOT be a trigger (or SHALL be removed) — only children detect combat

#### Scenario: Enemy group clears when all minions destroyed
- **WHEN** all child minions of an EnemyGroup are destroyed
- **THEN** the parent EnemyGroup GameObject SHALL also be destroyed, and EnemyAllClearDetector SHALL be notified
