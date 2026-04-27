## Context

Phase 1에서 작성된 모든 스크립트는 기능적으로 동작하지만 코드 품질과 일관성이 부족함. 특히:
- `SwerveMovement`: Update()에서 매 프레임 Input System 폴링 → Input Action Callback 기반으로 변경 필요
- `GameManager`: 서비스 참조 누락 (CrowdController, UICounter)
- 모든 스크립트: 에러 처리, null 체크, 로깅 형식 불일치

## Goals / Non-Goals

**Goals:**
- 모든 스크립트에 일관된 Allman 스타일, 네임스페이스, 명명 규칙 적용
- GameManager에서 모든 핵심 서비스 참조 제공
- SwerveMovement를 Input Action Callback 기반으로 리팩토링
- 각 스크립트에 null 체크 + early return + Debug.LogError 패턴 적용
- GroundScroller에 속도 가속 기능 추가

**Non-Goals:**
- 새로운 기능 추가 (Phase 2에서)
- UI 레이아웃 변경

## Decisions

1. **GameManager를 중앙 DI 컨테이너로 확장**: CrowdController, UICounter, LevelLoader 참조 추가. 다른 스크립트는 GameManager.Instance로 접근.
2. **SwerveMovement: Input Action Callback + LateUpdate 조합**: `PlayerInput` 컴포넌트 사용. SwerveInputActionMap의 Move 액션을 callback으로 바인딩. 실제 위치 이동은 LateUpdate에서.
3. **UICounter: Text 대신 TextMeshProUGUI 사용**: URP 프로젝트이므로 TextMeshPro가 더 적합. 폴백으로 일반 Text도 지원.
4. **로깅 형식 통일**: 모든 Debug.Log에 `[$ClassName]` 접두사 사용. 에러는 `Debug.LogError`, 경고는 `Debug.LogWarning`.

## Risks / Trade-offs

- **[호환성] Input Action Callback 변경 시 PlayerInput 컴포넌트 필요** → 기존 SwerveMovement 사용자는 Input Action Asset 다시 바인딩 필요
- **[성능] TextMeshPro가 일반 Text보다 약간 무거움** → URP 프로젝트에선 기본 포함. 품질이 더 중요
