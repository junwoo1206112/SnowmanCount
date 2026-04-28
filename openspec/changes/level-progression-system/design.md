## Context

현재 게임은 Level 1만 로드되고 클리어 후 아무 일도 일어나지 않는다. GameManager는 DontDestroyOnLoad지만, Retry는 씬을 리로드할 뿐 다음 레벨로 넘어가지 않는다. CrowdController는 항상 initialCount(5)로 시작한다.

Count Masters 방식: 레벨 클리어 → 모인 병력을 그대로 유지 → 다음 레벨 시작.

## Goals / Non-Goals

**Goals:**
- 레벨 클리어 시 다음 레벨(Level_02.xlsx, Level_03.xlsx ...) 로드
- 다음 레벨로 넘어갈 때 현재 병력 수 유지
- Retry는 현재 레벨을 병력 초기화(initialCount)로 다시 시작
- UIProgressBar가 실제 레벨 길이 반영
- 준비된 엑셀 파일이 더 이상 없으면 "All Levels Clear" 메시지

**Non-Goals:**
- 레벨 선택 화면(Level Select) — 단순 순차 진행
- 저장/로드 (PlayerPrefs) — 추후 추가 가능

## Decisions

1. **GameManager가 레벨 번호와 병력 수를 static 필드로 관리**
   - DontDestroyOnLoad라서 씬 리로드 시에도 유지됨
   - `currentLevel`, `carryOverCrowdCount` static int 필드
   - Retry 시 `carryOverCrowdCount = -1`로 초기화 (새 게임 신호)

2. **CrowdController가 Start()에서 carryOverCrowdCount 확인**
   - 값이 -1이면 initialCount 사용 (새 게임/Retry)
   - 값이 >0이면 해당 수만큼 병력 생성

3. **FinishLineController가 레벨 클리어 시 GameManager에 다음 레벨 요청**
   - `GameManager.Instance.LoadNextLevel()` 호출
   - GameManager가 `SceneManager.LoadScene()`으로 씬 리로드

4. **LevelLoader가 UIProgressBar에 maxDistance 전달**
   - LevelLoader에 `UIProgressBar progressBar` 직렬화 필드 추가
   - LoadLevel 끝에서 `progressBar.SetLevelLength(maxDistance)` 호출

5. **엑셀 파일 없으면 "All Levels Clear"**
   - `LevelDataProvider.LoadLevel(levelNumber)`가 null 반환 시 처리

## Risks / Trade-offs

- 씬 리로드 시 모든 월드 오브젝트가 새로 생성되므로 carry-over는 병력 수만 유지됨 (위치는 재설정)
- 엑셀 파일 naming convention: `Level_01.xlsx`, `Level_02.xlsx` ... 유지 필요
