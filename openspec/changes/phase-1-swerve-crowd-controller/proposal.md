## Why

Count Masters 스타일 러너 게임의 핵심 게임플레이를 구현해야 함. SwerveMovement와 CameraFollow는 이미 작성되었지만, 눈사람 무리 관리(CrowdController)와 게이트 연산(GateController)이 없어 실제 게임이 돌아가지 않는 상태.

## What Changes

- **CrowdController** — 눈사람 병력 생성, 병력 수 추적, 게이트 통과 시 연산 처리
- **GateController** — 게이트 통과 감지, +-x÷ 연산 로직
- **ObjectPooler** — 눈사람 오브젝트 풀링으로 최적화
- **UICounter** — 현재 병력 수 UI 표시
- **엑셀 데이터 연동** — NpoiLevelDataProvider로 레벨 데이터 로드 및 오브젝트 자동 배치

## Capabilities

### New Capabilities
- `crowd-control`: 눈사람 병력의 생성, 추적, 소멸 관리
- `gate-math`: 게이트 통과 시 병력 증감 연산 (+ - x ÷)
- `object-pooling`: 눈사람 프리팹 풀링으로 성능 최적화
- `ui-counter`: 병력 수 실시간 UI 표시

### Modified Capabilities
- (없음 — 첫 Phase)

## Impact

- `Assets/Scripts/Gameplay/` — CrowdController, GateController, ObjectPooler 추가
- `Assets/Scripts/UI/` — UICounter 추가
- `Assets/Scripts/Data/` — 기존 NpoiLevelDataProvider 활용
- `Assets/Prefabs/` — 눈사람 프리팹, 게이트 프리팹 준비 필요
