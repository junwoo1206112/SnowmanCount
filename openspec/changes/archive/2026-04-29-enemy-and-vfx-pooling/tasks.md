# Tasks: Enemy and VFX Pooling

- [x] 1. `ObjectPooler.cs` 범용화 리팩터링: `SetupFollowerComponents` 제거 및 인스턴스 관리 로직 분리
- [x] 2. `EnemyMinion.cs` 수정: 소멸 시 `ReturnToPool`을 사용하도록 변경
- [x] 3. `LevelLoader.cs` 수정: `enemyPooler` 직렬화 필드 추가 및 스폰 로직 교체
- [x] 4. 유니티 에디터 설정:
  - `AllyPooler` 오브젝트의 프리팹 설정 확인
  - 신규 `EnemyPooler` 오브젝트 생성 및 적군 미니언 프리팹 할당
- [x] 5. 대규모 전투 시 성능 저하가 없는지 테스트
