## ADDED Requirements

### Requirement: All levels cleared screen
**WHEN** LevelDataProvider.LoadLevel(levelNumber) returns null
**THEN** an "All Levels Clear" message is shown
**THEN** a Retry button is shown to restart from Level 1

#### Scenario: No more levels
- **WHEN** all level files (Level_01.xlsx through Level_N.xlsx) have been completed
- **THEN** LevelLoader.Start() detects null from data provider
- **THEN** Canvas shows "All Levels Clear" text
- **THEN** Retry button is shown
- **THEN** Retry resets currentLevel to 1 and reloads scene
