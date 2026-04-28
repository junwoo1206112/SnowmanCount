## Context

현재 Level Clear는 `GameManager.LoadNextLevelDelayed()` 코루틴으로 1.5초 후 자동으로 씬을 리로드한다. `ShowLevelClearUI()`가 호출되지만 LevelClearText만 표시하고 Time.timeScale=0f로 멈추지만, 1.5초 후 다시 1f로 바뀌고 씬이 리로드된다. 플레이어는 병력 수를 확인할 기회가 없고, 마지막 레벨에서는 아무것도 표시되지 않는다.

## Goals / Non-Goals

**Goals:**
- Level Clear 시 병력 수를 표시하는 전용 UI 화면
- Next 버튼으로만 다음 레벨로 진행 (자동 전환 제거)
- All Levels Clear 시 "All Levels Clear" 메시지 + 메인 메뉴/Retry
- 화면 상단에 현재 레벨 번호 표시 ("Level 1")

**Non-Goals:**
- 메인 메뉴 씬 (단순 UI로 대체)
- 별도의 레벨 선택 화면
- 저장/로드 시스템

## Decisions

1. **Level Clear UI는 Canvas의 기존 요소 활용**
   - `ShowLevelClearUI()`에서 LevelClearText 표시 + 병력 수 텍스트 + Next 버튼
   - Next 버튼 클릭 시 `GameManager.LoadNextLevel()` 호출 (자동 코루틴 제거)
   - 병력 수는 `carryOverCrowdCount`로 표시

2. **Level 번호는 Canvas에 LevelText 추가**
   - GameManager가 `OnSceneLoaded`에서 `GameObject.Find("LevelText")?.GetComponent<Text>().text = $"Level {currentLevel}"`로 설정

3. **All Levels Clear 처리**
   - `LevelLoader.Start()`에서 dataProvider.LoadLevel()이 null 반환 시 "All Levels Clear" UI 표시
   - LevelLoader 또는 GameManager가 Canvas의 AllClearText 활성화

4. **Level Clear UI의 Next 버튼**
   - Level Clear 화면에 Next 버튼 추가 (RetryButton과 별개)
   - Next 버튼: `Time.timeScale = 1f; currentLevel++; SceneManager.LoadScene(...)`
   - 기존 자동 코루틴은 제거

## Risks / Trade-offs

- Next 버튼을 누르기 전에 Time.timeScale = 0f이므로, 버튼 이벤트는 정상 동작 (Unity UI는 timeScale 영향 안 받음)
- 마지막 레벨의 엑셀 파일이 없으면 All Levels Clear 표시 → 이 시점에서 병력 수는 의미 없음
