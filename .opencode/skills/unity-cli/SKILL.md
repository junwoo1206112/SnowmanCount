---
name: unity-cli
description: Control Unity Editor from CLI - play/stop, exec C#, console logs, run tests, profiler, custom tools
license: MIT
compatibility: opencode
metadata:
  source: https://github.com/youngwoocho02/unity-cli
---

## 설치 확인

```bash
unity-cli status
```

Unity 에디터가 실행 중이어야 함. 연결 안 되면:
1. Unity 에디터 켜기
2. Edit → Preferences → General → Interaction Mode → No Throttling
3. 다시 `unity-cli status` 확인

## 필수 명령어

### 에디터 제어
```bash
unity-cli editor play --wait    # 플레이 모드 진입 + 대기
unity-cli editor stop           # 플레이 모드 종료
unity-cli editor pause          # 일시정지
unity-cli editor refresh        # 에셋 리프레시
```

### C# 코드 실행 (가장 중요)
```bash
unity-cli exec "return Application.dataPath;"
unity-cli exec "return EditorSceneManager.GetActiveScene().name;"
```

파이프로 복잡한 코드도 가능:
```bash
echo 'Debug.Log("hello"); return null;' | unity-cli exec
```

### 콘솔 로그
```bash
unity-cli console                    # 에러/경고 로그 (기본)
unity-cli console --type error       # 에러만
unity-cli console --clear            # 콘솔 클리어
```

### 테스트 실행
```bash
unity-cli test                       # EditMode 테스트 (기본)
unity-cli test --mode PlayMode       # PlayMode 테스트
unity-cli test --filter MyTest       # 특정 테스트 필터
```

### 메뉴 실행
```bash
unity-cli menu "File/Save Project"
unity-cli menu "Assets/Refresh"
```

### 프로파일러
```bash
unity-cli profiler enable
unity-cli profiler hierarchy --depth 3
unity-cli profiler disable
```

### 에셋 리시리얼라이즈
```bash
unity-cli reserialize                    # 전체 프로젝트
unity-cli reserialize Assets/Prefabs/Player.prefab  # 특정 파일
```

## 주의사항
- `--wait` 플래그 사용 시 완료될 때까지 블로킹됨
- `unity-cli exec`는 모든 로드된 어셈블리 접근 가능
- 복잡한 코드는 stdin 파이프 권장 (쉘 이스케이프 문제 회피)
