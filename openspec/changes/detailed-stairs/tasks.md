## 1. Material Setup

- [x] 1.1 Create Gold_Emission Material asset (URP Lit, Metallic + Emission enabled)
- [x] 1.2 Create Gold_Dark Material asset for riser/rail accent

## 2. Step Geometry (Tread + Riser)

- [x] 2.1 Refactor step build loop: replace single Cube with tread Cube + riser Cube per step
- [x] 2.2 Apply Gold_Emission to tread, Gold_Dark to riser

## 3. Side Rails

- [x] 3.1 Add left/right rail Cube pair for each step
- [x] 3.2 Apply matching material and adjust rail dimensions

## 4. Individual Follower Blocks

- [x] 4.1 Generate small Cube block at each follower's landing position during hop
- [x] 4.2 Apply Gold_Emission to blocks, ensure correct Z-fighting prevention
- [x] 4.3 Add decorative top tiles and segmented front-face blocks so the stairs read like stacked bonus blocks

## 5. Crown Ornament

- [x] 5.1 Create crown/trophy decoration at the topmost step center using primitives
- [x] 5.2 Apply gold emissive material to crown

## 6. Size Adjustments

- [x] 6.1 Reduce step width: starting cols 14→8, width multiplier 0.85→0.55
- [x] 6.2 Reduce per-step capacity: starting rows 6→3
- [x] 6.3 Increase max steps: 16→30
- [x] 6.4 Fix extreme narrowing: cols reduce 1 per step → 1 per 2 steps
- [x] 6.5 Fix extreme row reduction: rows reduce 1 per 2 steps → 1 per 3 steps
- [x] 6.6 Faster taper: cols reduce 1 per step, rows reduce 1 per 2 steps
- [x] 6.7 Smooth taper: widen starting capacity and lerp step width so stairs narrow gradually
- [x] 6.8 Extend visible stair count and reduce per-step capacity for a Count Masters-style long bonus ladder
- [x] 6.9 Adjust step rise/depth and camera zoom for the longer staircase
- [ ] 6.10 Run in editor and verify staircase visual quality
- [ ] 6.11 Verify no regressions in follower hop animation or scoring
