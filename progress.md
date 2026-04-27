# Snowman Count — Progress Report

> 마지막 업데이트: 2026-04-27
> 다음 작업: Phase 4 (폴리싱) 또는 Phase 3 마무리 (진행 바 UI 연결)

---

## 완료된 Phase

### Phase 1 — 코어 시스템 ✅
| 시스템 | 설명 | 상태 |
|--------|------|------|
| SwerveMovement | 마우스 드래그 좌우 이동, Input Action Callback | ✅ |
| WorldMover | 게이트/적/장애물이 플레이어 쪽으로 이동 | ✅ |
| GroundScroller | 바닥 텍스처 스크롤 (아래→위) | ✅ |
| ObjectPooler | 눈사람 병력 50개 풀링, 자동 확장 | ✅ |
| CrowdController | 병력 생성/Lerp 이동/+-x÷ 연산/GameOver | ✅ |
| GateController | 게이트 통과 감지, 연산자 기호 표시 | ✅ |
| LevelLoader | 엑셀 데이터 로드, 오브젝트 자동 배치 | ✅ |
| GameManager | DI 컨테이너, 서비스 등록 | ✅ |
| UICounter | 병력 수 UI, GameOver 텍스트 | ✅ |

### Phase 2 — 전투 및 장애물 ✅
| 시스템 | 설명 | 상태 |
|--------|------|------|
| EnemyGroup | 적군 충돌 → 병력 감소 | ✅ |
| ObstacleController | 장애물 충돌 → 1회 데미지 + 파괴 | ✅ |
| Finish 라인 | 초록색 큐브 생성 (추후 클리어 처리) | ✅ |
| GameOver | 병력 0 → 텍스트 + Retry 버튼 + Time.timeScale=0 | ✅ |
| PlayerPivot Rigidbody | Kinematic, 충돌 활성화 | ✅ |
| 벽 제거 | 도로 폭만큼만 Ground, 벽 없음 | ✅ |

### Phase 3 — 데이터 및 UI (진행 중)
| 시스템 | 설명 | 상태 |
|--------|------|------|
| Gate 연산자 기호 | 큐브 앞면에 +5, x2 등 표시 | ✅ |
| 좌우 게이트 선택 | 2개 게이트 각각 다른 연산자 | ✅ |
| Retry 버튼 | GameOver 시 버튼 → 씬 리로드 | ✅ |
| 진행 바 (UIProgressBar) | 코드 준비, Canvas 연결 안 됨 | ❌ |
| 게임 상태 관리 | Ready/Play/GameOver 전환 | ❌ |

---

## 핵심 아키텍처 결정사항

### 스크롤러 방식
- 플레이어는 제자리(X축만 이동) + 월드가 다가옴
- Ground 텍스처 스크롤로 전진 착시
- 게이트/적/장애물은 WorldMover가 -Z 이동

### 엑셀 데이터 구조
- 위치: `StreamingAssets/Levels/Level_{D2}.xlsx`
- Distance(거리) | ObjectType(타입) | Value(좌측값) | SubValue(우측값)
- Gate: Value="+5", SubValue="x2" (좌우 다른 연산자)
- Enemy: Value="Flame", SubValue="5" (타입, 개수)
- Obstacle: Value="Saw", SubValue="1" (타입, 데미지)

### 순환 참조 해결
- Core.asmdef는 Data만 참조 (Gameplay/UI 참조 금지)
- GameManager는 SendMessage로 Gameplay/UI 제어
- CrowdController는 GameManager.Instance 직접 호출 가능

---

## 남은 작업

### Phase 3 마무리
- [ ] UIProgressBar Canvas 연결 (진행 바)
- [ ] 게임 상태 관리 (Ready → Play → GameOver)

### Phase 4 — 폴리싱
- [ ] 눈사람 3D 모델 교체 (Capsule 대신)
- [ ] 게이트 통과 시 파티클 이펙트
- [ ] 배경 음악 및 SFX
- [ ] 게이트 통과 시 숫자 pop-up 애니메이션 (UICounter에 있음)
- [ ] 빌드 테스트

### 알려진 버그
- [ ] Global Volume MissingReferenceException (에디터 전용, 게임플레이 무관)
- [ ] Gate 프리팹 스크립트 참조 깨짐 (Missing Script → Remove 후 GateController 재추가 필요)
