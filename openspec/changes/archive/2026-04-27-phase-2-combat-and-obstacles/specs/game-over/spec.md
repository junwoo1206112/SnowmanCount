# Game Over

## 요구사항
1. CrowdController 병력이 0이 되면 OnCrowdDepleted 이벤트가 발생한다
2. GameManager가 이 이벤트를 구독하여 처리한다
3. GameManager.SendMessage("ShowGameOver")로 UICounter를 호출한다
4. GameOver 시 Time.timeScale = 0f로 게임이 일시정지된다
5. PlayerPivot의 Tag가 "Player"로 설정되어 있어야 모든 충돌이 동작한다
