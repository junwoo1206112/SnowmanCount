## Context

`EnemyMinion` already detects contact with followers and the player leader. The current implementation moves the follower slightly toward the enemy and then immediately removes the follower and enemy. The desired effect is the inverse: the enemy minion should lunge or step toward the player unit, making combat read as an enemy attack.

## Goals / Non-Goals

**Goals:**
- Make follower contact show a short enemy-forward movement before removal.
- Keep the one-shot collision guarantee with `hasCollided`.
- Avoid physics impulses so combat remains stable for large crowds.
- Preserve existing pooling behavior.

**Non-Goals:**
- Do not redesign enemy formation, lane matching, or level spawning.
- Do not add new animation assets or external packages.
- Do not change crowd math, level progression, or obstacle behavior.

## Decisions

- Use a coroutine on `EnemyMinion` for the attack step. This keeps the behavior local to the collision owner and allows a short time-based movement before cleanup.
- Move the enemy transform directly with `Vector3.Lerp`. The project uses kinematic trigger-based minions, so direct transform motion is more predictable than Rigidbody forces.
- Disable the enemy collider when the attack starts. `hasCollided` already guards the script, but disabling the collider avoids extra trigger traffic while the minion is being removed.
- Keep leader contact immediate. The visible Count Masters-style beat is most important for follower-vs-enemy contact, while leader contact is the failure path and should remain simple.

## Risks / Trade-offs

- Multiple enemies can hit nearby followers in the same frame -> `hasCollided` and collider disable limit each enemy to one resolution.
- Follower is pooled after a delay -> during the short step it may still be visible, which is intentional for readability.
- If the follower disappears from another system before cleanup -> the coroutine checks for null and still removes the enemy safely.
