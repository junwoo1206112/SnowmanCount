# Design

## Interfaces
`ILevelDataProvider` remains the read-only contract:

```csharp
LevelData LoadLevel(int levelNumber);
```

`ILevelDataRepository` extends it with write operations:

```csharp
bool SaveLevel(LevelData levelData);
bool AddRow(int levelNumber, LevelRow row);
bool UpdateRow(int levelNumber, int rowIndex, LevelRow row);
bool DeleteRow(int levelNumber, int rowIndex);
```

## NPOI Repository
`NpoiLevelDataProvider` becomes the Excel repository implementation. It reads and writes `Assets/StreamingAssets/Levels/Levels.xlsx` by default.

## DI Registration
`GameManager` registers one `NpoiLevelDataProvider` instance as `LevelDataRepository` and exposes it as the default `LevelDataProvider`.

## Runtime Note
Writing to `StreamingAssets` is safest in the Unity Editor and desktop development builds. Platform-specific builds may need a persistent-data copy later.
