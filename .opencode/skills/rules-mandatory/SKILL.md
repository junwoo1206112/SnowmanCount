---
name: rules-mandatory
description: MANDATORY rules - MUST follow these rules for ALL code changes. Unity 6, URP, Input System, NPOI, DI, code style
license: MIT
compatibility: opencode
metadata:
  priority: highest
---

## ⚠️ 이 스킬은 절대 무시하면 안 됨 (MANDATORY)

### 프로젝트 기술 스택
- Unity 6000.3.11f1, URP 17.3.0, C# 9.0, netstandard2.1
- New Input System 1.19.0, NPOI 2.8.0
- NuGetForUnity (NPOI 의존성), OpenSpec

### 네임스페이스 규칙
| 폴더 | 네임스페이스 |
|------|-------------|
| Scripts/Core/ | SnowmanCount.Core |
| Scripts/Data/ | SnowmanCount.Data |
| Scripts/Data/Models/ | SnowmanCount.Data.Models |
| Scripts/Gameplay/ | SnowmanCount.Gameplay |
| Scripts/UI/ | SnowmanCount.UI |
| Scripts/Managers/ | SnowmanCount.Core |

### 코드 스타일
- Allman 중괄호 (여는 중괄호 새 줄)
- 들여쓰기 4칸 (탭 금지)
- 클래스 PascalCase, 인터페이스 IPascalCase
- private 필드 camelCase (밑줄 없음)
- SerializeField camelCase, 프로퍼티 PascalCase
- 파일명 = 클래스명, 파일당 클래스 하나

### DI 패턴 (필수)
게임 내 DI는 GameManager 기반 수동 DI:
```csharp
// GameManager가 서비스 로케이터 역할
public static GameManager Instance { get; private set; }
public ILevelDataProvider LevelDataProvider { get; private set; }

// 소비
dataProvider = GameManager.Instance.LevelDataProvider;
```

### NPOI 엑셀 데이터
- `StreamingAssets/Levels/Level_{D2}.xlsx` 포맷
- ILevelDataProvider 인터페이스로 추상화
- NPOI DLL은 `Assets/Plugins/NPOI/`에 있음 (수동 관리)
- NuGetForUnity로 재설치 금지

### 중요 금지 사항
- Mirror 사용 금지 (싱글 플레이어)
- `.meta` 파일 수동 수정 금지
- 레거시 Input Manager 금지
- GameObject.Find() 금지
- 네트워크 동기화에 Update() 금지
