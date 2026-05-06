## Why

Counters Masters 스타일의 보스 시스템이 없어 레벨 종료 방식이 단조롭습니다. 각 레벨 마지막에 보스 전투를 추가하여 긴장감과 재미를 높이고, 보스를 처치해야 레벨 클리어가 되도록 합니다.

## What Changes

- 새로운 `Boss` 오브젝트 타입을 Excel 레벨 데이터에 추가
- 보스는 체력(HP)을 가지며, 주변에 미니언 링이 회전
- 보스 처치 시 레벨 클리어 (기존 EnemyWave 방식 대체 또는 보완)
- 보스 HP를 화면 상단에 UI로 표시
- 기존 `BossController.cs`를 확장하여 Counters Masters 스타일 전투 구현

## Capabilities

### New Capabilities
- `boss-battle`: 레벨 종료 지점에서 보스 전투 진행. 보스 HP, 미니언 링, 페이즈, 클리어 조건 포함
- `boss-data`: Excel 데이터 시트에 Boss 타입 정의. HP, 미니언 수, 난이도 파라미터 지정

### Modified Capabilities
- `enemy-system`: EnemyAllClearDetector가 보스도 클리어 조건으로 인식하도록 확장
- `level-progression`: 레벨 클리어 조건에 보스 처치 추가 (보스가 있는 레벨은 보스 처치가 클리어 조건)

## Impact

- `Assets/Scripts/Gameplay/BossController.cs`: 대규모 확장 (링 미니언, 페이즈, 패턴)
- `Assets/Scripts/Gameplay/LevelLoader.cs`: Boss 타입 스폰 로직 추가
- `Assets/Scripts/Gameplay/EnemyAllClearDetector.cs`: 보스 등록/해제 로직
- `Assets/Scripts/Data/NpoiLevelDataProvider.cs`: 변경 없음 (기존 ObjectType 파싱으로 충분)
- `Assets/StreamingAssets/Levels/Levels.xlsx`: 시트에 Boss 행 추가
