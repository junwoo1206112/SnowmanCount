## Context

현재 SwerveMovement(좌우 이동 + 자동 전진)와 CameraFollow(카메라 추적)는 구현 완료. 하지만 게임의 핵심인 병력 관리 시스템이 없어 실제 게임플레이가 불가능한 상태. LevelLoader는 스켈레톤 상태이고, GateController/CrowdController는 아직 구현되지 않음.

## Goals / Non-Goals

**Goals:**
- 눈사람 병력 생성 및 실시간 병력 수 추적 (CrowdController)
- 게이트 통과 시 +-x÷ 연산 처리 (GateController)
- 오브젝트 풀링으로 병력 관리 최적화 (ObjectPooler)
- 병력 수 UI 실시간 표시 (UICounter)
- 엑셀 데이터 기반 레벨 자동 배치 (LevelLoader 완성)

**Non-Goals:**
- 적군 전투 시스템 (Phase 2)
- 장애물 시스템 (Phase 2)
- 사운드/이펙트 (Phase 4)

## Decisions

1. **CrowdController 중심 설계**: CrowdController가 병력 수를 관리하고, GateController가 통과 감지 후 CrowdController에 연산 요청. 단방향 의존성 유지.
2. **오브젝트 풀링 도입**: Instantiate/Destroy 대신 Queue 기반 Pool. 병력 수가 많을 수록 성능 차이가 큼.
3. **UI는 별도 MonoBehaviour**: UICounter가 CrowdController의 병력 수를 매 프레임 읽어 Text에 반영. Event-driven 대신 polling (간단하고 예측 가능).
4. **LevelLoader가 게임 시작 시 엑셀 데이터 로드**: NpoiLevelDataProvider를 통해 LevelData를 로드하고, 각 row를 순회하며 게이트/적/장애물을 배치.

## Risks / Trade-offs

- **[성능] 병력 100+ 명일 때 Lerp 연산 부하** → ObjectPooler로 활성화된 병력만 연산, Lerp 업데이트는 1~2프레임 간격으로 분산
- **[엑셀 파싱] NPOI DLL 의존성 문제** → Plugins/NPOI/에 수동 배치 완료, NuGetForUnity 재설치 금지
- **[게임 밸런스] 게이트 배치에 따라 게임 난이도 급변** → 엑셀 데이터 수정으로 대응, 코드 변경 불필요
