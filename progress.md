# Snowman Count — Progress Report

> 마지막 업데이트: 2026-05-08
> 다음 작업: Phase 6 — 코인/보상 시스템

---

## 완료된 Phase

### Phase 1 — 코어 시스템 ✅
| 시스템 | 설명 | 상태 |
|--------|------|------|
| SwerveMovement | 마우스 드래그 좌우 이동, Input Action Callback | ✅ |
| WorldMover | 게이트/적/장애물이 플레이어 쪽으로 이동 | ✅ |
| GroundScroller | 바닥 텍스처 스크롤 (아래→위) | ✅ |
| ObjectPooler | 눈사람 병력 50개 풀링, 자동 확장 | ✅ |
| CrowdController | 병력 생성/이동/+-x÷ 연산/GameOver | ✅ |
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
| Enemy Wave 시스템 | 레벨 마지막에 Enemy 대량 배치 | ✅ |
| Enemy 전멸 클리어 | 모든 Enemy 파괴 시 Level Clear | ✅ |
| Level Clear 자동 전환 | Retry 버튼 없이 1.5초 후 자동 | ✅ |

### Phase 5 — 보스 및 전투 시스템 ✅
| 시스템 | 설명 | 상태 |
|--------|------|------|
| BossController | follower 충돌 → TakeDamage 즉시 소멸 | ✅ |
| 보스 Snowman 프리팹 | 보스에 Snowman 프리팹 할당 | ✅ |
| Excel Boss 데이터 | Level2 보스(550/50) | ✅ |
| 적 다중 링 진형 | SpawnMultiRingFormation | ✅ |
| 적 Swarm 돌진 | WorldMover → SetParent(null) → 개별 군집돌진 | ✅ |
| 적 Separation | OverlapSphere로 적끼리 밀쳐내기 | ✅ |
| 적 개별 타겟팅 | FindNearestAvailableFollower | ✅ |
| OnTriggerEnter 즉시 소멸 | 접촉 즉시 clash | ✅ |

### Phase 5b — 레벨 클리어/계단 연출 개선 ✅
| 시스템 | 설명 | 상태 |
|--------|------|------|
| FinishLineController | 골인 지점 (바닥 광선 + 깃발) | ✅ |
| Level1 Boss→Finish | 엑셀 데이터 Boss를 Finish로 변경 | ✅ |
| 피라미드 계단 (Count Masters 스타일) | 격자(grid) 배치, 중앙→바깥 순서 | ✅ |
| HopToPosition 호핑 애니메이션 | sin파 + 진동 효과, timeScale 무시 | ✅ |
| 계단 배율 라벨 | 각 계단에 ×1.1, ×1.2, ×2.3... TextMesh | ✅ |
| ZoomOutForStaircase | 카메라가 계단 전체를 조망 | ✅ |
| 보너스 웨이브 제거 | SpawnEnemyWave 완전 삭제 | ✅ |
| 레벨 전환 버그 수정 | ResetRuntimeState에서 currentLevel 리셋 제거 | ✅ |
| 적/장애물 클린업 | FinishLine+VictoryStaircase에서 전부 Destroy | ✅ |
| 엑셀 Gate 셀 string 강제 | SetCellType(String)으로 NPOI 자동변환 방지 | ✅ |

### Phase 6 — 코인/보상 시스템 ✅
| 시스템 | 설명 | 상태 |
|--------|------|------|
| PlayerPrefs 코인 저장 | SnowmanCount_Coins 키, 자동 로드/세이브 | ✅ |
| TotalCount (가상 카운트) | 화면 유닛과 별도로 총 카운트 추적 | ✅ |
| 유닛 시각적 제한 | maxFollowers=250 (렉 방지) | ✅ |
| 계단 배율 점수 | totalCount × 계단 배율 = 최종 점수 | ✅ |
| HUD 코인 표시 | 우측 상단 ◇ {totalCoins} (자동 생성) | ✅ |
| 보스 코인 보상 | 100 + 남은인원×5 | ✅ |

---

## 핵심 아키텍처 결정사항

### 스크롤러 방식
- 플레이어는 제자리(X축만 이동) + 월드가 다가옴
- Ground 텍스처 스크롤로 전진 착시
- 게이트/적/장애물은 WorldMover가 -Z 이동 (static totalDistanceTraveled로 진행률 계산)

### 엑셀 데이터 구조
- 위치: `StreamingAssets/Levels/Levels.xlsx`
- 시트명: `Level1`, `Level2` ...
- 열 구조: Distance | ObjectType(Gate/Enemy/Obstacle/Hole/Finish) | Value | SubValue
- Level1: Finish 라인 + 계단 연출
- Level2: 보스 처치 클리어

### 게임 상태 흐름
```
Ready ──(탭)──▶ Play ──(병력 0)──▶ GameOver ──(Retry)──▶ Ready (같은 레벨)
                  │
                  ├──(Finish 도달/Level1)──▶ LevelClear ──계단연출──▶ Level2
                  │
                  └──(보스 처치/Level2)──▶ LevelClear ──(1초)──────▶ Level1 (처음으로)
```

### DI 패턴
- GameManager 기반 수동 DI (서비스 로케이터)
- Gameplay → GameManager.Instance 직접 호출
- UI → Gameplay 컴포넌트 이벤트 구독 또는 FindFirstObjectByType

### 코인 시스템
- 저장: PlayerPrefs (SnowmanCount_Coins)
- 획득: totalCount × 계단 배율 (Level1) / 100 + count×5 (Level2 보스)
- 가상 카운트: TotalCount는 무제한, visual activeCrowd는 maxFollowers(250)로 제한
- UI: 우측 상단 CoinText (자동 생성, LegacyRuntime.ttf)

### 알려진 버그
- [ ] Global Volume MissingReferenceException (에디터 전용, 게임플레이 무관)
- [ ] Gate 프리팹 스크립트 참조 깨짐 (Missing Script → Remove 후 GateController 재추가 필요)
- [ ] 보스 HP UI 바 미구현 (TextMesh만 있음)
- [ ] FinishLineController Asset Store 미반영 (신규 스크립트)
