# Tasks: Crowd-Aware Road Width

- [x] 1. `LevelLoader.cs`에 `CROWD_MARGIN = 5.0f` 상수 추가
- [x] 2. `GetTotalWidth()` 수정: `(sm.XBound * 2f) + (CROWD_MARGIN * 2f)`
- [x] 3. 게이트 배치 로직 확인: 게이트가 여전히 리더의 조작 범위(`XBound`)에 맞춰 생성되는지 확인 (군중이 통과해야 하므로)
- [x] 4. 유니티에서 많은 수의 유닛을 데리고 도로 끝으로 이동하며 클리핑 여부 확인
