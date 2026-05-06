# Excel Level Data Repository

## Why
The project uses NPOI, but the runtime service currently exposes only read-only `LoadLevel()` behavior. If Excel is the level authoring source, the DI service should support reading, saving, adding, updating, and deleting level rows through a repository abstraction.

## What Changes
- Add `ILevelDataRepository` extending `ILevelDataProvider`.
- Expand `NpoiLevelDataProvider` into a read/write Excel repository.
- Register the NPOI repository in `GameManager` DI.
- Keep ScriptableObject provider available as a read-only runtime provider.

## Impact
- Excel `Levels.xlsx` becomes the DI-backed editable level data source.
- Systems can request `GameManager.Instance.LevelDataRepository` for editor/tooling workflows.
- Existing `LoadLevel()` callers continue to work through `ILevelDataProvider`.
