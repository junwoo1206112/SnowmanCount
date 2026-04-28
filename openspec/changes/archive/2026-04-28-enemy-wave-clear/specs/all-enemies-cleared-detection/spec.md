## ADDED Requirements

### Requirement: Level clear when all enemies defeated
A detector SHALL track all EnemyGroup instances and trigger Level Clear when all are destroyed.

#### Scenario: All enemies defeated
- **WHEN** all EnemyGroup objects in the scene have been destroyed
- **THEN** GameManager.Instance.OnLevelCleared(currentCrowdCount) is called
- **THEN** GameState transitions to LevelClear

#### Scenario: Some enemies remain
- **WHEN** at least one EnemyGroup object still exists
- **THEN** game continues in Play state
- **THEN** player can still be defeated (crowd count reaches 0 = GameOver)

#### Scenario: No enemies in level
- **WHEN** SpawnEnemyWave() has not been called (should not happen normally)
- **THEN** Level Clear is NOT triggered automatically
