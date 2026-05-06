## ADDED Requirements

### Requirement: Level data repository supports Excel write operations
The level data service SHALL support saving, adding, updating, and deleting rows in the Excel level data source.

#### Scenario: Add a row
- **WHEN** a caller adds a row to a level
- **THEN** the repository SHALL append it to that level's sheet
- **AND** persist it to `Levels.xlsx`

#### Scenario: Update a row
- **WHEN** a caller updates a row by index
- **THEN** the repository SHALL replace the row data
- **AND** persist it to `Levels.xlsx`

#### Scenario: Delete a row
- **WHEN** a caller deletes a row by index
- **THEN** the repository SHALL remove that row
- **AND** persist the remaining rows to `Levels.xlsx`

### Requirement: GameManager exposes editable level data through DI
GameManager SHALL expose a repository service while preserving existing read-only provider access.

#### Scenario: Existing loader reads a level
- **WHEN** LevelLoader asks `GameManager.Instance.LevelDataProvider.LoadLevel(level)`
- **THEN** the call SHALL still return level data.

#### Scenario: Tooling edits a level
- **WHEN** tooling asks `GameManager.Instance.LevelDataRepository`
- **THEN** it SHALL be able to read and write Excel-backed level data.
