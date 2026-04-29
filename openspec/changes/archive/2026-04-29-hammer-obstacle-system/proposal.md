# Proposal: Hammer Obstacle System

## Why
현재 장애물은 정적인 상태로 배치되어 있어 난이도가 낮고 단조로움. 카운트 마스터의 핵심 재미 요소인 "타이밍 맞추기"를 위해 좌우로 흔들리며 광범위한 피해를 주는 망치(Hammer) 기믹이 필요함.

## What Changes
- `HammerController.cs` 신규 추가: 좌우 왕복 운동 및 충돌 로직 담당.
- `LevelLoader.cs` 수정: 엑셀 데이터에서 `Value: Hammer`인 장애물을 만났을 때 망치 모델을 조립하여 생성.
- `ObstacleController` 재활용: 망치 머리 부분에 기존 장애물 로직을 적용하여 유닛 제거.

## Impact
- 플레이어에게 타이밍을 계산하여 통과해야 하는 전략적 도전 과제 부여.
- 시각적으로 더욱 역동적인 레벨 구성.
