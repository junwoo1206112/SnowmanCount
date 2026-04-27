# Snowman Count — 기능 명세서 (Specification)

> Count Masters 스타일 3D 러너 게임.  
> 눈사람 병력을 이끌고 게이트를 통과하며 병력을 키우고 적을 물리치는 게임.

---

## 목차

1. [게임 개요](#1-게임-개요)
2. [Phase 1 — 코어 시스템](#2-phase-1--코어-시스템)
3. [Phase 2 — 전투 및 장애물](#3-phase-2--전투-및-장애물)
4. [Phase 3 — 데이터 및 UI](#4-phase-3--데이터-및-ui)
5. [Phase 4 — 폴리싱](#5-phase-4--폴리싱)
6. [기술 스택](#6-기술-스택)
7. [프로젝트 구조](#7-프로젝트-구조)

---

## 1. 게임 개요

### 1.1 장르
- 3D 하이퍼 캐주얼 러너
- 병력 키우기 (Crowd Growth)
- 자동 진행 + 좌우 조작

### 1.2 플레이어 경험
1. 플레이어는 눈사람 무리를 이끌고 자동으로 전진한다
2. 마우스 드래그로 좌우로 이동한다
3. 파란 게이트(+ , x)를 통과하면 병력이 증가한다
4. 빨간 게이트(- , ÷)를 통과하면 병력이 감소한다
5. 병력이 0이 되면 게임 오버
6. 레벨 끝까지 도달하면 클리어

### 1.3 핵심 용어
| 용어 | 설명 |
|------|------|
| Snow Power | 현재 병력 수 (숫자) |
| PlayerPivot | 플레이어 중심점 (움직이지 않음) |
| Follower | 눈사람 병력 개체 (PlayerPivot 주변을 맴돔) |
| Gate | 병력 증감을 일으키는 관문 |

---

## 2. Phase 1 — 코어 시스템

### 2.1 SwerveMovement (좌우 이동)

**담당 파일**: `Assets/Scripts/Gameplay/SwerveMovement.cs`

**역할**: 플레이어의 좌우 이동을 담당한다.

**동작 방식**:
```
[마우스 드래그] → Input System 감지 → swerveAmount 계산
    → Update()에서 transform.position.x += swerveAmount
    → Mathf.Clamp로 xBound(-4 ~ +4) 제한
```

**핵심 코드 구조**:
```csharp
// Input Action Callback으로 입력 받음
OnMoveStarted: isDragging = true
OnMovePerformed: swerveAmount = input.x * swerveSpeed
OnMoveCanceled: swerveAmount = 0, isDragging = false

// 매 프레임
ApplySwerveOnly():
    pos.x += swerveAmount * Time.deltaTime
    pos.x = Clamp(pos.x, -xBound, xBound)
```

**중요**: 플레이어는 Z축(앞뒤)으로 이동하지 않는다.  
월드가 플레이어 쪽으로 다가오는 방식을 사용한다.

### 2.2 WorldMover (월드 스크롤)

**담당 파일**: `Assets/Scripts/Gameplay/WorldMover.cs`

**역할**: 게이트, 적, 장애물이 플레이어 쪽으로 다가오게 만든다.

**동작 방식**:
```
매 프레임:
    transform.Translate(Vector3.back * moveSpeed * Time.deltaTime)
    // = z축으로 계속 감소
    
    if (z < destroyZ): Destroy(this.gameObject)
    // 화면 밖으로 나가면 삭제
```

**왜 필요한가**:
- 플레이어가 움직이지 않으므로 월드 오브젝트가 대신 움직여야 함
- Count Masters, Subway Surfers 등 모든 러너 게임이 사용하는 방식
- 엑셀 Distance 값을 그대로 사용 가능 (게이트 위치가 변하지 않음)

### 2.3 GroundScroller (바닥 스크롤)

**담당 파일**: `Assets/Scripts/Gameplay/GroundScroller.cs`

**역할**: 바닥 텍스처가 움직여서 "내가 앞으로 가고 있다"는 착시를 준다.

**동작 방식**:
```
Awake(): Renderer에서 Material 참조 가져오기
Update():
    offset.y -= scrollSpeed * Time.deltaTime  
    // -= : 텍스처가 아래로 흐름 = 위로 올라가는 느낌
    
    material.SetTextureOffset("_BaseMap", offset)
```

**텍스처 설정**:
- Unity 에디터에서 Ground Material 선택
- `_BaseMap` Tiling: X=50, Y=200 (Plane 크기에 맞게 반복)
- `scrollSpeed`: 5 (플레이어 전진 속도와 동일하게)

### 2.4 ObjectPooler (오브젝트 풀)

**담당 파일**: `Assets/Scripts/Gameplay/ObjectPooler.cs`

**역할**: 눈사람 프리팹을 재사용해서 메모리를 최적화한다.

**동작 원리**:
```
게임 시작 시:
    눈사람 50개를 미리 생성 (SetActive(false) 상태)
    
병력이 필요할 때:
    GetPooledObject() → 풀에서 1개 꺼내서 SetActive(true)
    
병력이 사라질 때:
    ReturnToPool(obj) → SetActive(false), 다시 풀에 넣음
    
풀이 부족하면:
    autoExpand=true → 자동으로 새로 생성
```

**핵심 변수**:
| 변수 | 기본값 | 설명 |
|------|--------|------|
| prefab | (필수) | 풀링할 프리팹 |
| initialPoolSize | 50 | 초기 생성 개수 |
| autoExpand | true | 풀 부족 시 자동 확장 |

### 2.5 CrowdController (병력 관리)

**담당 파일**: `Assets/Scripts/Gameplay/CrowdController.cs`

**역할**: 눈사람 병력의 생성, 이동, 연산, 소멸을 총괄한다.

**병력 생성**:
```
Start():
    initialCount(5) 만큼 SpawnFollower()
    
SpawnFollower():
    objectPooler.GetPooledObject()
    → position = PlayerPivot 주변 랜덤 위치
    → activeCrowd 리스트에 추가
```

**병력 이동**:
```
매 프레임 (Update()):
    for each follower in activeCrowd:
        targetPos = PlayerPivot.position + GetOffsetForIndex(index)
        follower.position = Lerp(follower.position, targetPos, lerpSpeed)
        
GetOffsetForIndex(index):
    angle = (360 / activeCrowd.Count) * index
    radius = min(followRadius, 1 + count * 0.15)
    → 원형으로 배치 (Count Masters 스타일)
```

**병력 연산**:
```csharp
ApplyMathOperation(operatorType, value):
    switch operatorType:
        "+":  AddCrowd(value)     → value만큼 추가 생성
        "-":  RemoveCrowd(value)  → value만큼 제거
        "x":  MultiplyCrowd(n)    → 현재 수 × n
        "÷":  DivideCrowd(n)      → 현재 수 ÷ n
    → OnCrowdCountChanged 이벤트 발생
```

**게임 오버 조건**:
```
RemoveCrowd() 실행 후 CurrentCount <= 0
    → OnCrowdDepleted 이벤트 발생
    → GameManager.OnCrowdDepleted() 호출
```

### 2.6 GateController (게이트)

**담당 파일**: `Assets/Scripts/Gameplay/GateController.cs`

**역할**: 플레이어가 게이트를 통과하면 병력 연산을 실행한다.

**게이트 색상**:
```
SetGateData(operator, value):
    if operator == "+" or "x":
        color = 파란색 (Blue)  → 병력 증가
    if operator == "-" or "÷":
        color = 빨간색 (Red)   → 병력 감소
```

**트리거 감지**:
```
OnTriggerEnter(collider):
    if hasTriggered: return  (중복 방지)
    if tag != "Player": return (플레이어만)
    
    hasTriggered = true
    crowdController.ApplyMathOperation(operator, value)
    DisableGate()  // Collider + Renderer 비활성화
```

---

## 3. Phase 2 — 전투 및 장애물

> (준비 중 — 아직 구현되지 않음)

### 3.1 EnemyGroup (적군)
- 불꽃 정령 무리
- 아군 눈사람과 1:1 충돌 → 서로 소멸
- 병력 수가 많은 쪽이 승리

### 3.2 ObstacleController (장애물)
- 회전 톱날: 접촉 시 지속 데미지
- 가시 함정: 접촉 시 즉시 데미지
- 녹는 구간: 구역 통과 중 지속 데미지

---

## 4. Phase 3 — 데이터 및 UI

> (Phase 3 구현됨)

### 4.1 NPOI 엑셀 데이터

**사용 라이브러리**: NPOI 2.8.0  
**DLL 위치**: `Assets/Plugins/NPOI/` (수동 관리)

**엑셀 파일 위치**: `Assets/StreamingAssets/Levels/Level_{number:D2}.xlsx`

**엑셀 컬럼 구조**:
| 컬럼명 | 타입 | 설명 | 예시 |
|--------|------|------|------|
| Distance | float | Z축 배치 거리 | 10.0 |
| ObjectType | string | Gate / Enemy / Obstacle / Finish | Gate |
| Value | string | 연산자(+-x÷) 또는 타입 | + |
| SubValue | int | 수치 또는 병력 수 | 5 |

**레벨 데이터 예시 (Level_01.xlsx)**:
```
Distance | ObjectType | Value | SubValue
10       | Gate       | +     | 5
25       | Gate       | x     | 2
40       | Gate       | -     | 3
55       | Enemy      | Flame | 3
70       | Gate       | +     | 10
85       | Obstacle   | Saw   | 1
100      | Gate       | ÷     | 2
120      | Finish     |       | 0
```

### 4.2 DI 패턴 (Dependency Injection)

**적용 방식**: 수동 DI (GameManager 기반 서비스 로케이터)

**계층 구조**:
```
ILevelDataProvider (인터페이스)
    └── NpoiLevelDataProvider (NPOI로 엑셀 읽기)
    
GameManager.Instance.LevelDataProvider
    → 모든 스크립트가 여기서 주입받음
```

**코드 예시**:
```csharp
// GameManager에서 등록
private void RegisterServices()
{
    LevelDataProvider = new NpoiLevelDataProvider();
}

// LevelLoader에서 사용
dataProvider = GameManager.Instance.LevelDataProvider;
LevelData data = dataProvider.LoadLevel(1);
```

### 4.3 LevelLoader (레벨 로더)

**담당 파일**: `Assets/Scripts/Gameplay/LevelLoader.cs`

**역할**: 엑셀 데이터를 읽어서 게임 오브젝트를 자동 배치한다.

**동작 흐름**:
```
Awake():
    dataProvider = GameManager.Instance.LevelDataProvider

Start():
    LoadLevel(1)  // 게임 시작 시 레벨 1 로드

LoadLevel(n):
    data = dataProvider.LoadLevel(n)
    for each row in data.rows:
        SpawnObject(row)

SpawnObject(row):
    distance → Z축 위치
    switch ObjectType:
        "Gate":   SpawnGate()    → GateController + WorldMover
        "Enemy":  SpawnEnemy()   → (Phase 2)
        "Obstacle": SpawnObstacle() → (Phase 2)
        "Finish": SpawnFinish()  → (Phase 2)
```

### 4.4 UICounter (UI 카운터)

**담당 파일**: `Assets/Scripts/UI/UICounter.cs`

**역할**: 현재 병력 수를 화면 상단에 표시한다.

**표시 방식**:
```
상단 중앙에 Text:
    "Snow Power: 5"  (기본 prefix + 숫자)
    
숫자가 변경될 때:
    PlayPopRoutine():
        scale 1.0 → 1.3 (확대)
        scale 1.3 → 1.0 (원래대로)
        0.2초 동안 부드럽게 전환
        
병력이 0이 되면:
    ShowGameOver():
        "Game Over" 텍스트 표시
```

---

## 5. Phase 4 — 폴리싱

> (준비 중 — 아직 구현되지 않음)

### 5.1 예정된 작업
- 눈사람 모델 3D 에셋 교체
- 파티클 이펙트 (게이트 통과, 병력 충돌)
- 배경 음악 및 SFX
- 레벨 전환 화면
- 빌드 테스트

---

## 6. 기술 스택

| 항목 | 버전 | 설명 |
|------|------|------|
| Unity | 6000.3.11f1 | 게임 엔진 |
| URP | 17.3.0 | 렌더링 파이프라인 |
| Input System | 1.19.0 | 마우스/터치 입력 |
| NPOI | 2.8.0 | 엑셀 파일 읽기 |
| C# | 9.0 / netstandard2.1 | 언어 버전 |
| Test Framework | 1.6.0 | 유닛 테스트 |

### NuGetForUnity 주의사항
- NPOI는 `Assets/Plugins/NPOI/`에 **수동 배치**
- Caterpillars 프로젝트에서 DLL 복사해서 사용
- **NuGetForUnity로 재설치 금지** (의존성 DLL 충돌)

---

## 7. 프로젝트 구조

```
Assets/
├── Plugins/NPOI/             # NPOI DLL + 모든 의존성 DLL (수동 관리)
├── Scripts/
│   ├── Core/
│   │   └── GameManager.cs    # 싱글턴, DI 컨테이너
│   ├── Data/
│   │   ├── ILevelDataProvider.cs     # 데이터 제공 인터페이스
│   │   ├── NpoiLevelDataProvider.cs  # NPOI 구현체
│   │   ├── LevelDataService.cs       # 서비스 레지스트리
│   │   └── Models/
│   │       └── LevelData.cs          # LevelRow, LevelData 모델
│   ├── Gameplay/
│   │   ├── SwerveMovement.cs    # 좌우 이동
│   │   ├── CrowdController.cs   # 병력 관리
│   │   ├── GateController.cs    # 게이트 연산
│   │   ├── ObjectPooler.cs      # 오브젝트 풀링
│   │   ├── LevelLoader.cs       # 레벨 로드/배치
│   │   ├── WorldMover.cs        # 월드 스크롤
│   │   └── GroundScroller.cs    # 바닥 텍스처 스크롤
│   └── UI/
│       └── UICounter.cs         # 병력 수 UI
├── Scenes/
│   └── GameScene.unity          # 메인 게임 씬
├── StreamingAssets/Levels/
│   └── Level_01.xlsx            # 레벨 1 데이터
├── Editor/
│   └── ExcelLevelCreator.cs     # 엑셀 생성 도구
└── docs/
    └── specification.md         # 이 파일
```

### 어셈블리 참조 관계

```
Data.asmdef (참조 없음 — NPOI는 Plugins DLL 자동 로드)
    ↑
Core.asmdef (Data 참조)
    ↑
Gameplay.asmdef (Data + Core + Unity.InputSystem 참조)
UI.asmdef (참조 없음, 독립)
Managers.asmdef (Data 참조, rootNamespace=Core)
```

---

## 부록: 코드 스타일 규칙

### 네임스페이스
| 폴더 | 네임스페이스 |
|------|-------------|
| Scripts/Core/ | SnowmanCount.Core |
| Scripts/Data/ | SnowmanCount.Data |
| Scripts/Data/Models/ | SnowmanCount.Data.Models |
| Scripts/Gameplay/ | SnowmanCount.Gameplay |
| Scripts/UI/ | SnowmanCount.UI |
| Scripts/Managers/ | SnowmanCount.Core |

### 포맷팅
- Allman 중괄호 (여는 중괄호 새 줄)
- 들여쓰기 4칸 공백 (탭 금지)
- 파일당 클래스 하나, 파일명 = 클래스명

### 명명 규칙
- 클래스/메서드/프로퍼티: PascalCase
- 인터페이스: IPascalCase
- private 필드: camelCase (밑줄 없음)
- SerializeField: camelCase
- 매개변수: camelCase

### 로깅 규칙
```csharp
Debug.Log($"[ClassName] message");
Debug.LogWarning($"[ClassName] warning");
Debug.LogError($"[ClassName] error");
```

### 금지 사항
- Mirror 사용 금지 (싱글 플레이어)
- `.meta` 파일 수동 수정 금지
- 레거시 Input Manager 금지
- GameObject.Find() 금지
- NuGetForUnity로 NPOI 재설치 금지

---

*최종 업데이트: 2026-04-27*  
*버전: v0.1 (Phase 1 완료)*
