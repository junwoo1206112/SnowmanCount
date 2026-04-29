## Context
현재 아군 유닛(`FollowerComponent`)은 이미 개별 충돌체를 가지고 있을 가능성이 높으나(구멍 추락용), 적 유닛(`EnemyMinion`)은 단순히 시각적 구체(Sphere)일 뿐임.

## Goals / Non-Goals
**Goals:**
- 모든 적 미니언에게 `BoxCollider` 또는 `SphereCollider`(Trigger) 추가.
- 적 미니언이 아군 유닛의 태그("Player" 또는 "Follower")와 충돌 감지.
- 충돌 발생 시 즉시 양측 오브젝트 제거.

**Non-Goals:**
- 유닛 간의 복잡한 물리 밀치기(Rigidbody 힘 대결).
- HP 시스템 도입 (항상 1:1 교환).

## Decisions
1. **EnemyMinion 로직**: `OnTriggerEnter`가 발생하면 부딪힌 대상이 아군인지 확인 후, `CrowdController.RemoveSpecificFollower()` 호출 및 자신을 `Destroy()`.
2. **EnemyGroup 최적화**: 미니언들이 모두 파멸하면 `EnemyGroup` 오브젝트 자체도 깔끔하게 정리되도록 관리.

## Risks / Trade-offs
- 많은 수의 유닛이 동시에 충돌할 경우 물리 엔진 오버헤드 (최적화된 Trigger 사용으로 최소화).
- 1:1 교환이므로 아군 수가 적군보다 작으면 즉시 전멸하게 됨 (난이도 설계 시 주의).
