## ADDED Requirements

### Requirement: Level clear screen shows crowd count
**WHEN** player reaches the finish line
**THEN** LevelClearText is displayed with the final crowd count
**THEN** a Next button is shown
**THEN** Time.timeScale is set to 0

#### Scenario: Next button advances to next level
- **WHEN** player clicks Next button
- **THEN** GameManager.LoadNextLevel() is called
- **THEN** currentLevel is incremented
- **THEN** carryOverCrowdCount is set to the final crowd count
- **THEN** scene is reloaded

#### Scenario: Retry button resets to level 1
- **WHEN** player clicks Retry button on level clear screen
- **THEN** carryOverCrowdCount = -1
- **THEN** currentLevel = 1
- **THEN** scene is reloaded (starting from Level 1)

#### Scenario: No automatic transition
- **WHEN** level is cleared
- **THEN** game waits indefinitely for player input (Next or Retry)
- **THEN** auto-loading coroutine is NOT used
