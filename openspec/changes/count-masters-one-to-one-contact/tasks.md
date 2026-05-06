## 1. Reliable Enemy Contact

- [x] 1.1 Add per-enemy contact radius
- [x] 1.2 Resolve nearby follower/leader in `FixedUpdate`
- [x] 1.3 Reuse the same one-shot resolver from trigger callbacks
- [x] 1.4 Replace spherical proximity priority with lane-based follower matching
- [x] 1.5 Spawn enemy minions in shallow duel lines
- [x] 1.6 Reserve matched followers and move both units toward a shared clash point before removal
- [x] 1.7 Pull clash points toward the player group center line for a compressed fight feel

## 2. Verification

- [x] 2.1 Static search confirms one-to-one resolver exists
- [x] 2.2 Unity log check shows no C# compile errors
- [x] 2.3 Static search confirms duel lane resolver exists
- [x] 2.4 Static search confirms clash movement state exists
- [x] 2.5 Static search confirms center-pulled clash settings exist
