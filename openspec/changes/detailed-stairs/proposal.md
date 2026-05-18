## Why

현재 승리 연출 계단(VictoryStaircase)이 단순 Primitive Cube 판(platform) 하나로만 구성되어 있어 시각적으로 밋밋함. Count Masters 스타일의 디테일한 계단 연출로 업그레이드하여 레벨 클리어 만족도와 게임의 전체적인 완성도를 높이고자 함.

## What Changes

- 계단 스텝 구조를 단일 판 → **tread(밟는 면) + riser(앞면) 3D 구조**로 변경 (두께감 있는 개별 계단)
- 계단 양옆에 **사이드 레일/벽** 추가 (벽돌 장식)
- 각 병력 착지 위치마다 **개별 기둥/블록** 생성 (Count Masters 스타일)
- Material 업그레이드: 금속 + Emission 효과, 그라데이션
- 꼭대기 **왕관/트로피 장식** 추가
- StairPiece 전용 Prefab 생성 (빌드 타임 최적화)

## Capabilities

### New Capabilities
- `victory-staircase-visuals`: 디테일한 승리 계단 연출 시스템 — 개별 스텝 블록, 사이드 레일, 장식, 향상된 Material

### Modified Capabilities

<!-- None -->

## Impact

- `Assets/Scripts/Gameplay/LevelLoader.cs` — VictoryStaircaseRoutine() 대폭 수정
- `Assets/Prefabs/` — StairPiece.prefab 신규 추가
- 새로운 Material 파일들 (Gold_Emission 등)
- 성능 영향: 스텝당 추가 오브젝트로 인한 오버헤드 (최대 16단 × 소폭 증가, 무시할 수준)
