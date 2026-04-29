## Context
현재 `unitRadius`는 0.5f로 고정되어 있음. 100명일 때 대형의 반지름은 약 5~6m에 달함. 300명이 되면 도로를 가득 채우게 됨.

## Goals / Non-Goals
**Goals:**
- 유닛 수가 늘어남에 따라 개체 간의 간격을 자동으로 좁힘.
- 0~50명까지는 기본 간격 유지.
- 50~200명까지 선형적으로 간격을 좁힘 (최대 40%까지 축소).
- 200명 이상은 최소 간격 유지.

**Non-Goals:**
- 유닛 모델 자체의 스케일을 줄임 (모델 크기는 유지).
- 대형의 모양(원형)을 변경.

## Decisions
1. **Dynamic Multiplier 계산**: 
   - `count` < 50: `1.0`
   - `count` > 200: `0.6`
   - 중간: `Lerp` 적용.
2. **RedistributeAngles 적용**: `dynamicRadius`를 계산하여 링 간격과 링 내 슬롯 수 계산에 반영.
3. **Smooth Transition**: 유닛들이 갑자기 겹쳐 보이지 않도록 `MoveFollower`의 `Lerp` 속도를 통해 자연스럽게 이동시킴.

## Risks / Trade-offs
- 너무 촘촘하면 유닛끼리 겹침(Clipping)이 발생함 (시각적 허용 범위 내에서 조정).
- `RedistributeAngles`가 매번 호출되므로 CPU 부하가 늘어날 수 있으나, 500명 이하에서는 미미함.
