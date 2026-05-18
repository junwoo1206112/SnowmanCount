## ADDED Requirements

### Requirement: Victory staircase SHALL have 3D step geometry (tread + riser)

Each step in the victory staircase pyramid SHALL consist of two visual components:
- A **tread**: horizontal surface that followers stand on
- A **riser**: vertical front face connecting to the step below

#### Scenario: Step constructed with tread and riser
- **WHEN** the staircase is built during victory sequence
- **THEN** each step SHALL have a horizontal tread plane and a vertical riser face forming a solid 3D step appearance

### Requirement: Victory staircase SHALL have decorative side rails

The staircase SHALL have side rail/wall structures on both the left and right edges of each step, extending along the full depth of the staircase.

#### Scenario: Side rails present on staircase
- **WHEN** the staircase is fully built
- **THEN** both sides of the staircase SHALL have visible rail/wall structures running along the step edges

### Requirement: Each follower SHALL stand on an individual block

When followers hop onto the staircase, each follower SHALL be placed onto a small individual block rather than directly on the flat step surface.

#### Scenario: Follower positioned on individual block
- **WHEN** a follower completes its hop animation to a step position
- **THEN** there SHALL be a small block beneath the follower, distinct from the main step platform

### Requirement: Victory staircase SHALL read as stacked bonus blocks

Each staircase step SHALL include small decorative top tiles and segmented front-face blocks so the staircase appears built from detailed reward blocks rather than only large flat slabs.

#### Scenario: Step has block detail
- **WHEN** a staircase step is constructed
- **THEN** the step SHALL include multiple small top tiles across the standing surface
- **AND** the visible front face SHALL include segmented block details

### Requirement: Victory staircase SHALL have a long bonus-ladder silhouette

The victory staircase SHALL contain enough visible steps to read like a long Count Masters-style bonus ladder, with follower capacity spread across many narrow steps instead of being consumed by a few wide steps.

#### Scenario: Stair count is extended
- **WHEN** followers enter the victory staircase sequence
- **THEN** the generated staircase SHALL include a long visible run of steps
- **AND** follower placement capacity SHALL be distributed across many steps
- **AND** the camera SHALL zoom far enough to frame the extended staircase

### Requirement: Staircase material SHALL have metallic and emission properties

The staircase materials SHALL use metallic reflection and emission (glow) effects to create a premium visual appearance.

#### Scenario: Metallic and emissive appearance
- **WHEN** the staircase is rendered
- **THEN** the step materials SHALL exhibit metallic reflection and a visible glow/emission effect

### Requirement: Staircase top SHALL have a decorative crown ornament

The highest point of the staircase SHALL feature a decorative crown/trophy ornament to mark the peak.

#### Scenario: Crown ornament present at top
- **WHEN** the staircase construction is complete
- **THEN** the topmost step SHALL have a decorative crown/trophy object placed at its center
