# Tasks: Incremental Crowd Filling

- [x] 1. `CrowdController.cs`의 `SpawnFollowers()` 수정: 추가 후 `RedistributeAngles()` 호출 유지 확인
- [x] 2. `RedistributeAngles()` 로직 정교화: 
  - `dynamicRadius` 계산 시 현재 `activeCrowd.Count`를 더 안정적으로 반영
  - 모든 유닛에게 리스트 인덱스 순서대로 극좌표 할당 (기존 유닛들의 인덱스가 유지되므로 위치도 유지됨)
- [x] 3. 유니티에서 유닛이 하나씩 추가될 때 기존 유닛들이 가만히 있는지 확인
