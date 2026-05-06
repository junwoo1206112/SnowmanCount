# Design: Fix Stationary Player and Deadly Holes

## Architectural Adjustments

### 1. Movement Sync
The player's Z-position will be locked. 
`SwerveMovement.cs` will handle X-axis (swerve) and Y-axis (only in BonusRound/Falling).
`forwardSpeed` in `SwerveMovement` is now purely for reference.

### 2. Hole Interaction
`HoleController` will use `other.CompareTag("Player")` to identify the leader.
The leader will have a falling animation (simple downward velocity) followed by `OnLeaderDied`.

### 3. Raycast Filtering
Height detection is now state-dependent.
```csharp
if (state == BonusRound) 
{
    // Perform Raycast
}
else 
{
    // targetY = 0.5f (unless falling)
}
```

## Implementation Plan
1. Update `HoleController.cs` to handle Player and Followers.
2. Update `SwerveMovement.cs` to remove Z-movement and fix verticality.
3. Update `CrowdController.cs` to restrict height logic to BonusRound.
