## Context

현재 승리 계단(LevelLoader.VictoryStaircaseRoutine)은 각 스텝당 하나의 Primitive Cube(flat platform)로만 구성. 0.2f 두께의 금색 판에 TextMesh 라벨만 붙어 있어 시각적 깊이가 부족함. Count Masters의 피라미드 계단은 각 스텝이 3D 구조체 + 사이드 장식 + 개별 병력 블록으로 구성되어 있어 훨씬 디테일함.

## Goals / Non-Goals

**Goals:**
- 각 스텝을 tread(밟는 면) + riser(앞면) 구조의 3D 계단으로 변경
- 계단 양옆에 사이드 레일/벽 추가
- 각 병력 착지 위치마다 개별 기둥/블록 생성
- 금속 + Emission Material 적용
- 꼭대기 왕관/트로피 장식
- StairPiece Prefab 도입

**Non-Goals:**
- 계단 애니메이션(HopToPosition) 동작 변경 — 기존 유지
- 계단 배율 로직 변경 — 기존 유지
- 새로운 게임플레이 메커니즘 추가
- 성능 최적화 (기존 수준 유지)

## Decisions

### 1. 스텝 구조: Tread + Riser 조합
- **결정:** 각 스텝을 2개의 Cube(tread + riser)로 구성
- **이유:** 단일 Cube로는 front face(계단 앞면) 표현 불가. Tread는 밟는 면, Riser는 앞면 vertical face로 분리하여 실제 계단 형태 구현
- **대안:** Mesh 결합 — 불필요한 복잡도, 단순 분리가 더 유지보수 용이

### 2. 사이드 레일
- **결정:** 각 스텝 양옆에 길쭉한 Cube 벽 추가 (stepW 기준 양쪽 끝)
- **이유:** 피라미드 계단의 입체감과 안정적인 시각적 프레임 제공
- **대안:** 별도 Prefab — 복잡도 대비 효과 미미, 코드 내 생성으로 충분

### 3. 개별 병력 블록
- **결정:** 각 병력이 착지하는 위치마다 작은 Cube 블록(0.25×0.12×0.25) 생성
- **이유:** Count Masters에서 각 캐릭터가 개별 타일 위에 서는 효과 재현
- **대안:** 기존 flat platform 유지 — 디테일 부족

### 4. Material
- **결정:** 기존 Gold Color + Metallic 유지, Emission 강화
- **이유:** URP Lit Material에서 `_Emission` 활성화만으로도 발광 효과 구현 가능. 별도 Material 에셋 파일을 만들어 재사용
- **대안:** Shader Graph 커스텀 — 오버엔지니어링

### 5. StairPiece Prefab
- **결정:** 개별 스텝 Piece를 Prefab화하지 않고 코드 내 생성 유지
- **이유:** 각 스텝 크기(cols/rows에 따라 동적)가 매번 달라 Prefab으로 고정 불가. 빌드 타임 게임오브젝트 생성이 더 적합
- **대안:** Prefab + Scale 조정 — 동적 크기 대응이 더 복잡해짐

### 6. 장식: 꼭대기 왕관
- **결정:** 최상단 스텝 위에 Cube + Cylinder 조합으로 단순한 왕관/트로피 형상 생성
- **이유:** 별도 3D 모델 없이 Primitive만으로 충분한 시각적 포인트 제공

## Risks / Trade-offs

- **[Performance]** 스텝당 1개 → tread+riser+side×2+블록들 = 약 5배 오브젝트 증가 — Max 16단 기준 80→400개, 무시할 수준
- **[Code Complexity]** VictoryStaircaseRoutine 길이 증가 — 별도 헬퍼 메서드 분리로 대응
- **[Design Risk]** 과도한 디테일이 모바일 최적화에 영향 — URP 배칭으로 처리 가능
