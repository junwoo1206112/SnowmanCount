## ADDED Requirements

### Requirement: Enemy minions resolve combat one-to-one
Each enemy minion SHALL resolve a single collision against a player follower or leader.

#### Scenario: Follower collides with enemy minion
- **WHEN** a follower touches an enemy minion
- **THEN** the follower SHALL be removed from the crowd
- **AND** that enemy minion SHALL disappear
- **AND** no additional followers SHALL be removed by that enemy minion

#### Scenario: Leader collides with enemy minion
- **WHEN** the leader touches an enemy minion
- **THEN** the crowd count SHALL decrease by one
- **AND** that enemy minion SHALL disappear

### Requirement: Enemy group clears after all minions disappear
EnemyGroup SHALL remain registered as active until all child enemy minions are defeated.

#### Scenario: Last enemy minion disappears
- **WHEN** an enemy group's remaining minion count reaches zero
- **THEN** the group SHALL unregister from `EnemyAllClearDetector`
- **AND** the group object SHALL be destroyed
