## Context

현재 LevelLoader.SpawnObstacle()은 `obstacleType`이 "hammer"인지만 확인하고, 나머지는 전부 동일한 회색 Cylinder로 생성. ObstacleController는 단순 Trigger 사망 로직만 있음. 새로운 장애물 타입을 추가하려면 SpawnObstacle 분기 + 각 타입별 동작 스크립트가 필요.

## Goals / Non-Goals

**Goals:**
- 회전 톱날(Saw): Cylinder가 Y축으로 계속 회전, 충돌 시 병력 사망
- 벽+틈(Wall): 도로 폭의 벽을 만들고 특정 위치에 틈 생성, 충돌 시 병력 사망
- 회전 바(Spinner): 긴 Cube 막대가 중심축 기준 회전, 충돌 시 병력 사망
- 각 타입별 시각적 구분 (색상, 형태)
- 장애물 데이터 모델 확장 (positionX 등)

**Non-Goals:**
- 장애물 사운드 효과
- 파티클/폭발 이펙트
- 장애물 HP 시스템 (여전히 1-hit 제거)
- 기존 Hammer 동작 변경

## Decisions

### 1. 타입별 분기: ObstacleType enum + switch
- **결정:** ObstacleController에 `ObstacleType` enum 추가 (`Saw`, `Wall`, `Spinner`, `Generic`)
- **이유:** SpawnObstacle에서 타입에 따라 생성 로직 분기. 각 타입별 고유 컴포넌트를 추가하거나 ObstacleController에서 직접 처리
- **대안:** 별도 클래스 계층 — 오버엔지니어링.

### 2. Saw: Cylinder + Y축 회전
- **결정:** Cylinder를 만들고 SawController 컴포넌트 추가하여 회전. 색상: 금속 회색
- **회전 속도:** `rotationsPerSecond = 2` (기본값)
- **이유:** Primitive Cylinder가 원형이라 방향 신경 안 쓰고 회전 가능

### 3. Wall: 여러 Cube로 벽 + 틈 구성
- **결정:** SpawnObstacle에서 `value` 파싱 (예: "left", "right", "center")으로 틈 위치 결정
- **틈 너비:** roadW * 0.25 (도로 폭의 25%)
- **구조:** 왼쪽 벽 + 오른쪽 벽 (틈 사이) — 2개의 Cube. 색상: 진한 회색
- **높이:** 3유닛, **두께:** 0.5유닛
- **월드 컨트롤러:** WallCollisionTrigger라는 별도 Trigger 추가 (또는 기존 ObstacleController 재사용)

### 4. Spinner: 중심축 + 회전 막대
- **결정:** 부모 GameObject에 SpinnerController 추가, 자식으로 긴 Cube 막대
- **회전 속도:** 90도/초 (기본)
- **막대 길이:** roadW * 0.8, 두께 0.3, 높이 0.5
- **위치:** y=1.5 (지상에서 약간 떠있음)

### 5. 장애물 데이터 확장 (선택)
- **결정:** positionX는 랜덤/중앙 고정 대신 SpawnObstacle 내부에서 타입별로 계산
- **이유:** Excel 데이터 구조 변경 필요 없음, 코드 내에서 처리 가능
- Wall은 value("left"/"right"/"center")로 틈 위치 지정

## Risks / Trade-offs

- **[Performance]** Saw 연속 회전 + Spinner 지속 회전 = Update() 부하 — 미미함
- **[Code Cleanliness]** SpawnObstacle이 타입별로 길어짐 — 헬퍼 메서드 분리로 대응
- **[Balance]** Wall 틈 위치가 항상 중앙/좌/우 고정 → 난이도 조절 한계 — 추후 positionX 필드 추가 가능
