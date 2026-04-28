## ADDED Requirements

### Requirement: Progress bar uses actual level length
UIProgressBar SHALL use the actual maximum distance from level data instead of hardcoded 100f.

#### Scenario: Progress matches level distance
- **WHEN** LevelLoader finishes loading level data with maxDistance D
- **THEN** LevelLoader calls UIProgressBar.SetLevelLength(D)
- **THEN** progress bar fillAmount = -playerPivot.z / D
