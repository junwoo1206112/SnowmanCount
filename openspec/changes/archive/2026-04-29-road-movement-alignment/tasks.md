# Tasks: Road-Movement Alignment Fix

- [x] 1. `LevelLoader.cs`에 `ROAD_MARGIN` 상수(2.0f) 추가
- [x] 2. `LevelLoader.GetTotalWidth()` 메서드 수정: `(sm.XBound * 2f) + ROAD_MARGIN` 적용
- [x] 3. `SpawnGate()` 로직 확인: 게이트가 여전히 `XBound` 내부에 안전하게 배치되는지 검증
- [x] 4. 유니티에서 플레이어 이동 시 클리핑 여부 확인 (시각적 검증)
