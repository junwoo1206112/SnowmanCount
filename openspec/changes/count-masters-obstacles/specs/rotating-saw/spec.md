## ADDED Requirements

### Requirement: Rotating saw SHALL spin continuously on Y-axis

The saw obstacle SHALL be a cylindrical object that rotates around its Y-axis at a constant speed.

#### Scenario: Saw rotates continuously
- **WHEN** the saw obstacle is spawned and visible
- **THEN** the saw SHALL rotate around its Y-axis at the configured speed

### Requirement: Rotating saw SHALL remove followers on contact

When a follower or the player touches the rotating saw, the follower SHALL be removed from the crowd.

#### Scenario: Follower hits rotating saw
- **WHEN** a follower collides with the saw obstacle
- **THEN** the follower SHALL be removed from the crowd and the saw SHALL be destroyed

### Requirement: Rotating saw SHALL have a metallic visual appearance

The saw SHALL have a metallic gray color with visible rotation animation.

#### Scenario: Saw has metallic appearance
- **WHEN** the saw is rendered
- **THEN** it SHALL appear as a metallic gray cylindrical object with a visible spinning animation
