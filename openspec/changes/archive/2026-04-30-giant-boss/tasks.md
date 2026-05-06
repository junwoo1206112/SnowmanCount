# Tasks: Giant Boss System

## 1. Core Implementation
- [ ] 1.1 Create `Assets/Scripts/Gameplay/BossController.cs`.
- [ ] 1.2 Implement HP reduction and follower consumption logic in `BossController`.
- [ ] 1.3 Implement Boss HP display (simple TextMeshPro or legacy Text).

## 2. Integration
- [ ] 2.1 Update `LevelLoader.cs` to spawn the Giant Boss instead of the Enemy Wave.
- [ ] 2.2 Adjust Boss HP scaling per level for balanced difficulty.

## 3. Visuals & Polishing
- [ ] 3.1 Add a "Hit" flash effect (color change) when boss takes damage.
- [ ] 3.2 Ensure the boss is large and positioned correctly on the road.

## 4. Verification
- [ ] 4.1 Verify Level 1 boss can be defeated and triggers Level 2.
- [ ] 4.2 Verify follower count decreases correctly when hitting the boss.
