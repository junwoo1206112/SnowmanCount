## Context

현재 레벨의 끝은 엑셀 데이터의 `Finish` 타입 오브젝트로 감지된다. 그런데 엑셀 데이터에 Finish 행이 없고, LevelLoader의 `SpawnFinish()`는 있지만 호출되지 않는다. Count Masters 스타일에서는 레벨 끝에서 대량의 적과 전투하여 클리어한다.

LevelLoader가 엑셀 데이터를 다 배치한 후, 그 뒤에 Enemy 대량 무리를 자동으로 추가 배치하고, 모든 EnemyGroup이 Destroy되면 Level Clear로 전환한다.

## Goals / Non-Goals

**Goals:**
- 각 레벨 마지막에 Enemy 대량 무리 배치 (Finish 라인 대신)
- 모든 Enemy가 처치되면 Level Clear
- 레벨별 Enemy 수: Level1=10, Level2=20, Level3=30...
- Enemy 배치 위치: 마지막 오브젝트 거리 + 30m

**Non-Goals:**
- 개별 Enemy와의 1:1 전투 (단순 집단 충돌 → 카운트 감소로 유지)
- 보스전 특별 이펙트/애니메이션 (추후 추가 가능)

## Decisions

1. **LevelLoader가 마지막 오브젝트 거리 계산 후 Enemy Wave 배치**
   - `SpawnLevel()` 끝에서 `maxDistance + 30f` 위치에 EnemyGroup 생성
   - EnemyGroup의 enemyCount를 `currentLevel * 10`으로 설정
   - EnemyGroup은 기존 `EnemyGroup.cs` 재활용

2. **EnemyGroup이 모두 사라지면 Level Clear**
   - `EnemyAllClearDetector`라는 빈 GameObject를 씬에 배치
   - EnemyGroup이 파괴될 때 Detector에게 알림
   - 모든 EnemyGroup이 파괴되면 `GameManager.Instance.OnLevelCleared()` 호출

3. **FinishLineController 제거**
   - 더 이상 Finish 타입 사용 안 함
   - 엑셀 데이터에서도 Finish 행 제거

4. **SpawnEnemyWave() 메서드**
   - LevelLoader에 새 메서드 추가
   - maxDistance + 30 위치에 enemyCount만큼 자식 minion 생성
   - 각 minion은 WorldMover로 플레이어 쪽으로 이동

## Risks / Trade-offs

- EnemyGroup이 많으면 성능 영향 가능 (ObjectPooler와 무관하게 Instantiate)
- 플레이어가 Enemy Wave까지 도달하지 못하고 GameOver 되면 레벨 재시작
- Level 1에서 적 10마리는 너무 쉬울 수 있음 → 값 조정 필요
