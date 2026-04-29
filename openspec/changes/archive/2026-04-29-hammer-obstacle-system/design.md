## Context
망치는 도로 위쪽에 매달려 좌우로 흔들리는 시계추(Pendulum) 방식으로 작동함.

## Goals / Non-Goals
**Goals:**
- 코드로 망치 모델(자루 + 머리)을 조립 생성.
- `Mathf.Sin`을 이용한 부드러운 좌우 왕복 운동.
- 망치 머리에 닿은 유닛들을 리스트에서 제거.

**Non-Goals:**
- 물리 엔진(Hinge Joint 등)을 사용한 복잡한 물리 계산 (성능을 위해 코드 이동 선호).
- 정교한 3D 애니메이션 파일 사용.

## Decisions
1. **모델 구조**:
   - `HammerRoot`: 회전 중심점 (도로 위 공중)
   - `Handle`: 얇고 긴 큐브
   - `Head`: 크고 무거운 큐브 (여기에 `ObstacleController` 부착)
2. **운동 파라미터**: `SwingSpeed` (속도), `SwingAngle` (각도)를 설정 가능하게 함.
3. **충돌 처리**: 기존 `ObstacleController`를 그대로 사용하여 코드 중복 방지.

## Risks / Trade-offs
- 망치가 너무 빠르면 유저가 불합리하다고 느낄 수 있음 (속도 조절 필요).
- 도로 너비(60m)가 매우 넓으므로 망치의 크기나 흔들림 범위도 그에 맞춰 커져야 함.
