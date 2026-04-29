## Context
현재 `ObjectPooler.cs`는 아군 유닛 전용으로 하드코딩되어 있음. 적 유닛은 `GameObject.CreatePrimitive`로 매번 새로 생성되고 `Destroy`로 파괴됨.

## Goals / Non-Goals
**Goals:**
- 적 유닛(`EnemyMinion`)을 위한 오브젝트 풀링 구현.
- `ObjectPooler.cs`를 리팩터링하여 다양한 프리팹을 지원하도록 범용화.
- 1:1 전투 시 발생하는 잦은 생성/삭제 오버헤드 제거.

**Non-Goals:**
- 모든 맵 장애물(망치 등)까지 즉시 풀링 (필요 시 추후 확장).
- 복잡한 풀 매니저 싱글턴 구현 (현재의 컴포넌트 방식 유지).

## Decisions
1. **Generic ObjectPooler**: `SetupFollowerComponents`를 제거하고, 프리팹 자체에 필요한 컴포넌트가 이미 붙어있다고 가정하는 방식으로 간소화.
2. **Dedicated Instances**:
   - `AllyPooler`: 아군 프리팹 관리.
   - `EnemyPooler`: 적군 프리팹 관리.
3. **LevelLoader Integration**: `LevelLoader`가 `EnemyPooler`를 참조하여 적 미니언을 스폰.

## Risks / Trade-offs
- 프리팹 설정이 중요해짐 (기존에는 코드로 컴포넌트를 붙여줬으나, 이제 프리팹에 미리 붙어있어야 함).
- `OnEnable` 등 유니티 생명주기 관리에 주의해야 함 (풀에서 나올 때 상태 초기화).
