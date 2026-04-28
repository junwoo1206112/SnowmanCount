## Why

Count Masters 스타일 게임에서 레벨의 끝은 Finish 라인이 아니라 강력한 적 무리(보스전)로 마무리된다. 현재 Finish 라인은 엑셀 데이터에 존재하지 않아 아무 일도 일어나지 않는다. 레벨 마지막에 Enemy 대량을 배치하고, 모든 적을 처치해야 클리어되는 구조로 변경하여 게임의 재미와 난이도를 높인다.

## What Changes

- Finish 라인 제거 (더 이상 엑셀 데이터에서 Finish 타입 사용 안 함)
- LevelLoader가 마지막 오브젝트 배치 후 Enemy 대량을 추가 배치 (엑셀과 무관하게)
- 모든 EnemyGroup이 Destroy되면 Level Clear로 처리
- 레벨이 올라갈수록 적 수 증가 (Level1: 10마리, Level2: 20마리, Level3: 30마리...)
- 엑셀 데이터는 기존 게이트/장애물/소규모 적 배치만 담당

## Capabilities

### New Capabilities
- `enemy-wave-system`: 레벨 마지막에 Enemy 대량 무리 배치 및 전투
- `all-enemies-cleared-detection`: 모든 적 처치 시 Level Clear 감지

### Modified Capabilities
- `level-transition`: Finish 라인 대신 Enemy 전멸 시 Level Clear로 변경

## Impact

- `LevelLoader.cs`: SpawnFinish 제거, SpawnEnemyWave() 메서드 추가
- `FinishLineController.cs`: 제거 (더 이상 사용 안 함)
- `EnemyGroup.cs`: 모든 적 처치 감지 로직 추가
- `Levels.xlsx` 데이터: Finish 행 제거
- 레벨별 난이도 테이블 필요
