## ADDED Requirements

### Requirement: Excel Boss row format

The Levels.xlsx SHALL support a new "Boss" ObjectType. The row format SHALL be:
- Distance (A): float — boss spawn position on Z axis
- ObjectType (B): string = "Boss"
- Value (C): int — boss max HP
- SubValue (D): int — number of ring minions

#### Scenario: Boss row parsed
- **WHEN** LevelLoader reads a row with ObjectType "Boss"
- **THEN** it SHALL parse Value as bossHP (int)
- **AND** it SHALL parse SubValue as minionCount (int)

#### Scenario: Missing subValue defaults
- **WHEN** SubValue is empty or missing
- **THEN** minionCount SHALL default to 6

### Requirement: LevelLoader spawns Boss objects

The LevelLoader SHALL have a new case in SpawnObject() for "boss" that instantiates and configures the boss.

#### Scenario: Boss spawned via LevelLoader
- **WHEN** LevelLoader.SpawnObject() receives ObjectType "boss"
- **THEN** it SHALL create a new Boss GameObject at the specified position
- **AND** SHALL call BossController.Setup(hp, minionCount)
- **AND** the boss SHALL NOT get a WorldMover component
