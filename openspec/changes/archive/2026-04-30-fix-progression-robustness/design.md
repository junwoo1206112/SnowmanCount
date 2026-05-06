# Design: Fix Level Progression and Robust Level Loading

## Architectural Improvements

### 1. GameStateManager Persistence
Modify `GameStateManager` to manage its own singleton lifecycle correctly.
```csharp
private void Awake()
{
    if (Instance != null && Instance != this)
    {
        Destroy(gameObject);
        return;
    }
    Instance = this;
    DontDestroyOnLoad(gameObject);
}
```

### 2. Flexible Level Name Matching
In `NpoiLevelDataProvider`, implement a fuzzy matcher for sheet names.
```csharp
private ISheet FindSheetCaseInsensitive(IWorkbook workbook, string targetName)
{
    string normalizedTarget = targetName.ToLower().Replace(" ", "");
    for (int i = 0; i < workbook.NumberOfSheets; i++)
    {
        string sheetName = workbook.GetSheetName(i).ToLower().Replace(" ", "");
        if (sheetName == normalizedTarget) return workbook.GetSheetAt(i);
    }
    return null;
}
```

### 3. GameManager Initialization Guard
Add a null check in `OnSceneLoaded` before accessing `GameStateManager`.

## Implementation Plan
1. Update `GameStateManager.cs` for persistence.
2. Update `NpoiLevelDataProvider.cs` for flexible sheet lookup.
3. Update `GameManager.cs` to ensure `currentLevel` and UI are handled safely during transitions.
