## ADDED Requirements

### Requirement: Commercial runner HUD
**WHEN** the game scene loads
**THEN** the Canvas displays a top-center level header and progress bar suitable for a mobile runner
**AND** the current level number remains visible as `Level {currentLevel}`

#### Scenario: Progress updates during play
- **WHEN** `GameManager.TotalDistanceTraveled` increases
- **THEN** the progress fill updates from 0 to 1 using the configured level length

### Requirement: Crowd count badge
**WHEN** the crowd count changes
**THEN** the UI displays the new value inside a prominent snow power badge
**AND** the count animates with a brief pop feedback

### Requirement: Start and end state presentation
**WHEN** the game is ready
**THEN** a tap-to-start prompt is visible

**WHEN** play starts
**THEN** the tap-to-start prompt is hidden

**WHEN** the game ends or clears
**THEN** centered result text and retry controls use a polished panel presentation
