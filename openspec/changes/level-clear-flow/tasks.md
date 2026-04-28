## 1. Level Clear - 병력 수 표시 + 자동 전환

- [x] 1.1 ShowLevelClearUI(crowdCount)에 병력 수 텍스트 표시 ("Snow Power: {count}")
- [x] 1.2 Level Clear 시 1.5초 후 자동 전환 (LoadNextLevelDelayed 복원)
- [x] 1.3 Level Clear 화면에 Retry 버튼 표시 (level=1로 리셋)
- [x] 1.4 GameOver Retry는 같은 레벨 유지 (currentLevel 리셋 안 함)

## 2. All Levels Clear

- [x] 2.1 LoadLevel null 반환 시 LevelClearText에 "All Levels Clear!" 표시
- [x] 2.2 Retry 버튼 표시 (level=1로 리셋)

## 3. Level 번호 표시

- [x] 3.1 OnSceneLoaded에서 "LevelText" 업데이트 ("Level {currentLevel}")
- [x] 3.2 Canvas에 "LevelText" 없으면 무시

## 4. UI 배치 가이드

- [ ] 4.1 Canvas에 LevelText 추가 (상단 중앙, "Level 1")
- [ ] 4.2 기존 RetryButton 재활용 (GameOver/LevelClear/AllClear 공용)
- [ ] 4.3 LevelClearText 비활성화 상태로 시작
