# Design: Robust Level Start & UI Management

## Key Architectural Updates

### 1. GameManager Initialization
```csharp
private void Awake()
{
    // ... singleton logic ...
    
    // ENSURE CLEAN START
    currentLevel = 1; 
    carryOverCrowdCount = -1;
}
```

### 2. Recursive UI Search
```csharp
private Transform FindDeepChild(Transform parent, string name)
{
    foreach (Transform child in parent)
    {
        if (child.name == name) return child;
        Transform result = FindDeepChild(child, name);
        if (result != null) return result;
    }
    return null;
}
```

### 3. Safety Buffers in LevelLoader
In `SpawnObject`, if `row.distance < 5.0f`, skip spawning if it's a hazard or move it further.

## Implementation Plan
1.  Modify `GameManager.cs`: Reset variables and add `FindDeepChild`.
2.  Update UI methods in `GameManager.cs` to use the new helper.
3.  Modify `LevelLoader.cs`: Add safety check for spawn distances.
