## ADDED Requirements

### Requirement: Enemy minions reliably resolve one-to-one contact
Enemy minions SHALL remove one player unit when close enough to contact it, even if a trigger event is missed. Matching SHOULD prefer the closest active follower in the enemy minion's narrow forward lane.

#### Scenario: Enemy reaches follower
- **WHEN** an enemy minion reaches a follower inside its duel lane
- **THEN** that follower SHALL be reserved for that enemy
- **AND** both units SHALL move toward a shared clash point
- **AND** the shared clash point SHOULD be pulled toward the player group's center line
- **AND** that follower SHALL be removed after the clash point is reached
- **AND** that enemy minion SHALL disappear

#### Scenario: Enemy reaches leader
- **WHEN** an enemy minion reaches the leader and no follower is available in its duel lane
- **THEN** one crowd count SHALL be removed
- **AND** that enemy minion SHALL disappear

### Requirement: Enemy waves use readable duel lines
Enemy minion waves SHALL be spawned as shallow rows so early contact reads as unit-against-unit lines.

#### Scenario: Small enemy group spawns
- **WHEN** a group contains six or fewer enemy minions
- **THEN** the minions SHALL spawn on a single horizontal line
