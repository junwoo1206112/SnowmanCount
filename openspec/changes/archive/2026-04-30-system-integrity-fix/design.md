# Design: System Integrity & Stability Fix

## Architectural Refactoring

### 1. The "Eternal" Road
`LevelLoader` will calculate the absolute end of the level (Castle Z) and ensure the road tiles (`SpawnRoadSegment`) cover the entire distance.
`roadLimitZ = maxDistance + 40f (Boss) + 80f (Stairs) + 50f (Buffer)`.

### 2. Global Ticker
`GameManager` will handle the "World Advancement":
```csharp
if (state == Play || state == BonusRound) 
    TotalDistanceTraveled += worldSpeed * Time.deltaTime;
```
Transient objects (`WorldMover`) will simply move at `worldSpeed` without trying to calculate global distance.

### 3. Shared Height Map
`SwerveMovement` (The Leader) will perform one raycast down.
```csharp
public float CurrentGroundY { get; private set; }
// ... Raycast ... hit.point.y ...
```
`CrowdController` will use `leader.CurrentGroundY` to set follower heights, significantly reducing CPU usage and eliminating feedback loops.

## Implementation Details
1.  **Core:** Reset statics and centralize distance.
2.  **Gameplay:** Update state filters for movement. Fix road length.
3.  **Optimization:** Implement the Shared Height Map.
