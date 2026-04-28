## ADDED Requirements

### Requirement: Level transition on finish
FinishLineController SHALL trigger next level loading when player reaches the finish line.

#### Scenario: Finish triggers next level
- **WHEN** player collides with Finish line
- **THEN** GameState is set to LevelClear
- **THEN** GameManager.LoadNextLevel() is called
- **THEN** crowd count is saved
- **THEN** scene is reloaded with incremented level number

#### Scenario: LevelClear UI is shown briefly
- **WHEN** LevelClear state is entered
- **THEN** LevelClearText is displayed
- **WHEN** 1.5 seconds pass
- **THEN** next level loads
