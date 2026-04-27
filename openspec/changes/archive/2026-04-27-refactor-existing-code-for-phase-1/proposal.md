## Why

Phase 1 구현 과정에서 작성된 5개 스크립트(ObjectPooler, CrowdController, GateController, UICounter, LevelLoader)와 기존 3개 스크립트(SwerveMovement, CameraFollow, GroundScroller, GameManager)의 품질이 일관되지 않음. 모든 코드를 동일한 코드 스타일 규칙(Allman, 네임스페이스, 명명 규칙, DI 패턴, 에러 처리, 로깅)에 맞춰 정밀하게 정리해야 함. 또한 각 스크립트가 게임에서 어떻게 동작하는지 세부 로직까지 spec에 명시해야 함.

## What Changes

- **ObjectPooler.cs**: null 체크 강화, dispose 패턴, 풀 상태 로깅
- **CrowdController.cs**: 연산 정밀도 개선, 대형 알고리즘 최적화, 이벤트 방출 타이밍 조정
- **GateController.cs**: 중복 트리거 방지 강화, 태그 검증, 게이트 비활성화 시각 효과
- **UICounter.cs**: TextMeshPro 전환, DOTween 없는 순수 코루틴 애니메이션 개선
- **LevelLoader.cs**: 널 체크, 프리팹 부재 시 graceful fallback, 배치 위치 자동 계산
- **SwerveMovement.cs**: Input System 이벤트 기반 리팩토링 (Update polling → callback)
- **CameraFollow.cs**: 경계 제한, 충돌 시 부드러운 복귀
- **GroundScroller.cs**: 머티리얼 null 체크, 속도 가속
- **GameManager.cs**: 서비스 레지스트리 확장, CrowdController/UICounter 참조 추가

## Capabilities

### New Capabilities
- (없음 — 모두 기존 코드 리팩토링)

### Modified Capabilities
- `crowd-control`: 대형 알고리즘 개선, 연산 정밀도 향상
- `gate-math`: 트리거 검증 강화, 중복 방지 로직
- `object-pooling`: 안전성 강화, 상태 추적 로깅
- `ui-counter`: TextMeshPro 대응, 애니메이션 부드럽게
- (기존 Gameplay 스크립트 전반)

## Impact

- `Assets/Scripts/Core/GameManager.cs` — 서비스 레지스트리 확장
- `Assets/Scripts/Gameplay/` — 5개 파일 전면 리팩토링
- `Assets/Scripts/UI/UICounter.cs` — TextMeshPro 대응
