## 1. ObstacleType Enum

- [x] 1.1 Add ObstacleType enum to ObstacleController (Saw, Wall, Spinner, Generic)
- [x] 1.2 Add obstacleType field + SetObstacleType method

## 2. Rotating Saw

- [x] 2.1 Create SawController with continuous Y-axis rotation
- [x] 2.2 Update SpawnObstacle: Cylinder with metallic gray + SawController
- [x] 2.3 Assign obstacleType = Saw on ObstacleController

## 3. Wall Barrier

- [x] 3.1 Create wall builder: parse value ("left"/"right"/"center") for gap position
- [x] 3.2 Spawn wall segments (2 Cubes) with gap, dark color, ObstacleController on each
- [x] 3.3 Adjust wall height (3u), thickness (0.5u), gap width (roadW * 0.25)

## 4. Spinning Bar

- [x] 4.1 Create SpinnerController with continuous bar rotation
- [x] 4.2 Spawn parent + child bar Cube (roadW * 0.8), orange accent
- [x] 4.3 Add ObstacleController to bar, set ObstacleType.Spinner

## 5. Spawn Integration

- [x] 5.1 Update LevelLoader.SpawnObstacle()/case statement: "saw", "wall", "spinner"
- [x] 5.2 Verify existing "hammer" and generic obstacles still work

## 7. Road Length & Overlap Fix

- [x] 7.1 Increase road padding: maxDistance +50 → +80
- [x] 7.2 Fix test data conflicts: revert Excel, use non-conflicting distances
- [x] 7.3 Update AddObstacleTestData with safe distances (18, 55, 90, 150, 185...)
- [ ] 7.4 Run "Tools → Add Obstacle Test Data" in Unity after reimport
- [ ] 7.5 Verify in editor: saw rotates, wall has gap, spinner spins
- [ ] 7.6 Verify no regressions in existing behavior