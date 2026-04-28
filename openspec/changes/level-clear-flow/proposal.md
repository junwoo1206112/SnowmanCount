## Why

Count Masters 스타일 게임에서 레벨 클리어는 단순히 "다음 레벨 로드"가 아니라, 클리어 화면에 병력 수를 보여주고 플레이어가 Next 버튼을 눌러 진행하는 명확한 피드백이 필요하다. 현재는 레벨 클리어 시 1.5초 딜레이 후 자동으로 넘어가고, 마지막 레벨 도달 시 아무 UI도 없어 플레이어가 진행 상태를 알 수 없다.

## What Changes

- Level Clear UI: 병력 수를 표시하고 Next 버튼을 누르면 다음 레벨로 진행
- All Levels Clear: 마지막 레벨 클리어 시 "All Levels Clear" 메시지 표시
- 레벨 번호 표시: 화면 상단에 현재 레벨 번호를 표시

## Capabilities

### New Capabilities
- `level-clear-screen`: 레벨 클리어 시 병력 수 표시 + Next 버튼
- `all-levels-clear`: 마지막 레벨 도달 시 종료 화면
- `level-number-display`: 화면 상단에 "Level N" 표시

### Modified Capabilities
- 없음

## Impact

- `GameManager.cs`: Level Clear UI 로직 변경 (자동 전환 → 버튼 입력)
- `LevelLoader.cs`: 마지막 레벨 처리
- 새 UI 프리팹 또는 Canvas 요소: LevelClearText, LevelNumberText
