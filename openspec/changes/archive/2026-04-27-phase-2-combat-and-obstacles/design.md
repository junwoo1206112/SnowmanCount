## Context

Phase 1에서 플레이어 이동, 병력 관리, 게이트 연산 완료. Phase 2에서 적군 전투, 장애물, 레벨 클리어를 추가. 버그 리포트에서 태그 문제, GameOver 연결 문제, Obstacle 첫 데미지 문제 발견하여 모두 수정함.

## Goals / Non-Goals

**Goals:**
- EnemyGroup: 1:1 충돌 소멸, ApplyMathOperation("-", count) 호출
- ObstacleController: OnTriggerStay로 지속 데미지
- Finish: 충돌 감지 (추후 클리어 화면)
- GameOver: OnCrowdDepleted → UICounter.ShowGameOver() + Time.timeScale = 0
- PlayerPivot Tag = "Player" (필수, 에디터 설정)

**Non-Goals:**
- 보스전 (Phase 4)
- 레벨 전환 화면 (Phase 3)

## Decisions

1. **GameManager가 SendMessage로 UI 제어**: 순환 참조 방지를 위해 Gameplay/UI asmdef를 직접 참조하지 않고 SendMessage 사용
2. **WorldMover가 모든 오브젝트 이동**: 플레이어는 제자리, 월드가 다가옴 (Count Masters 방식)
3. **Obstacle은 OnTriggerStay 사용**: 구역 내에 머무는 동안 지속 데미지

## Risks

- **[태그] PlayerPivot Tag 설정 누락** → 모든 충돌 실패. AGENTS.md에 강제 명시 필요
- **[순환 참조] Core asmdef가 Gameplay/UI 참조 시 순환 발생** → SendMessage 우회
