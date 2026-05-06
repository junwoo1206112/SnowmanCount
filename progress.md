# Snowman Count — Progress Report

> 마지막 업데이트: 2026-05-06
> 다음 작업: Phase 5 — 폴리싱

---

## 완료된 Phase

### Phase 1 — 코어 시스템 ✅
| 시스템 | 설명 | 상태 |
|--------|------|------|
| SwerveMovement | 마우스 드래그 좌우 이동, Input Action Callback | ✅ |
| WorldMover | 게이트/적/장애물이 플레이어 쪽으로 이동 | ✅ |
| GroundScroller | 바닥 텍스처 스크롤 (아래→위) | ✅ |
| ObjectPooler | 눈사람 병력 50개 풀링, 자동 확장 | ✅ |
| CrowdController | 병력 생성/Lerf 이동/+-x÷ 연산/GameOver | ✅ |
| GateController | 게이트 통과 감지, 연산자 기호 표시 | ✅ |
| LevelLoader | 엑셀 데이터 로드, 오브젝트 자동 배치 | ✅ |
| GameManager | DI 컨테이너, 서비스 등록 | ✅ |
| UICounter | 병력 수 UI, GameOver 텍스트 | ✅ |

### Phase 2 — 전투 및 장애물 ✅
| 시스템 | 설명 | 상태 |
|--------|------|------|
| EnemyGroup | 적군 충돌 → 병력 감소 | ✅ |
| ObstacleController | 장애물 충돌 → 1회 데미지 + 파괴 | ✅ |
| GameOver | 병력 0 → 텍스트 + Retry 버튼 + Time.timeScale=0 | ✅ |
| PlayerPivot Rigidbody | Kinematic, 충돌 활성화 | ✅ |
| 벽 제거 | 도로 폭만큼만 Ground, 벽 없음 | ✅ |

### Phase 3 — 데이터 및 UI ✅
| 시스템 | 설명 | 상태 |
|--------|------|------|
| Gate 연산자 기호 | 큐브 앞면에 +5, x2 등 표시 | ✅ |
| 좌우 게이트 선택 | 2개 게이트 각각 다른 연산자 | ✅ |
| Retry 버튼 | GameOver 시 버튼 → 씬 리로드 | ✅ |
| 게임 상태 관리 | Ready → 탭 → Play → GameOver / LevelClear | ✅ |
| Ready "Tap to Start" | New Input System 기반 터치/클릭 감지 | ✅ |
| 레벨 병력 유지 | 레벨 클리어 시 병력 수를 다음 레벨로 carry-over | ✅ |
| 레벨 번호 표시 | 화면 상단 "Level N" (LevelText) | ✅ |
| Level Clear UI | 병력 수 표시 + 1.5초 후 자동 전환 | ✅ |

### Phase 4 — 인프라 개선 ✅
| 시스템 | 설명 | 상태 |
|--------|------|------|
| 단일 엑셀 파일 | Levels.xlsx에 시트(Level1, Level2...)로 통합 | ✅ |
| asmdef 순환 참조 해결 | Core→Data, Gameplay→Data+Core, UI→Gameplay | ✅ |
| Gameplay 속도 3배 | WorldMover 15, GroundScroller 1.5 (테스트용) | ✅ |
| Enemy Wave 시스템 | 레벨 마지막에 Enemy 대량 배치 | ✅ |
| Enemy 전멸 클리어 | 모든 Enemy 파괴 시 Level Clear | ✅ |
| 장애물/적 10% 데미지 | 현재 병력의 10% (최소 1) 깎기 | ✅ |
| Level Clear 자동 전환 | Retry 버튼 없이 1.5초 후 자동 | ✅ |

### Phase 5 — 보스 및 전투 시스템 ✅
| 시스템 | 설명 | 상태 |
|--------|------|------|
| BossController (단순화) | 미니언 제거, follower 충돌 → TakeDamage 즉시 소멸 | ✅ |
| 보스 SnowmanSmall 프리팹 | Capsule 대신 SnowmanSmall (2x 스케일), Collider 자동 추가 | ✅ |
| LevelLoader Boss 스폰 | SpawnBoss() + bossPrefab 필드 + "boss" 케이스 | ✅ |
| Excel Boss 데이터 | Level1(560/30/6), Level2(550/50/8) | ✅ |
| 적 다중 링 진형 | SpawnMultiRingFormation (Ring 0~N, CrowdController 방식) | ✅ |
| 적 Y 통일 | 0.5 → 0 (땅에 닿게) | ✅ |
| 적 Swarm 돌진 | WorldMover → SetParent(null) → 개별 군집돌진 (속도 18) | ✅ |
| 적 Separation | OverlapSphere로 적끼리 밀쳐내기 (넓은 전선 유지) | ✅ |
| 적 개별 타겟팅 | FindNearestAvailableFollower로 각자 가장 가까운 follower 추적 | ✅ |
| OnTriggerEnter 즉시 소멸 | pause/dash/각도매칭 제거, 접촉 즉시 clash | ✅ |
| PlayerPivot Y=0 통일 | 0.5 강제 제거, SnowGirl_2 local -0.5 → 0 | ✅ |
| ObjectPooler 자식화 | PlayerPivot 자식으로 이동 (하이라키 통일) | ✅ |
| GetPolarOffset Y=0 | followerYOffset 제거, 모든 유닛 동일 Y | ✅ |
| MoveFollower 즉시 위치 | Vector3.Lerp 제거, targetPos 즉시 설정 (본체+분신 일체 이동) | ✅ |
| 피라미드 계단 승리 연출 | 8877665544332211 구조, follower climbing, WorldMover 정지 | ✅ |
| Excel Hole 값 "left" | 빈 값 → "left" 명시적 지정 | ✅ |
| 죽은 코드 삭제 | BossMinionController, EnemyFormationController, temp 스크립트 정리 | ✅ |

