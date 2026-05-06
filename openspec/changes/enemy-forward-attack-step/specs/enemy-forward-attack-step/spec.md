## ADDED Requirements

### Requirement: Enemy minions step forward before defeating followers
Enemy minions SHALL perform a short visible movement toward a contacted follower before removing that follower and resolving the enemy minion.

#### Scenario: Enemy contacts follower
- **WHEN** an enemy minion contacts a follower
- **THEN** the enemy minion SHALL move toward that follower for a short attack step
- **AND** the follower SHALL remain visible until the attack step completes
- **AND** the follower SHALL be removed after the attack step completes
- **AND** the enemy minion SHALL be returned to its pool or destroyed after the attack step completes

#### Scenario: Enemy attack step starts
- **WHEN** an enemy minion begins its attack step
- **THEN** the enemy minion collider SHALL be disabled
- **AND** duplicate contact resolution SHALL be prevented

### Requirement: Leader contact remains deterministic
Enemy minions SHALL continue to remove one crowd count when contacting the player leader and no follower-specific attack step is available.

#### Scenario: Enemy contacts leader
- **WHEN** an enemy minion contacts the player leader
- **THEN** one crowd count SHALL be removed
- **AND** the enemy minion SHALL be returned to its pool or destroyed exactly once
