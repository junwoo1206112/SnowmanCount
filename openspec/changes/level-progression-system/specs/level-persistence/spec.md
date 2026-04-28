## ADDED Requirements

### Requirement: Level persistence system
GameManager SHALL maintain current level number and crowd count across scene reloads using static fields.

#### Scenario: Level number persists after scene reload
- **WHEN** player clears a level
- **THEN** GameManager.currentLevel is incremented by 1
- **WHEN** scene reloads
- **THEN** LevelLoader loads the next level using Level_{D2}.xlsx format

#### Scenario: Crowd count carries over to next level
- **WHEN** player clears a level with N followers
- **THEN** GameManager.carryOverCrowdCount is set to N
- **WHEN** next level scene loads
- **THEN** CrowdController spawns N followers instead of initialCount

#### Scenario: Retry resets crowd count
- **WHEN** player clicks Retry
- **THEN** GameManager.carryOverCrowdCount is set to -1
- **WHEN** scene reloads
- **THEN** CrowdController spawns initialCount followers

#### Scenario: All levels cleared
- **WHEN** LevelDataProvider.LoadLevel(levelNumber) returns null
- **THEN** "All Levels Clear" message is shown instead of loading a level
