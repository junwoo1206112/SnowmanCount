# Proposal: Crowd-Aware Road Width

## Why
현재 플레이어(리더)의 조작 범위와 도로 폭이 1:1로 일치하여, 리더가 도로 끝으로 이동할 경우 리더 주변의 군중(Followers)이 도로 밖으로 나가는 시각적 오류가 발생함.

## What Changes
- `LevelLoader.cs`에 `CROWD_MARGIN` (군중 여유분) 상수 도입 (기본값 5.0m 권장).
- `GetTotalWidth()` 계산식 수정: `(sm.XBound * 2f) + (CROWD_MARGIN * 2f)`.
- 이를 통해 리더가 `XBound` 끝에 도달해도, 좌우로 약 5m의 추가 공간이 있어 군중들이 도로 안에 머물게 함.

## Impact
- 군중이 난간 밖으로 튀어나가는 현상 해결.
- 시각적으로 더욱 안정적인 군중 이동 구현.
