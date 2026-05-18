## ADDED Requirements

### Requirement: Spinning bar SHALL rotate around center axis

The spinning bar obstacle SHALL have a long horizontal bar that rotates around a central pivot point.

#### Scenario: Bar rotates continuously
- **WHEN** the spinning bar is spawned and visible
- **THEN** the bar SHALL rotate around its center axis at the configured speed

### Requirement: Spinning bar SHALL remove followers on contact

When a follower or the player touches the spinning bar, the follower SHALL be removed from the crowd.

#### Scenario: Follower hits spinning bar
- **WHEN** a follower collides with the spinning bar
- **THEN** the follower SHALL be removed from the crowd and the bar SHALL be destroyed

### Requirement: Spinning bar SHALL have a distinct visual appearance

The spinning bar SHALL be visually distinct with a long rectangular shape and contrasting color.

#### Scenario: Bar has distinct appearance
- **WHEN** the spinning bar is rendered
- **THEN** it SHALL appear as a long horizontal bar with a bright/orange accent color
