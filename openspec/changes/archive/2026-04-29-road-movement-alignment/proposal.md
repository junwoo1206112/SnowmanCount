# Proposal: Road-Movement Alignment Fix

## Why
현재 플레이어의 이동 범위(`XBound`)와 도로의 너비(`TotalWidth`)가 정확히 2배 관계(`TotalWidth = XBound * 2`)로 설정되어 있어, 플레이어가 화면 끝으로 이동했을 때 캐릭터의 절반이 도로 밖으로 나가거나 난간에 겹쳐 보이는 현상이 발생함. 플레이어가 최대로 이동했을 때도 시각적으로 도로 위에 안정적으로 위치하도록 정렬 로직을 개선해야 함.

## What Changes
- `LevelLoader.GetTotalWidth()`에서 플레이어의 가시적 크기를 고려한 **Safety Margin(안전 여유분)**을 추가함.
- `TotalWidth = (XBound * 2) + (PlayerRadius * 2) + RailMargin`.
- 이를 통해 플레이어가 `XBound` 끝에 도달해도 발이 도로 안에 위치하고 난간과 겹치지 않게 함.

## Capabilities
### Modified Capabilities
- `level-loader-dynamic-width`: 플레이어 이동 범위에 따른 도로 폭 자동 계산 로직 개선

## Impact
- `LevelLoader.cs`: 도로 폭 계산 공식 수정
- 시각적 안정성 향상 및 클리핑 방지
