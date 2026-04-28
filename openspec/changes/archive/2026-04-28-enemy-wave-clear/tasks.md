## 1. Enemy Wave 시스템

- [x] 1.1 LevelLoader.SpawnEnemyWave(float positionZ, int enemyCount) 메서드 추가
- [x] 1.2 SpawnLevel() 끝에서 SpawnEnemyWave(maxDistance + 30f, currentLevel * 10) 호출
- [x] 1.3 EnemyGroup의 SetEnemyCount()로 enemyCount 설정

## 2. Enemy 전멸 감지 (AllClearDetector)

- [x] 2.1 EnemyAllClearDetector.cs 생성 (RegisterEnemy/UnregisterEnemy)
- [x] 2.2 EnemyGroup.OnDestroy에서 Detector.UnregisterEnemy() 호출
- [x] 2.3 모든 EnemyGroup 사라지면 GameManager.Instance.OnLevelCleared() 호출

## 3. 엑셀 데이터 정리

- [x] 3.1 ExcelLevelCreator 데이터에 finish 타입 없음 (처음부터 없었음)
- [ ] 3.2 Unity 에디터에서 Tools → Create Levels.xlsx 실행하여 Levels.xlsx 재생성

## 4. FinishLineController 제거

- [x] 4.1 LevelLoader.SpawnFinish() 메서드 제거
- [x] 4.2 SpawnObject의 "finish" 케이스 제거
- [x] 4.3 finishPrefab SerializeField 제거
- [x] 4.4 FinishLineController.cs 파일 삭제
