# AGENTS.md - SnowmanCount (MANDATORY - ALL RULES MUST BE FOLLOWED)

**이 문서의 모든 규칙은 선택이 아닌 필수입니다. 코드를 작성하거나 게임을 개발할 때 반드시 따라야 합니다.**

## ⚠️ 개발 워크플로우 (필수)

### 1. OpenSpec 워크플로우 (모든 기능 추가/변경에 필수)
```
/opsx:propose "기능명"   → proposal.md, design.md, specs, tasks.md 자동 생성
/opsx:apply             → tasks.md 순서대로 구현
/opsx:explore           → 아이디어 검토만 (코드 작성 금지)
/opsx:archive           → 완료 후 보관
```

**예외**: 단순 버그 수정(1-2줄)만 바로 수정 가능. 구조적 변경은 반드시 propose → apply.

### 2. Unity CLI (에디터 제어에 필수)
```bash
unity-cli editor play --wait    # 플레이 모드 진입
unity-cli editor stop           # 종료
unity-cli exec "return ..."     # C# 코드 실행 (가장 중요)
unity-cli console --type error  # 에러 로그 확인
unity-cli test --mode PlayMode  # 테스트 실행
unity-cli reserialize           # 에셋 수정 후 리시리얼라이즈
```

### 3. 스킬 로드 (필수)
작업 시작 시 다음 스킬을 순서대로 로드할 것:
```
skill({ name: "rules-mandatory" })
skill({ name: "unity-cli" })
skill({ name: "openspec-workflow" })
```

## 프로젝트 개요

Unity 6 (6000.3.11f1) 기반 3D 러너 게임. Count Masters 스타일의 병력 키우기 게임. URP(17.3.0), New Input System(1.19.0), NPOI(2.8.0).

## 빌드 및 실행

- **메인 씬**: `Assets/Scenes/GameScene.unity`
- **에디터 플레이**: Unity 에디터 Play 버튼
- **VS Code 디버깅**: `.vscode/launch.json` → "Attach to Unity"
- **테스트**: Unity 에디터 → Window → General → Test Runner
  - 단일 테스트: Test Runner에서 선택 후 Run Selected
  - CLI: `unity-cli test` / `unity-cli test --mode PlayMode`

## 프로젝트 구조

```
Assets/
├── Plugins/NPOI/           # NPOI v2.8.0 + 의존성 DLL (수동 관리)
├── Scripts/
│   ├── Core/               # GameManager (Singleton, DI)
│   ├── Data/               # ILevelDataProvider, NpoiLevelDataProvider, Models
│   ├── Data/Models/        # LevelData, LevelRow
│   ├── Gameplay/           # SwerveMovement, CameraFollow, LevelLoader
│   ├── UI/                 # (준비 중)
│   └── Managers/           # (준비 중)
├── Scenes/                 # GameScene.unity
├── StreamingAssets/Levels/ # 레벨 데이터 (.xlsx)
├── .opencode/skills/       # 스킬 정의 (rules-mandatory, unity-cli, openspec-workflow)
└── openspec/               # OpenSpec 변경사항
```

## 네임스페이스 (필수)

| 폴더 | 네임스페이스 |
|------|-------------|
| `Scripts/Core/` | `SnowmanCount.Core` |
| `Scripts/Data/` | `SnowmanCount.Data` |
| `Scripts/Data/Models/` | `SnowmanCount.Data.Models` |
| `Scripts/Gameplay/` | `SnowmanCount.Gameplay` |
| `Scripts/UI/` | `SnowmanCount.UI` |
| `Scripts/Managers/` | `SnowmanCount.Core` |

## 코드 스타일 (필수)

### Using 문 순서
```
System → UnityEngine → NPOI → 프로젝트 네임스페이스
```
각 그룹은 빈 줄로 구분.

### 중괄호 및 포맷팅
- **Allman 스타일** (여는 중괄호 새 줄)
- 들여쓰기 **4칸** (탭 금지)
- 메서드 사이 빈 줄
- 파일당 클래스 하나, 파일명 = 클래스명

### 명명 규칙
| 요소 | 규칙 | 예시 |
|------|------|------|
| 클래스 | PascalCase | `SwerveMovement` |
| 인터페이스 | IPascalCase | `ILevelDataProvider` |
| private 필드 | camelCase (밑줄 없음) | `swerveAmount` |
| SerializeField | camelCase | `forwardSpeed` |
| 프로퍼티 | PascalCase | `Instance` |
| 메서드 | PascalCase | `LoadLevel()` |
| 매개변수 | camelCase | `levelNumber` |
| 구조체/열거형 | PascalCase | `LevelRow`, `CellType` |

