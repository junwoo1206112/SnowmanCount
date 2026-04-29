## Requirements

### Requirement: Road width covers player movement with margin
The visual road SHALL be wider than the player's movement range to prevent visual clipping at the edges.

#### Scenario: Road width calculation
- **GIVEN** SwerveMovement.XBound = 30
- **GIVEN** ROAD_MARGIN = 2.0
- **WHEN** LevelLoader.GetTotalWidth() is called
- **THEN** it returns 62.0 (30 * 2 + 2)

#### Scenario: Side rail positioning
- **GIVEN** GetTotalWidth() returns 62.0
- **WHEN** SpawnSideRail is called for the right side
- **THEN** it is positioned at x = 31.15 (62 / 2 + 0.15)
- **THEN** the inner edge of the rail is at 31.0
- **THEN** at XBound = 30.0, a player with radius 0.5 is at 30.5, which is inside the rail edge (31.0)
