## Context
플레이어의 `SwerveMovement.xBound`가 30일 때, 도로 폭은 60m로 생성됨. 플레이어 피벗이 30 위치에 도달하면 캐릭터 모델의 절반이 30m 지점을 넘어가게 되어 도로 밖으로 보임.

## Goals / Non-Goals
**Goals:**
- 플레이어가 최대로 스와이프했을 때도 캐릭터 전체가 시각적으로 도로 내부에 위치하도록 함.
- 도로 폭을 `(XBound * 2) + 마진` 형태로 변경.
- 난간(`SideRail`) 위치가 늘어난 도로 폭에 맞춰 자동으로 바깥으로 밀려나게 함.

**Non-Goals:**
- `XBound` 값 자체를 수정 (플레이어 조작 범위는 유지).
- 도로의 텍스처나 모델 변경.

## Decisions
1. **마진(Margin) 상수 도입**: `LevelLoader.cs`에 `ROAD_MARGIN` 상수를 도입 (기본값 약 2.0m).
2. **TotalWidth 계산식 수정**: `GetTotalWidth()`에서 `(sm.XBound * 2f) + ROAD_MARGIN` 반환.
3. **난간 위치 자동 연동**: 이미 `SpawnSideRail`이 `currentRoadWidth`를 참조하므로 `GetTotalWidth()`만 수정하면 됨.

## Risks / Trade-offs
- 도로가 더 넓어지면 게이트나 장애물이 상대적으로 작아 보일 수 있음.
- 마진이 너무 크면 플레이어가 도로 끝에 도달하기 전에 벽에 막힌 느낌을 줄 수 있음 (적절한 값 2.0~3.0m 권장).
