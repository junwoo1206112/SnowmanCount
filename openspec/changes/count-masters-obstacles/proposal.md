## Why

현재 장애물은 Saw(회전 없는 회색 원통)와 Hammer 단 2종류로 매우 단순함. Count Masters 스타일의 다양한 장애물(회전 톱날, 벽+틈, 회전 바)을 추가하여 게임플레이의 재미와 난이도를 높이고자 함.

## What Changes

- **회전 톱날(Saw)** 신규 추가: 계속 회전하는 톱날 장애물 (기존 고정 원통 대체)
- **벽+틈(Wall)** 신규 추가: 도로를 가로막는 벽, 특정 위치에 틈(gap)이 있어서 통과해야 함
- **회전 바(Spinner)** 신규 추가: 중심축 기준으로 긴 막대가 회전하는 장애물
- 각 장애물 타입별 시각적 프리팹/메쉬 및 고유 동작 구현
- 장애물 데이터 모델 확장: positionX, width 등 추가 필드 (선택)

## Capabilities

### New Capabilities
- `rotating-saw`: 회전하는 톱날 장애물 — 지속 회전 + 충돌 시 병력 제거
- `wall-barrier`: 벽 + 틈 장애물 — 도로 폭을 가로막고 특정 위치만 통과 가능
- `spinning-bar`: 회전 바 장애물 — 중심축 기준 긴 막대 회전

### Modified Capabilities

<!-- None -->

## Impact

- `Assets/Scripts/Gameplay/LevelLoader.cs` — SpawnObstacle() 확장, 새 obstacle type 분기 처리
- `Assets/Scripts/Gameplay/ObstacleController.cs` — 타입별 동작 분기 로직 추가
- 신규 스크립트 파일들 (WallController, SpinnerController, SawController 등)
- 카메라 및 연출 변경 없음