---

## 핵심 아키텍처 결정사항

### 스크롤러 방식
- 플레이어는 제자리(X축만 이동) + 월드가 다가옴
- Ground 텍스처 스크롤로 전진 착시
- 게이트/적/장애물은 WorldMover가 -Z 이동 (static totalDistanceTraveled로 진행률 계산)

### 엑셀 데이터 구조
- 위치: `StreamingAssets/Levels/Levels.xlsx`
- 시트명: `Level1`, `Level2`, `Level3` ...
- 열 구조: Distance | ObjectType(Gate/Enemy/Obstacle/Hole/Boss) | Value | SubValue
- 더 이상 Finish 타입 사용 안 함 (Enemy 전멸로 클리어)

### asmdef 참조 구조 (순환 참조 방지)
```
Data (참조 없음)
  ↑
Core → Data + Unity.InputSystem
  ↑
Gameplay → Data + Core + Unity.InputSystem
  ↑
UI → Gameplay
```

### 게임 상태 흐름
```
Ready ──(탭)──▶ Play ──(병력 0)──▶ GameOver ──(Retry)──▶ Ready (같은 레벨)
                  │                                      (currentLevel 유지)
                  └──(Enemy 전멸)──▶ LevelClear ──계단연출──▶ 다음 레벨
                                                             (carry-over)
                                                             (currentLevel++)
```

### DI 패턴
- GameManager 기반 수동 DI (서비스 로케이터)
- Gameplay → GameManager.Instance 직접 호출
- UI → Gameplay 컴포넌트 이벤트 구독 또는 FindFirstObjectByType

### 중요 수정사항
- `GameManager.OnCrowdCountChanged()` 제거 → `UICounter`가 CrowdController.OnCrowdCountChanged 직접 구독
- `GameManager.OnLevelCleared()`가 파라미터로 crowdCount 받음
- GameOver Retry는 같은 레벨 유지, Level Clear Retry는 Level 1로 리셋
- `Input.GetMouseButtonDown` → New Input System으로 변경
- `GameObject.FindWithTag("Player")` → `WorldMover.totalDistanceTraveled`로 진행률 계산
- `LevelDataService.cs` 제거 (dead code)
- `NpoiLevelDataProvider.GetCellIntValue()` 제거 (dead code)

### 플레이어 Y 통일 (2026-05-06)
- PlayerPivot Y=0 (SwerveMovement 0.5 강제 제거)
- SnowGirl_2 local Y=-0.5 → 0 (자식 오프셋 제거)
- GetPolarOffset Y=-0.5 → 0 (followerYOffset 필드 제거)
- ObjectPooler Y=-0.5 → 0, PlayerPivot 자식으로 이동
- 모든 유닛 동일 Y=0에서 하나의 영역으로 동작

### 적 전투 시스템 (Count Masters 스타일, 2026-05-06)
- ✅ 다중 링 진형 (SpawnMultiRingFormation)
- ✅ 적 Y=0 통일 (WorldMover로 접근)
- ✅ Aggro 30m → SetParent(null) → 개별 돌진 (swarmSpeed 18)
- ✅ Separation (OverlapSphere 2m, 적끼리 밀쳐내기)
- ✅ 각자 가장 가까운 follower 타겟팅 (FindNearestAvailableFollower)
- ✅ OnTriggerEnter 즉시 소멸 (코루틴/대시/pause 없음)
- ✅ 보스: SnowmanSmall 프리팹 + SphereCollider(1.5) + TakeDamage
- ✅ 피라미드 계단 (8877665544332211, 최대 72명)

---

## 알려진 버그
- [ ] Global Volume MissingReferenceException (에디터 전용, 게임플레이 무관)
- [ ] Gate 프리팹 스크립트 참조 깨짐 (Missing Script → Remove 후 GateController 재추가 필요)
- [ ] Save/Load 시스템 없음 (게임 재시작 시 레벨 초기화)
- [ ] 보스 HP UI 바 미구현 (TextMesh만 있음)
- [ ] 구멍 타일 시각적 생성 확인 필요 (GetHoleTypeAt 로직은 정상)

---

## OpenSpec 활성 변경사항

| Change | 상태 | 설명 |
|--------|------|------|
| `phase-1-swerve-crowd-controller` | ⏳ 17/20 | CameraFollow, GateController labelObject 수정, 테스트 미완 |
| `level-progression-system` | ✅ 완료 | 레벨 간 병력 유지, 씬 리로드 |
| `level-clear-flow` | ✅ 완료 | Level Clear UI, All Clear, Level 번호 |
| `single-excel-multi-levels` | ✅ 완료 | Levels.xlsx 통합 |
| `enemy-wave-clear` | ✅ 완료 | Enemy 전멸 클리어, 난이도 조절 |
| `boss-system` | ✅ 완료 | 보스 시스템 (단순화) |
| `enemy-formation-fix` | ✅ 완료 | 적 WorldMover 속도 통일, 다중 링 진형 |
| `hole-y-fix` | ✅ 완료 | HoleType 방지, 모든 유닛 Y=0 통일 |

