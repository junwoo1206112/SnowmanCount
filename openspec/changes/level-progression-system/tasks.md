## 1. GameManager - 레벨 및 병력 유지 static 필드

- [x] 1.1 GameManager에 `currentLevel` (int, static, 기본 1) 추가
- [x] 1.2 GameManager에 `carryOverCrowdCount` (int, static, 기본 -1) 추가
- [x] 1.3 GameManager에 `LoadNextLevel()` 메서드 추가 (씬 리로드, currentLevel 증가, carryOverCrowdCount 설정)
- [x] 1.4 RetryGame()에서 carryOverCrowdCount = -1로 리셋

## 2. CrowdController - 저장된 병력 수 복원

- [x] 2.1 Start()에서 GameManager.carryOverCrowdCount 확인 로직 추가
- [x] 2.2 carryOverCrowdCount > 0이면 해당 수만큼 SpawnFollower 호출, -1이면 initialCount 사용
- [x] 2.3 carryOverCrowdCount가 사용된 후에는 -1로 리셋 (중복 복원 방지)

## 3. FinishLineController - 다음 레벨 전환

- [x] 3.1 FinishLineController에서 레벨 클리어 시 GameManager.LoadNextLevel() 호출 (기존 OnLevelCleared에서 처리)
- [x] 3.2 LoadNextLevel 전에 1.5초 딜레이 추가 (coroutine)

## 4. LevelLoader - UIProgressBar에 레벨 길이 전달

- [x] 4.1 LevelLoader에 `[SerializeField] private UIProgressBar progressBar` 필드 추가
- [x] 4.2 LoadLevel() 끝에서 `progressBar.SetLevelLength(maxDistance)` 호출

## 5. UIProgressBar 수정

- [x] 5.1 `levelLength` setter 메서드 `SetLevelLength(float length)` 추가 (이미 존재)
- [x] 5.2 `GameObject.FindWithTag("Player")` → `FindFirstObjectByType<SwerveMovement>()`로 변경
