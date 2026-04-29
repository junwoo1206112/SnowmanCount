# Proposal: Preserve Formation on Unit Loss

## Why
현재 군중 시스템은 유닛이 제거될 때마다 실시간으로 대형을 재계산함. 이로 인해 구멍에 빠지거나 적과 충돌할 때 남은 유닛들이 갑자기 중앙으로 모여들며 "수축"하는 어색한 시각적 효과가 발생함. 카운트 마스터처럼 유닛 상실 시에는 현재 대형을 유지하고, 유닛 추가 시에만 대형을 갱신해야 함.

## What Changes
- `CrowdController.cs`에서 유닛 제거 메서드(`RemoveSpecificFollower`, `RemoveFromList`, `RemoveLastFollower`) 호출 시 `RedistributeAngles()`를 실행하지 않도록 수정.
- `SpawnFollowers()` (유닛 추가) 시에만 `RedistributeAngles()`를 호출하여 대형을 최적화.

## Impact
- 유닛이 죽을 때 남은 유닛들이 요동치지 않고 안정적으로 전진함.
- 장애물에 부딪힌 부위의 유닛들만 사라지는 자연스러운 연출 가능.
