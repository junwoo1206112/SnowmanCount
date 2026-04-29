## ADDED Requirements

### Requirement: Each follower has a trigger Collider and "Follower" tag
Every spawned follower SHALL have a Collider component with `isTrigger = true` and the Unity tag "Follower".

#### Scenario: Follower spawns with Collider and tag
- **WHEN** ObjectPooler.GetPooledObject() is called
- **THEN** the returned object SHALL have a Collider with `isTrigger = true` and tag "Follower"

#### Scenario: Follower returned to pool resets tag
- **WHEN** ObjectPooler.ReturnToPool() is called
- **THEN** the object's Collider SHALL remain, tag SHALL remain "Follower"

#### Scenario: Follower does not collide with other followers
- **WHEN** two followers' Colliders overlap
- **THEN** no collision events SHALL fire (isTrigger has no physics collision between triggers)
