## Why

Count Masters 스타일 게임에서 핵심 진행 방식은 유한한 길이의 레벨을 클리어하고, 다음 레벨로 병력을 유지하며 진행하는 것이다. 현재 게임은 Level 1만 로드되고 클리어 후 다음 레벨로 넘어가는 시스템이 없어 게임 루프가 완성되지 않았다.

## What Changes

- 레벨 클리어 시 다음 레벨 번호를 가지고 씬을 리로드
- GameManager가 현재 레벨 번호와 병력 수를 씬 간 유지 (DontDestroyOnLoad)
- CrowdController가 시작 시 저장된 병력 수를 복원
- LevelLoader가 레벨 번호에 따라 엑셀 파일 로드
- Retry는 현재 레벨로 다시 시작 (병력 초기화)
- UIProgressBar가 실제 레벨 길이를 반영하도록 수정

## Capabilities

### New Capabilities
- `level-persistence`: 레벨 간 병력 수와 레벨 번호를 유지
- `level-transition`: 레벨 클리어 → 다음 레벨 로드
- `progress-bar-fix`: UIProgressBar가 실제 maxDistance를 사용하도록 수정

### Modified Capabilities

- 없음

## Impact

- `GameManager.cs`: 현재 레벨 번호 저장, 씬 리로드 로직 변경
- `CrowdController.cs`: 저장된 병력 수로 시작하는 기능 추가
- `LevelLoader.cs`: maxDistance를 UIProgressBar에 전달
- `UIProgressBar.cs`: SetLevelLength() 메서드 사용
- `FinishLineController.cs`: 다음 레벨 로드 트리거
