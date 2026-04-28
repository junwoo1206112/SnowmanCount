## ADDED Requirements

### Requirement: Enemy wave at end of level
LevelLoader SHALL spawn a large enemy group at the end of each level, positioned after all other objects.

#### Scenario: Enemy wave size by level
- **GIVEN** currentLevel = 1
- **WHEN** SpawnEnemyWave() is called
- **THEN** 10 enemies are spawned
- **GIVEN** currentLevel = 2
- **THEN** 20 enemies are spawned
- **GIVEN** currentLevel = N
- **THEN** N * 10 enemies are spawned

#### Scenario: Enemy wave position
- **GIVEN** level data maxDistance = 510
- **WHEN** SpawnEnemyWave() is called
- **THEN** enemy wave is positioned at distance 540 (maxDistance + 30)
