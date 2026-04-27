## Why

Phase 1, 2에서 게임의 핵심 기능(이동, 병력, 게이트, 전투, 장애물)이 완성됨. 하지만 게임의 완성도를 높이려면 UI 개선과 게임 상태 관리가 필요함. 특히 게이트에 연산자 기호가 없어서 어떤 효과인지 알 수 없고, 게임오버 후 Retry 기능이 없음.

## What Changes

- **게이트에 연산자 기호 표시**: 파란색/빨간색 큐브 위에 TextMeshPro로 +5, x2, -3, ÷2 표시
- **진행 바**: 레벨 진행률 표시 (0% → 100%)
- **게임 상태 관리**: Ready (시작 대기) → Play (진행) → GameOver (종료)
- **Retry 버튼**: 게임오버 시 Retry 버튼으로 씬 리로드
- **레벨 클리어**: Finish 도달 시 Clear 화면 (선택)

## Capabilities

### New Capabilities
- `gate-label`: 게이트 위에 연산자 기호 표시
- `progress-bar`: 레벨 진행률 UI
- `game-state`: 게임 상태 전환 관리
- `retry`: 게임오버 후 재시작

### Modified Capabilities
- `ui-counter`: 게임오버 시 Retry 버튼 추가
