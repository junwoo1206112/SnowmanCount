## ADDED Requirements

### Requirement: Gate detects individual followers passing through
GateController SHALL detect both "Player" (leader) and "Follower" tags. Each "Follower" passing through SHALL trigger one math operation. The "Player" passing through SHALL trigger math operation for all remaining followers.

#### Scenario: Follower passes through gate
- **WHEN** a GameObject with tag "Follower" enters the gate's trigger Collider
- **THEN** GateController SHALL apply one math operation to the crowd (e.g., +1 for "+" with value 1 per follower), increment a counter, and not disable the gate

#### Scenario: Leader passes through gate
- **WHEN** the GameObject with tag "Player" enters the gate's trigger Collider
- **THEN** GateController SHALL apply the math operation for ALL remaining followers at once, then disable the gate

#### Scenario: Gate prevents double-trigger from same follower
- **WHEN** the same follower stays inside the gate Collider
- **THEN** the gate SHALL NOT trigger twice for the same follower

#### Scenario: Gate disables after all uses exhausted
- **WHEN** gate has been triggered by all followers (counter == total followers at time of leader pass) or leader has passed
- **THEN** gate SHALL disable its Collider and Renderer

### Requirement: Per-unit gate operations
Gate math operations SHALL work per individual follower passing through:
- "+" : spawn 1 follower per operation call (value parameter unused for per-unit, or value = 1)
- "x" : spawn 1 additional follower (double the passing unit)
- "-" : remove 1 follower (the passing unit itself is consumed)
- "÷" : remove 1 follower (the passing unit is consumed, works as -1)

#### Scenario: Plus gate adds one unit per follower
- **WHEN** one follower passes through a "+" gate
- **THEN** CrowdController SHALL add 1 follower to the crowd

#### Scenario: Multiply gate adds one unit per follower
- **WHEN** one follower passes through a "x" gate
- **THEN** CrowdController SHALL add 1 follower to the crowd (doubling the passing unit)

#### Scenario: Minus gate removes passing follower
- **WHEN** one follower passes through a "-" gate
- **THEN** that follower SHALL be removed from the crowd

#### Scenario: Divide gate removes passing follower
- **WHEN** one follower passes through a "÷" gate
- **THEN** that follower SHALL be removed from the crowd