### 어셈블리 정의
- `Data.asmdef` — 참조 없음 (NPOI는 Plugins DLL이 자동 로드)
- `Core.asmdef` — Data 참조
- `Gameplay.asmdef` — Data + Core + `Unity.InputSystem` 참조
- `UI.asmdef` — 참조 없음
- `Managers.asmdef` — Data 참조, rootNamespace=`SnowmanCount.Core`

### DI 패턴 (필수 - GameManager 기반 수동 DI)
```csharp
// GameManager가 DI 컨테이너
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public ILevelDataProvider LevelDataProvider { get; private set; }

    private void RegisterServices()
    {
        LevelDataProvider = new NpoiLevelDataProvider();
    }
}

// 소비는 인터페이스 주입
dataProvider = GameManager.Instance.LevelDataProvider;
```

### NPOI 데이터 엑세스
- 엑셀 위치: `StreamingAssets/Levels/Level_{D2}.xlsx`
- 열 구조: Distance | ObjectType(Gate/Enemy/Obstacle/Finish) | Value(연산자/타입) | SubValue(수치)
- NPOI DLL은 `Assets/Plugins/NPOI/` 수동 관리 (Caterpillars 프로젝트에서 복사)
- NuGetForUnity로 재설치 금지

### MonoBehaviour 패턴
```csharp
[Header("Movement Settings")]
[SerializeField] private float forwardSpeed = 5f;
```

### 싱글턴 패턴
```csharp
public static GameManager Instance { get; private set; }
private void Awake()
{
    if (Instance != null && Instance != this) { Destroy(gameObject); return; }
    Instance = this;
    DontDestroyOnLoad(gameObject);
}
```

### 디버그 로깅
```csharp
Debug.Log($"[ClassName] message");
Debug.LogWarning($"[ClassName] warning");
Debug.LogError($"[ClassName] error");
```

### 에러 처리
- Debug.LogError + early return
- null provider는 기본값 폴백
- NPOI 파일 없으면 null 반환

### C# 언어 기능
- 표현식 본문: `bool IsReady => dataProvider != null;`
- 문자열 보간: `$"..."`
- Null 조건부: `sheet?.GetRow(i)`
- 대상 형식 new(): `new Vector3(0f, 0f, row.distance)`
- `nameof()` 사용
- `[Serializable]` 모든 데이터 모델
- C# 9.0, netstandard2.1

## ⚠️ 금지 규칙 (절대 위반 금지)
- **Mirror 사용 금지** — 싱글 플레이어 게임
- **`.meta` 파일 수동 수정 금지** — Unity가 관리
- **레거시 Input Manager 금지** — New Input System만 사용
- **GameObject.Find() 금지** — 싱글턴/참조/FindFirstObjectByType 사용
- **네트워크 동기화에 Update() 금지** — SyncVar/ClientRpc 사용 금지 (Singleton)
- **NuGetForUnity로 NPOI 재설치 금지** — Caterpillars 프로젝트에서 DLL 복사

## OpenSpec 상세

### 지원 도구
| 도구 | Skills 경로 | Commands 경로 |
|------|------------|---------------|
| Claude Code | `.claude/skills/openspec-*/SKILL.md` | `.claude/commands/opsx/<id>.md` |
| Cursor | `.cursor/skills/openspec-*/SKILL.md` | `.cursor/commands/opsx-<id>.md` |
| GitHub Copilot | `.github/skills/openspec-*/SKILL.md` | `.github/prompts/opsx-<id>.prompt.md` |
| OpenCode | `.opencode/skills/openspec-*/SKILL.md` | `.opencode/commands/opsx-<id>.md` |

### 전체 Tool ID
`amazon-q`, `antigravity`, `auggie`, `bob`, `claude`, `cline`, `codex`, `codebuddy`, `continue`, `costrict`, `crush`, `cursor`, `factory`, `forgecode`, `gemini`, `github-copilot`, `iflow`, `junie`, `kilocode`, `kiro`, `opencode`, `pi`, `qoder`, `qwen`, `roocode`, `trae`, `windsurf`

### 설정
```bash
openspec init --tools opencode
openspec init --tools all
openspec init --profile core
```

## 진행 상황 (필독 — 작업 시작 시 반드시 읽을 것)

현재 진행 상태는 `progress.md`를 참고하세요. 이 파일에는 완료된 Phase, 남은 작업, 알려진 버그, 핵심 아키텍처 결정사항이 정리되어 있습니다.

**중요**: 작업을 시작하기 전에 반드시 `progress.md`를 읽고 현재 상태를 파악한 후 이어서 작업하세요.
```
