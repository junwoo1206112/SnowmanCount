# Snowman Count — Game Design Spec

## 개요
Count Masters 스타일의 3D 러너 게임.
플레이어는 눈사람 무리를 이끌고 얼음 세계를 달리며,
게이트로 병력을 증강하고 적군을 격파하여 최종 보스까지 도달한다.

---

## 1. 핵심 시스템 (Core Features)

### 1.1 스와이브 제어 & 자동 전진 (Swerve & Auto-Run)
- **전진**: 플레이어는 일정 속도로 +Z축 방향으로 자동 전진
- **조작**: 마우스 좌클릭 드래그(또는 터치 드래그)로 X축 좌우 이동
- **제한**: `Mathf.Clamp`로 도로 경계를 벗어나지 않도록 이동 범위 제한
- **카메라**: 3인칭 후면 추적, 플레이어 무리 전체가 보이도록俯角

### 1.2 군중 연산 (Crowd Math Logic)
게이트 통과 시 눈사람 병력 수가 실시간 연산된다.

| 게이트 색 | 연산 | 설명 |
|-----------|------|------|
| 파란색 (Blue) | +, × | 병력 증가 (강화) |
| 빨간색 (Red) | -, ÷ | 병력 감소 (약화) |

- 연산 결과는 상단 UI 카운트에 pop-up 애니메이션과 함께 즉시 갱신
- 병력이 0 이하가 되면 게임 오버

### 1.3 지능형 군중 대형 (Dynamic Formation)
- **리더 추적**: 모든 눈사람은 Player Pivot 기준 일정 반경 내 랜덤 목표 지점으로 `Lerp` 이동
- **충돌 회피**: 개체 간 겹침 방지, 자연스러운 무리 형성
- **오브젝트 풀링**: 활성/비활성화로 메모리 최적화

### 1.4 병력 충돌 전투 (Crowd vs Crowd)
- 아군 눈사람 <-> 적군 불꽃 정령이 1:1 충돌 시 **서로 소멸**
- 병력 수가 많은 쪽이 밀어내고, 적은 쪽은 전멸
- 전투 중 실시간으로 양측 카운트 감소

---

## 2. 장애물 & 적군 (Obstacles & Enemies)

| 종류 | 컨셉 | 로직 |
|------|------|------|
| 불꽃 정령 (Flame Spirit) | 빨간색 불꽃 몬스터 무리 | 아군과 1:1 충돌 소멸, 병력 많으면 승리 |
| 회전 톱날 (Spinning Saw) | 날카로운 톱날 장애물 | 접촉 시 일정 시간마다 병력 감소 |
| 가시 함정 (Spike Trap) | 바닥 솟는 가시 | 즉시 병력 감소 |
| 녹는 구간 (Melting Zone) | 여름 바닥 효과 | 구역 통과 중 지속적으로 카운트 감소 |

---

## 3. 데이터 중심 레벨 디자인 (Data-Driven Design)

### CSV/Excel 데이터 구조

```
Distance, ObjectType, Value, SubValue
10,     Gate,        +,     5
25,     Gate,        x,     2
40,     Enemy,       Flame, 3
55,     Obstacle,    Saw,   1
70,     Gate,        -,     3
100,    Gate,        ÷,     2
...
```

- **Distance**: 게이트/장애물이 등장할 Z축 거리
- **ObjectType**: Gate, Enemy, Obstacle, Finish, Boss, Hole
- **Value**: 연산자(+, -, x, ÷) 또는 적 타입
- **SubValue**: 수치 또는 적 병력 수

### LevelManager
- 게임 시작 시 CSV/Excel 로드
- 플레이어 진행 거리에 따라 오브젝트를 순차적으로 배치
- 완료 후 다음 레벨 or 보스전 진입

### All Levels Clear
- **WHEN** `LevelDataProvider.LoadLevel(levelNumber)` returns null (시트 없음)
- **THEN** "All Levels Clear!" 메시지 표시 (LevelClearText)
- **THEN** Retry 버튼 표시 (Level 1로 리셋)
- **THEN** `Time.timeScale = 0` (일시정지)

---

## 4. UI 구성

| 요소 | 설명 |
|------|------|
| 상단 카운트 | 현재 눈사람 병력 수 (실시간 갱신, pop-up 효과) |
| 레벨 번호 | 상단 중앙 "Level {N}" (씬 로드 시 업데이트) |
| 진행 바 | 레벨 진행률 (0% ~ 100%) |
| 게임 오버 | 병력 0 도달 시 표시, Retry 버튼 (같은 레벨) |
| 레벨 클리어 | 보스 처치 or 끝 도달 시 병력 수 표시 + 계단 연출 후 Final Score |
| 올 클리어 | 모든 레벨 완료 시 "All Levels Clear!" + Retry 버튼 (Level 1) |

---

## 5. 추천 클래스 구조 (C#)

```
Scripts/
├── Core/
│   ├── GameManager.cs          — 전체 게임 상태 관리 (Singleton)
│   ├── SwerveMovement.cs       — 드래그 입력 → X축 이동
│   └── ObjectPooler.cs         — 눈사람/이펙트 풀링
├── Gameplay/
│   ├── CrowdController.cs      — 눈사람 무리 관리, 연산 처리
│   ├── CrowdFollower.cs        — 개별 눈사람 행동 (Lerp 이동)
│   ├── GateController.cs       — 게이트 통과 감지 및 연산
│   ├── EnemyGroup.cs           — 적군 무리 관리
│   └── ObstacleController.cs   — 장애물 피해 처리
├── Data/
│   ├── LevelData.cs            — CSV 파싱 데이터 모델
│   └── LevelLoader.cs          — CSV 로드 및 스테이지 빌드
├── UI/
│   ├── UICounter.cs            — 상단 카운트 표시
│   ├── UIProgressBar.cs        — 진행 바
│   └── UIManager.cs            — 게임오버/클리어 팝업
└── Managers/
    └── GameStateManager.cs     — 상태 전환 (Ready/Play/GameOver/Clear)
```

---

## 6. 개발 로드맵 (Phase)

### Phase 1 — 코어 시스템
- [x] 프로젝트 세팅 (URP, Input System)
- [ ] SwerveMovement + 자동 전진
- [ ] CrowdController 기본 병력 생성
- [ ] 게이트 연산 (+, -, x, ÷)

### Phase 2 — 전투 & 장애물
- [ ] EnemyGroup 충돌 전투
- [ ] 장애물 (톱날, 가시, 녹는 구간)
- [ ] 오브젝트 풀링 최적화

### Phase 3 — 데이터 & UI
- [ ] CSV/Excel 레벨 데이터 파싱
- [ ] LevelLoader 스테이지 자동 빌드
- [ ] UI (카운트, 진행바, 팝업)
- [ ] 게임 상태 관리 (Ready/Play/GameOver/Clear)

### Phase 4 — 폴리싱
- [ ] 눈사람 대형 최적화
- [ ] 이펙트/사운드
- [ ] 빌드 및 테스트
