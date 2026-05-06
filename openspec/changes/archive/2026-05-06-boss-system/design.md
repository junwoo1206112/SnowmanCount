## Context

현재 레벨은 EnemyWave가 전멸하면 자동 클리어됩니다. Counters Masters 스타일의 보스를 추가하여 레벨 종료 지점에 보스 전투를 배치하고, 보스를 처치해야 클리어되도록 합니다.

기존 `BossController.cs`는 기본 HP/피격/사망 처리는 있지만, 링 미니언, 페이즈, 패턴이 없습니다. 이번 설계는 보스를 Counters Masters 스타일로 확장합니다.

## Goals / Non-Goals

**Goals:**
- 보스는 레벨 끝에서 길을 막고 서있음 (WorldMover 이동 없음)
- 보스 주변에 미니언 링이 회전하며 플레이어 병력과 충돌
- 보스 HP가 일정 이하로 떨어지면 페이즈 전환 (미니언 재생성, 패턴 변경)
- 보스 HP를 월드 UI(TextMesh)와 화면 상단 UI 바에 표시
- Excel 데이터에 Boss 타입 정의 (HP, 미니언 수)
- 보스 처치 시 EnemyAllClearDetector.UnregisterEnemy() → 레벨 클리어
- 기존 EnemyWave와 공존 가능 (보스 없는 레벨은 기존 방식 유지)

**Non-Goals:**
- 보스의 복잡한 AI 행동 패턴 (단순 링 미니언 + 돌진)
- 멀티플레이어
- 보스 전용 애니메이션 (3D 모델 교체는 추후)

## Decisions

**1. 링 미니언 시스템 — BossMinionController (신규)**
- 보스 자식으로 N개의 미니언이 일정 반경을 회전
- 각 미니언은 주기적으로 가장 가까운 follower를 향해 돌진 (charge)
- 돌진 후 원위치로 복귀
- 미니언이 follower와 충돌하면 follower 1 제거, 미니언은 리스폰
- 회전 속도/반경은 HP 페이즈에 따라 변화

**2. 보스 페이즈 — BossController 확장**
- Phase 1 (HP 100~50%): 느린 회전, 미니언 4~6마리
- Phase 2 (HP 50~25%): 빠른 회전, 미니언 8~10마리, 돌진 간격 단축
- Phase 3 (HP 25~0%): 매우 빠른 회전, 미니언 12~15마리, 지속 돌진
- 페이즈 전환 시 미니언 재생성 + 이펙트

**3. 보스 피해 — follower 충돌 방식**
- follower가 보스 본체에 충돌하면 보스 HP 1 감소 + follower 제거
- player가 보스 본체에 충돌하면 GameOver (기존 BossController 로직 유지)
- 보스 HP는 Excel data에서 지정 (기본값 30)

**4. 클리어 조건 — EnemyAllClearDetector 연동**
- BossController.Start()에서 clearDetector.RegisterEnemy() 호출 (이미 구현됨)
- BossController.Die()에서 clearDetector.UnregisterEnemy() 호출 (이미 구현됨)
- 보스가 등록되면 기존 EnemyWave의 UnregisterEnemy도 함께 동작하여 ALL 클리어 조건 달성

**5. Excel 데이터 포맷**
- 기존 ObjectType 열에 "Boss" 추가
- Value: HP (예: 30)
- SubValue: 미니언 수 (예: 6)
- 예시: `500 | Boss | 30 | 6`
- LevelLoader에서 "boss" 케이스 추가

**6. 보스 배치**
- 레벨의 가장 먼 거리(마지막 EnemyWave 이후)에 배치
- 보스는 WorldMover 컴포넌트를 가지지 않음 (정지)
- 보스 뒤로는 도로가 없음 (막다른 길)

## Risks / Trade-offs

- [Ring minion collision] 미니언이 너무 많으면 성능 이슈 → ObjectPooler 재사용, 최대 15마리 제한
- [Boss positioning] WorldMover 없는 보스는 플레이어가 도달할 때까지 기다려야 함 → totalDistanceTraveled로 보스 활성화 시점 결정
- [Phase transition] 페이즈 전환 중 버그 가능성 → 간단한 State Machine 사용, 코루틴 중복 방지
