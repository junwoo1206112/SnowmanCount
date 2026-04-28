## 1. 게이트 연산자 기호 표시

- [x] 1.1 GateController.SpawnGate에서 큐브 위에 TextMeshPro 생성
- [x] 1.2 연산자에 따라 "+5", "x2", "-3", "÷2" 텍스트 표시
- [x] 1.3 카메라를 향하도록 LookAt 설정

## 2. 게임 상태 관리

- [x] 2.1 GameStateManager: Ready → Play → GameOver 상태 전환
- [x] 2.2 Ready 상태에서 클릭 시 Play로 전환
- [x] 2.3 GameOver 시 Retry 버튼 표시

## 3. Ready UI

- [x] 3.1 Ready 상태 "Tap to Start" 텍스트 (Canvas에서 수동 배치 필요)
- [x] 3.2 Play 전환 시 텍스트 숨김

## 4. 레벨 클리어

- [x] 4.1 FinishLineController: Finish 충돌 감지
- [x] 4.2 LevelClear 상태 추가 (GameState.LevelClear)
- [x] 4.3 LevelClear UI 표시 (LevelClearText + Retry)

## 5. Retry 기능

- [x] 5.1 Retry 버튼 클릭 시 SceneManager.LoadScene(현재 씬) 리로드
- [x] 5.2 Time.timeScale = 1f로 복원
