## 1. GameManager — DI 컨테이너 확장

- [x] 1.1 CrowdController, UICounter 참조 추가
- [x] 1.2 싱글턴 패턴 유지, OnDestroy 정리
- [x] 1.3 모든 서비스 RegisterServices()에서 등록

## 2. SwerveMovement — Input Action Callback 기반 리팩토링

- [x] 2.1 PlayerInput 컴포넌트와 Input Action Asset 연동
- [x] 2.2 OnMove(InputAction.CallbackContext) 콜백 구현
- [x] 2.3 LateUpdate에서 실제 위치 이동
- [x] 2.4 Mathf.Clamp 경계 제한 유지
- [x] 2.5 태그 로깅 추가

## 3. CameraFollow — 경계 및 부드러움 개선

- [x] 3.1 타겟 null 체크 early return
- [x] 3.2 Offset 유지, positionClamp로 위치 제한
- [x] 3.3 LateUpdate 사용 확인

## 4. GroundScroller — 속도 가속 및 안정성

- [x] 4.1 머티리얼 null 체크
- [x] 4.2 scrollSpeed 플레이어 속도에 비례하도록
- [x] 4.3 Awake에서 Renderer 캐싱

## 5. ObjectPooler — 안전성 강화

- [x] 5.1 null 체크 (prefab, 반환된 obj)
- [x] 5.2 풀 상태 로깅 (Debug.Log)
- [x] 5.3 ActiveCount/AvailableCount 프로퍼티

## 6. CrowdController — 연산 및 대형 정밀도

- [x] 6.1 ApplyMathOperation switch 패턴 유지
- [x] 6.2 대형 알고리즘 index 기반 원형 배치
- [x] 6.3 OnCrowdCountChanged 이벤트 방출
- [x] 6.4 OnCrowdDepleted → GameManager.Instance.UICounter.ShowGameOver()

## 7. GateController — 트리거 검증

- [x] 7.1 OnTriggerEnter 태그 "Player" 검증
- [x] 7.2 hasTriggered 플래그로 중복 방지
- [x] 7.3 SetGateData에서 연산자에 따라 색상 자동 결정
- [x] 7.4 통과 후 Collider + Renderer 비활성화

## 8. UICounter — TextMeshPro 전환

- [x] 8.1 TextMeshProUGUI 참조 추가 (일반 Text 폴백)
- [x] 8.2 Pop 애니메이션 코루틴 개선
- [x] 8.3 GameOver 텍스트 표시

## 9. LevelLoader — 안전한 배치

- [x] 9.1 게이트 프리팹 null 체크
- [x] 9.2 LoadLevel(1) Start()에서 자동 호출
- [x] 9.3 배치 위치 라인 단위 자동 계산

## 10. 모든 스크립트 코드 스타일 통일

- [x] 10.1 Allman 중괄호 확인
- [x] 10.2 네임스페이스 일치 (AGENTS.md 표 참조)
- [x] 10.3 Using 문 순서 (System → UnityEngine → NPOI → 프로젝트)
- [x] 10.4 private 필드 camelCase (밑줄 없음)
- [x] 10.5 Debug.Log $"[ClassName]" 형식 통일
