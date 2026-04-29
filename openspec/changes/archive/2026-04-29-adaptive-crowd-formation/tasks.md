# Tasks: Adaptive Crowd Formation

- [x] 1. `CrowdController.cs`에 밀집도 관련 변수 추가 (`minRadiusMultiplier = 0.6f`, `maxSqueezeCount = 200`)
- [x] 2. `GetDynamicRadius()` 메서드 구현: 현재 카운트에 따른 반지름 계산
- [x] 3. `RedistributeAngles()` 수정: `GetDynamicRadius()` 값을 사용하여 대형 재계산
- [x] 4. `GetSlotsInRing()` 수정: 인자로 `dynamicRadius`를 받도록 변경
- [x] 5. 유니티에서 병력을 대량으로 늘려보며 대형이 촘촘해지는지 확인
