## ADDED Requirements

### Requirement: Current level number displayed on screen
**WHEN** scene loads
**THEN** a Text element in the Canvas shows "Level {currentLevel}"
**THEN** the text is positioned at the top-center of the screen

#### Scenario: Level number updates on transition
- **WHEN** player advances to the next level
- **THEN** scene reloads
- **THEN** LevelText displays the new level number
