## Why

현재는 `StreamingAssets/Levels/Level_01.xlsx`, `Level_02.xlsx` 등 레벨마다 개별 파일로 관리한다. 레벨이 늘어날수록 파일이 많아져 관리가 어렵고, NPOI를 도입한 의미가 퇴색된다. 하나의 엑셀 파일에 여러 시트(Sheet)로 레벨을 통합하면 파일 관리가 쉽고 데이터 일관성이 높아진다.

## What Changes

- `StreamingAssets/Levels/Levels.xlsx` 단일 파일로 통합
- 각 레벨은 시트 이름 `Level1`, `Level2`, `Level3` ... 으로 구분
- `NpoiLevelDataProvider.LoadLevel(int levelNumber)`가 시트 이름 `Level{levelNumber}`를 읽도록 변경
- 기존 `Level_01.xlsx` 등 개별 파일 제거 및 `Levels.xlsx`로 데이터 이관
- `GameManager.currentLevel`이 시트 인덱스로 사용됨

## Capabilities

### New Capabilities
- `single-excel-storage`: 단일 엑셀 파일에서 시트별 레벨 데이터 로드

### Modified Capabilities
- 없음

## Impact

- `NpoiLevelDataProvider.cs`: LoadLevel 내부 로직 변경 (파일명 → 시트명)
- `StreamingAssets/Levels/`: Level_XX.xlsx 삭제, Levels.xlsx 생성
- `ExcelLevelCreator.cs`: 에디터 도구도 시트 기반으로 변경
