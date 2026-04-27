## Why

Phase 1에서 플레이어 이동, 병력 관리, 게이트 연산, 엑셀 데이터 로드가 완료됨. 이제 게임에 도전 과제를 추가해야 함. 적군과의 전투, 장애물 회피, 레벨 클리어 조건이 없으면 단순히 게이트만 통과하는 게임이 됨.

## What Changes

- **EnemyGroup**: 불꽃 정령 무리. 아군 눈사람과 1:1 충돌 → 서로 소멸. 병력 수로 승패 결정.
- **ObstacleController**: 회전 톱날(지속 데미지), 가시 함정(즉시 데미지), 녹는 구간(구역 데미지)
- **FinishLine**: 레벨 끝 도달 시 클리어 처리
- **엑셀 데이터에 Enemy/Obstacle/Finish 추가**: LevelLoader에서 실제 오브젝트 배치

## Capabilities

### New Capabilities
- `enemy-combat`: 적군 무리와 1:1 충돌 전투
- `obstacle-system`: 3종 장애물 (톱날, 가시, 녹는 구간)
- `level-finish`: 레벨 클리어 조건 및 처리

### Modified Capabilities
- `gate-math`: (변경 없음)
- `crowd-control`: 충돌 시 병력 소멸 로직 추가

## Impact

- `Assets/Scripts/Gameplay/EnemyGroup.cs` — 새 파일
- `Assets/Scripts/Gameplay/ObstacleController.cs` — 새 파일
- `Assets/Scripts/Gameplay/LevelLoader.cs` — SpawnEnemy/SpawnObstacle/SpawnFinish 구현
- `Assets/Scripts/Gameplay/CrowdController.cs` — RemoveCrowd 고도화 (n명씩 제거)
