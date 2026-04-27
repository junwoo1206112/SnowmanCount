## 1. ObjectPooler

- [x] 1.1 Create ObjectPooler 클래스 (Queue 기반, 초기 풀 크기 50)
- [x] 1.2 GetPooledObject / ReturnToPool 메서드 구현
- [x] 1.3 풀 부족 시 자동 확장 로직

## 2. CrowdController

- [x] 2.1 게임 시작 시 N명의 눈사람 생성 (ObjectPooler 통해)
- [x] 2.2 각 병력이 PlayerPivot 주변 Lerp 이동
- [x] 2.3 병력 수 변경 시 UI 업데이트 이벤트
- [x] 2.4 병력 0 이하 시 GameOver 처리

## 3. GateController

- [x] 3.1 게이트 트리거 감지 (OnTriggerEnter)
- [x] 3.2 Blue Gate: +, x 연산 구현
- [x] 3.3 Red Gate: -, ÷ 연산 구현
- [x] 3.4 연산 결과 CrowdController에 전달

## 4. UICounter

- [x] 4.1 Canvas 생성 및 TextMeshPro 텍스트 배치
- [x] 4.2 UICounter 스크립트로 병력 수 실시간 표시
- [x] 4.3 숫자 변경 시 pop-up 애니메이션

## 5. LevelLoader 완성

- [x] 5.1 NpoiLevelDataProvider로 엑셀 데이터 로드
- [x] 5.2 LevelRow 순회하며 게이트/적/장애물 GameObject 배치
- [x] 5.3 게임 시작 시 LoadLevel(1) 호출

## 6. 통합 테스트 (Unity 에디터 필요)

- [ ] 6.1 PlayerPivot + SwerveMovement + CameraFollow 연동 확인
- [ ] 6.2 게이트 통과 → 병력 증감 확인
- [ ] 6.3 병력 0 → GameOver 확인
