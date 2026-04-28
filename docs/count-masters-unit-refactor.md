# Count Masters-Style Individual Unit Refactor

> 이 문서는 현재 SnowmanCount 프로젝트를 Count Masters처럼 각 병력이 개별 유닛으로 동작하도록 리팩터할 때 필요한 작업을 정리합니다.

---

## 목표

현재: 리더(PlayerPivot)만 충돌 감지, 병력은 장식용 Lerp 팔로워
변경: 각 병력이 개별 Collider + 개별 이동 + 개별 충돌 처리

---

## 작업 목록

### 1. 각 병력에 Collider 추가

- ObjectPooler가 스폰하는 SnowGirl 프리팹에 Collider 추가
- Collider는 `isTrigger = true` (물리 충돌 없이 감지만)
- Rigidbody는 불필요 (직접 위치 제어)
- CrowdController.SpawnFollower()에서 병력 스폰 시 태그 설정 (예: "Follower")

### 2. CrowdController 수정 (개별 병력 관리)

- 현재: `activeCrowd` 리스트 + Dictionary로 위치 관리
- 변경: 각 병력이 자신의 목표 위치를 스스로 관리
- `SeparateFollowers()` 제거 (물리 충돌로 자연스럽게 분리)
- 병력 간 물리 충돌은 Collider + Rigidbody(isKinematic=false)로 처리 가능
- 리더가 이동하면 각 병력이 독립적으로 Lerp 이동

### 3. GateController 수정 (병력 줄지어 통과)

- 현재: 리더가 게이트에 닿으면 모든 병력 일괄 연산
- 변경: 각 병력이 게이트를 개별 통과
- 게이트 앞에 병력이 줄을 서서 한 명씩 지나감
- 통과한 병력만 연산 적용
- GateController가 병력 트리거 감지

### 4. EnemyGroup 수정 (1:1 충돌)

- 현재: 리더가 EnemyGroup에 닿으면 병력 일괄 감소
- 변경: 각 병력이 EnemyGroup의 개별 미니언과 1:1 충돌
- 아군 병력 vs 적군 미니언 충돌 시 서로 소멸
- `EnemyGroup.OnTriggerEnter`에서 태그 "Follower" 감지

### 5. HoleController 수정 (개별 병력 감지)

- 현재: 리더가 Hole에 닿으면 병력 절반 일괄 제거
- 변경: 각 병력이 Hole에 닿으면 해당 병력만 제거
- `HoleController.OnTriggerEnter`에서 태그 "Follower" 감지

### 6. 게이트 연산 수정 (개별 병력 단위)

- 현재: `CrowdController.ApplyMathOperation()`이 일괄 처리
- 변경: 게이트 통과 시 병력 1마리씩 연산
- `+5`: 병력 1마리가 통과할 때마다 5마리 추가 스폰
- `x2`: 통과한 병력 1마리당 1마리 추가 스폰
- `-3`: 통과한 병력 1마리당 3마리 제거 (음수 게이트 통과 가능?)
- `÷2`: 통과한 병력 1마리 제거 (절반)

---

## 고려사항

### 성능
- 병력 100마리 + 각각 Collider = 성능 부하
- Object Pooling 유지, Collider는 Pooling 시 재사용

### 병력 태그
- "Follower" 태그를 모든 병력에 할당
- PlayerPivot은 "Player" 태그 유지
- Gate/Enemy/Hole이 두 태그를 모두 감지하거나 분기 처리

### 물리 충돌 vs Trigger
- 병력 간 물리 충돌은 성능 이슈 가능
- `isTrigger = true` + 수동 위치 제어 권장
- 병력이 서로 겹치면 자연스러운 회피보다는 약간의 퍼짐 효과

### 리더 역할
- 리더(PlayerPivot)는 계속 존재 (SwerveMovement 유지)
- 리더가 먼저 전진하고 병력이 따라옴
- 리더가 죽으면(GameOver) 모든 병력도 정지

---

## 보류 결정

현재는 아래 이유로 리팩터를 보류함:
- 기존 코드 대규모 수정 필요 (3~5시간)
- 병력이 장식용이어도 게임플레이 자체는 정상 동작
- 폴리싱(모델/사운드/파티클)을 먼저 진행 중

리팩터를 원할 때 이 문서를 보고 작업을 시작하면 됩니다.
