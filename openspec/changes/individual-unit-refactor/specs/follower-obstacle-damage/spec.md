## ADDED Requirements

### Requirement: Obstacle damages one follower at a time
ObstacleController SHALL detect "Follower" tag collisions and remove one follower per collision, then destroy itself.

#### Scenario: Follower touches obstacle
- **WHEN** a GameObject with tag "Follower" enters the obstacle's trigger Collider
- **THEN** the obstacle SHALL remove 1 follower from CrowdController and destroy itself

#### Scenario: Obstacle does not damage whole crowd
- **WHEN** a follower touches the obstacle
- **THEN** the obstacle SHALL NOT apply percentage-based damage to the entire crowd

#### Scenario: Leader touching obstacle still works
- **WHEN** the GameObject with tag "Player" enters the obstacle's trigger Collider
- **THEN** the obstacle SHALL also remove 1 follower (fallback — leader takes damage for the crowd)
