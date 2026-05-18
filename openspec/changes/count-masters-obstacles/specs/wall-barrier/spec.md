## ADDED Requirements

### Requirement: Wall barrier SHALL block part of the road

The wall barrier SHALL be a wall structure that blocks a portion of the road, leaving a gap for the player to pass through.

#### Scenario: Wall with left gap
- **WHEN** the wall barrier is spawned with value "left"
- **THEN** the wall SHALL occupy the right portion of the road, leaving a gap on the left side

#### Scenario: Wall with right gap
- **WHEN** the wall barrier is spawned with value "right"
- **THEN** the wall SHALL occupy the left portion of the road, leaving a gap on the right side

#### Scenario: Wall with center gap
- **WHEN** the wall barrier is spawned with value "center"
- **THEN** the wall SHALL have two wall segments on the left and right sides, with a gap in the center

### Requirement: Wall barrier SHALL remove followers on contact

When a follower or the player touches the wall barrier, the follower SHALL be removed from the crowd.

#### Scenario: Follower hits wall
- **WHEN** a follower collides with the wall barrier
- **THEN** the follower SHALL be removed from the crowd

### Requirement: Wall barrier SHALL have a distinct visual appearance

The wall barrier SHALL be visually distinct from other obstacles with a dark solid color.

#### Scenario: Wall has solid appearance
- **WHEN** the wall barrier is rendered
- **THEN** it SHALL appear as a tall solid wall structure with a dark color
