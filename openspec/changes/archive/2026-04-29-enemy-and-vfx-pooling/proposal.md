# Proposal: Enemy and VFX Pooling

## Why
1:1 전투 시스템이 도입됨에 따라 수십, 수백 명의 적 유닛과 전투 이펙트가 빈번하게 생성되고 파괴됨. 이는 가비지 컬렉션(GC) 부하를 일으켜 모바일 환경에서 프레임 드랍을 유발할 수 있음. 아군 유닛과 마찬가지로 적 유닛과 시각 효과도 오브젝트 풀링(Object Pooling)을 통해 관리해야 함.

## What Changes
- `ObjectPooler.cs` 고도화: 여러 종류의 프리팹을 관리할 수 있도록 범용적인 풀러로 리팩터링하거나, 전용 `EnemyPooler` 추가.
- `EnemyMinion.cs`: 파괴 시 `Destroy()` 대신 `Pool.ReturnToPool()` 호출.
- `LevelLoader.cs`: 적군 생성 시 `GameObject.CreatePrimitive` 대신 풀러에서 오브젝트를 가져오도록 수정.
- (추가) 전투 소멸 이펙트용 풀 추가.

## Impact
- 메모리 사용량 안정화 및 런타임 성능 향상.
- 대규모 전투 상황에서도 부드러운 프레임 유지.
- 향후 다양한 장애물이나 아이템 풀링으로 확장 가능.
