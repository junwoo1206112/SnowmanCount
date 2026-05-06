# Design: Giant Boss System

## Architectural Changes

### 1. BossController.cs (New)
- **Properties:** `int maxHP`, `int currentHP`, `float visualScale`.
- **Logic:** 
    - `Start()`: Register with `EnemyAllClearDetector`.
    - `OnTriggerEnter()`: If hit by Follower/Player, decrement HP, consume follower, update UI.
    - `Die()`: Unregister from detector, play effect, destroy.

### 2. LevelLoader.cs (Update)
- Update `SpawnEnemyWave` logic (or create `SpawnGiantBoss`):
    - Create a large Cube/Sphere (placeholder for boss).
    - Attach `BossController`, `WorldMover`, and `Rigidbody/Collider`.
    - Set HP based on `currentLevel` (e.g., `Level 1: 30 HP`, `Level 2: 60 HP`).

### 3. UI Integration
- Use a world-space Canvas or a simple overhead Text to display Boss HP.

## Implementation Details
- Boss will be placed at `maxDistance + 10f`.
- Boss scale will be `(5, 5, 5)` to look intimidating.
