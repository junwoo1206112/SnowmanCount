## ADDED Requirements

### Requirement: Followers move independently toward leader
Each follower SHALL move independently toward a target position relative to the leader, without rigid ring formation indexing.

#### Scenario: Follower moves toward its assigned offset
- **WHEN** Update() runs and game state is Play
- **THEN** each follower SHALL Lerp toward its assigned position (leader position + radial offset)

#### Scenario: Crowd size change redistributes followers
- **WHEN** a follower is added or removed from the crowd
- **THEN** all followers SHALL have their angles evenly redistributed around the leader

#### Scenario: Followers spread organically
- **WHEN** multiple followers exist
- **THEN** each follower SHALL have a unique angle evenly distributed (360 / count) so they spread around the leader

#### Scenario: Follower does not block other followers
- **WHEN** followers move toward their targets
- **THEN** they SHALL pass through each other (no physics blocking)
