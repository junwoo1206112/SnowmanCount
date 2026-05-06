## 1. BossController 확장

- [x] 1.1 BossController.Setup()에 minionCount 파라미터 추가
- [x] 1.2 보스 페이즈 시스템 구현 (Phase 1/2/3, HP 임계값 기반)
- [x] 1.3 보스 링 미니언 생성 및 회전 로직 구현 (BossMinionController 신규)
- [x] 1.4 미니언 돌진(Charge) 패턴 구현 (follower 충돌 → 제거, 원위치 복귀)
- [x] 1.5 페이즈 전환 로직 구현 (미니언 재생성, 속도/개수 변경)
- [x] 1.6 보스 HP UI 업데이트 (TextMesh + UI HP bar 연동)
- [x] 1.7 보스 배치 시 WorldMover 제외 (정지 상태)

## 2. LevelLoader 확장

- [x] 2.1 LevelLoader.SpawnObject()에 "boss" 케이스 추가
- [x] 2.2 Boss 오브젝트 생성 및 BossController.Setup(hp, minionCount) 호출
- [x] 2.3 보스 프리팹 생성 (없으면 Primitive Capsule로 폴백)

## 5. 보스 등장 버그 수정

- [x] 5.1 보스에 WorldMover 추가
- [x] 5.2 OnTriggerEnter에서 자동 Activate()
- [x] 5.3 xlsx InvalidFormatException 수정 (jar META-INF)

## 3. Excel 데이터 업데이트

- [x] 3.1 Level1 시트에 Boss 행 추가 (560, HP=30, minions=6)
- [x] 3.2 Level2 시트에 Boss 행 추가 (550, HP=50, minions=8)

## 4. 클리어 조건 연동

- [x] 4.1 EnemyAllClearDetector가 보스 등록/해제 처리 확인
- [x] 4.2 보스 처치 시 OnBossDefeated() → LoadNextLevelDelayed() 흐름 확인
