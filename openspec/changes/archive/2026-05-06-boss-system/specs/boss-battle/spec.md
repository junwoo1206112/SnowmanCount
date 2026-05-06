## ADDED Requirements

### Requirement: Boss blocks the road at level end

The boss SHALL be positioned at the farthest point of the level road and SHALL NOT move with WorldMover.
The boss SHALL block the entire road width so the player cannot pass without engagement.

#### Scenario: Boss positioned at level end
- **WHEN** the level data contains a Boss row at distance D
- **THEN** the boss SHALL spawn at position (0, 0, D)
- **AND** the boss SHALL NOT have a WorldMover component

#### Scenario: Player reaches boss
- **WHEN** the player's totalDistanceTraveled reaches the boss's position
- **THEN** the boss SHALL activate its ring minions
- **AND** the boss HP bar SHALL appear

### Requirement: Boss has HP and takes damage from followers

The boss SHALL have a configurable HP value set from Excel data. When a follower collides with the boss, the boss SHALL take 1 damage and the follower SHALL be removed.

#### Scenario: Follower hits boss
- **WHEN** a follower (FollowerComponent) collides with the boss trigger
- **THEN** boss currentHP SHALL decrease by 1
- **AND** the follower SHALL return to pool
- **AND** the boss SHALL flash red for 0.1s

#### Scenario: Player hits boss
- **WHEN** the player (tag "Player") collides with the boss trigger
- **AND** boss currentHP is greater than 1
- **THEN** GameManager.OnLeaderDied() SHALL be called (GameOver)

### Requirement: Boss ring minions orbit and charge

The boss SHALL have N ring minions that orbit around it. Each minion SHALL periodically charge toward the nearest follower, then return to orbit.

#### Scenario: Ring minion charges follower
- **WHEN** a ring minion detects a follower within charge range
- **AND** the minion is not already charging
- **THEN** the minion SHALL lerp toward the follower's position over 0.12s
- **AND** on collision, SHALL remove the follower and reset to orbit

#### Scenario: Ring minion returns to orbit
- **WHEN** the charge completes without collision
- **THEN** the minion SHALL lerp back to its orbital position

### Requirement: Boss phases change behavior at HP thresholds

The boss SHALL have 3 phases that trigger at HP thresholds:
- Phase 1 (HP > 50%): slow rotation speed, 4 minions
- Phase 2 (HP 25-50%): medium rotation speed, 8 minions, faster charge
- Phase 3 (HP < 25%): fast rotation speed, 12 minions, continuous charge

#### Scenario: Phase transitions
- **WHEN** boss HP drops below 50%
- **THEN** the boss SHALL transition to Phase 2
- **AND** ring minions SHALL be replaced with new count at higher speed

#### Scenario: Phase 3 activation
- **WHEN** boss HP drops below 25%
- **THEN** the boss SHALL transition to Phase 3
- **AND** ring minions SHALL charge continuously without return delay

### Requirement: Boss defeat clears the level

When boss HP reaches 0, the boss SHALL call EnemyAllClearDetector.UnregisterEnemy() and trigger level clear.

#### Scenario: Boss defeated
- **WHEN** boss currentHP reaches 0
- **THEN** the boss SHALL call UnregisterEnemy() on EnemyAllClearDetector
- **AND** GameManager.OnBossDefeated() SHALL be called
- **AND** the boss GameObject SHALL be destroyed

### Requirement: Boss HP is displayed

The boss HP SHALL be displayed as a world-space TextMesh above the boss and as a UI progress bar on screen.

#### Scenario: HP display updates
- **WHEN** boss takes damage
- **THEN** the TextMesh above the boss SHALL update to show current HP
- **AND** the UI HP bar SHALL update proportionally
