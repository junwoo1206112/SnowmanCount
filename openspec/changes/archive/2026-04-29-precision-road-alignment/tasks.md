# Tasks: Precision Road Alignment

- [x] 1. `LevelLoader.cs`에서 `ROAD_MARGIN` 제거
- [x] 2. `GetTotalWidth()`가 `XBound * 2`만 반환하도록 수정
- [x] 3. `SpawnSideRail` 계산식 검증: 
  - `totalWidth / 2` (XBound) + `0.15` (난간 절반 폭) = `XBound + 0.15`
  - 난간 스케일 `0.3`의 절반인 `0.15`를 빼면 안쪽 벽 위치는 정확히 `XBound`. (검증 완료)
- [x] 4. 유니티에서 플레이어를 끝으로 이동시켜 난간에 딱 붙는지 확인
