# Proposal: Adaptive Crowd Formation

## Why
현재 군중 형성 로직은 유닛 수에 상관없이 고정된 간격(`unitRadius`)을 사용함. 병력이 많아질수록(예: 100명 이상) 무리의 폭이 너무 넓어져 도로를 벗어나거나 시각적으로 산만해 보일 수 있음. 병력 수가 많아질수록 대형을 더 촘촘하게(Squeeze) 만들어 밀집도를 높여야 함.

## What Changes
- `CrowdController.cs`에 가변 밀도 로직 도입.
- 유닛 수에 따라 내부적으로 사용하는 `dynamicUnitRadius`를 계산.
- 병력이 적을 때는 널널하게, 병력이 많아질수록 `unitRadius`의 일정 비율까지 간격을 좁힘.
- `RedistributeAngles()`와 `GetSlotsInRing()`에서 이 동적 값을 사용하도록 수정.

## Impact
- 많은 병력이 모여도 대형이 일정 크기 이상으로 퍼지지 않고 밀집됨.
- 카운트 마스터 특유의 "밀집된 군중" 느낌이 강화됨.
- 도로 폭(`XBound`) 내에 군중을 유지하기가 더 쉬워짐.
