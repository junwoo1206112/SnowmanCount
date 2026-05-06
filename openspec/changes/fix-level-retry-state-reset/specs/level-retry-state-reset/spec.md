## ADDED Requirements

### Requirement: Runtime sessions reset level progression state
The game SHALL reset static level progression state when a new runtime session starts.

#### Scenario: Fresh runtime session starts
- **WHEN** a new runtime session starts
- **THEN** the current level SHALL be reset to Level 1
- **AND** carry-over crowd count SHALL be cleared

### Requirement: All-levels-clear retry restarts from Level 1
The all-levels-clear retry action SHALL restart the game from Level 1 instead of reloading the missing level.

#### Scenario: Player retries after all levels are cleared
- **WHEN** the player clicks Retry on the all-levels-clear screen
- **THEN** the current level SHALL be set to Level 1
- **AND** the active scene SHALL reload with normal time scale

### Requirement: Ready scene load hides end-state UI
The game SHALL hide stale GameOver, LevelClear, and Retry UI when a scene loads into Ready state.

#### Scenario: Scene reloads after retry or level transition
- **WHEN** the active scene is loaded
- **THEN** end-state UI SHALL be hidden before Ready gameplay input is accepted

### Requirement: End-state transitions are accepted only during Play
The game SHALL ignore late level-clear and game-over signals after the game has already left Play state.

#### Scenario: Last enemy resolves after crowd depletion
- **WHEN** the crowd has already depleted and the game has entered GameOver
- **THEN** a late level-clear signal SHALL be ignored
- **AND** the game SHALL remain in GameOver

#### Scenario: Late collision resolves after level clear
- **WHEN** the game has already entered LevelClear
- **THEN** a late crowd-depleted signal SHALL be ignored
- **AND** the game SHALL remain in LevelClear
