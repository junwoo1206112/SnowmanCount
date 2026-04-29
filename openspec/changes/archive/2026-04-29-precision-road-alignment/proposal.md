# Proposal: Precision Road-Movement Alignment

## Why
플레이어의 조작 한계점(`XBound`)과 도로의 시각적 끝부분(난간 안쪽 벽)을 완벽하게 일치시켜야 함. 현재는 2m의 마진이 있어 플레이어가 끝까지 가도 난간에 닿지 않는 어색함이 있음.

## What Changes
- `LevelLoader.cs`에서 `ROAD_MARGIN`을 제거하거나 0으로 설정.
- `GetTotalWidth()`가 `sm.XBound * 2f`를 정확히 반환하도록 수정.
- 난간 생성 로직(`SpawnSideRail`)이 `XBound` 위치에 정확히 안쪽 벽을 형성하는지 재확인.

## Impact
- 플레이어가 최대로 스와이프하면 캐릭터의 중심(Pivot)이 정확히 난간의 안쪽 면에 위치하게 됨.
- 조작 범위와 시각적 범위가 1:1로 일치함.
