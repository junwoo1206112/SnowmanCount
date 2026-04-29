# 카운트 마스터 스타일 구멍 시스템 구현 노트

이 문서는 플레이어의 이동 범위에 맞춰 도로가 생성되고, 물리적으로 구멍이 뚫리는 시스템의 구현 상세를 기록합니다.

## 1. 핵심 아키텍처

### 도로 타일 시스템 (LevelLoader.cs)
- **방식**: 고정된 커다란 바닥 대신, 일정한 간격(5m)의 도로 조각(Road Segment)을 동적으로 생성.
- **폭 자동 맞춤**: `SwerveMovement.xBound` 값을 참조하여 `width = xBound * 2`로 도로 스케일을 자동 설정.
- **구멍 생성**: `IsHoleAt()` 검사를 통해 구멍이 위치해야 할 구간에는 도로 조각을 생성하지 않음 (물리적 단절).

### 추락 및 제거 로직 (HoleController.cs)
- **트리거 감지**: 도로가 끊긴 구간에 `BoxCollider(isTrigger)`를 배치.
- **연출 순서**:
    1. 유닛이 트리거 접촉 시 `isFalling` 플래그 활성화.
    2. `CrowdController` 리스트에서 즉시 제거 (UI 숫자가 즉시 반영됨).
    3. `Rigidbody`를 활성화하고 아래로 힘(Force)을 가해 추락 연출.
    4. 0.7초 후 오브젝트를 풀(Pool)로 반환.

### 군집 이동 제어 (CrowdController.cs / FollowerComponent.cs)
- **이동 차단**: `isFalling` 상태인 유닛은 `Update` 루프의 이동 로직(`MoveFollower`)에서 제외하여 물리 엔진의 영향을 받도록 함.
- **상태 초기화**: 유닛이 풀에서 재사용될 때 `isFalling` 플래그 및 물리 속도를 초기화.

## 2. 주요 코드 변경 사항

### FollowerComponent.cs
- `isFalling` 변수 추가.
- `OnEnable()`에서 `Rigidbody` 속도 및 중력 설정 초기화.

### CrowdController.cs
- `RemoveFromList(GameObject follower)`: 리스트에서만 제거하고 숫자를 갱신하는 메서드.
- `ReturnFollowerToPool(GameObject follower)`: 연출 후 풀로 반환하는 메서드.
- `Update()`: `fc.isFalling` 체크 로직 추가.

### LevelLoader.cs
- `SpawnLevel()`: 도로 타일 생성 루프(`for`문) 추가.
- `SpawnRoadSegment()`: 플레이어 범위에 맞는 도로 조각 생성.
- `IsHoleAt()`: 특정 위치가 구멍 구간인지 판단.
- `SpawnHole()`: 도로가 없는 구간에 추락 트리거 생성.

## 3. 설정 팁
- **바닥 오브젝트**: 기존 씬에 수동으로 배치된 `Ground` 오브젝트는 삭제하거나 비활성화해야 동적 도로가 보입니다.
- **XBound 조정**: `SwerveMovement`의 `xBound` 값만 수정하면 도로 폭도 자동으로 따라옵니다.
