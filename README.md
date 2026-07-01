# Snowman Crowd Runner

Unity 6 기반의 캐주얼 러너/카운트 게임 프로토타입입니다. 플레이어가 좌우로 이동하며 게이트를 통과하고, 군중 수를 증감시키며 장애물, 적 그룹, 보스전을 돌파하는 구조를 구현했습니다.

## Project Overview

이 프로젝트는 Count Masters류 하이퍼캐주얼 러너의 핵심 루프를 Unity/C#로 재구성한 작업입니다. 단순 이동만 있는 샘플이 아니라, 군중 수 계산, 팔로워 배치, 오브젝트 풀링, 데이터 기반 레벨 배치, 전투/보스 처리까지 연결된 플레이 흐름을 목표로 했습니다.

## Main Features

- Swerve 입력 기반 좌우 이동
- 게이트 연산 처리: `+`, `-`, `x`, `/`
- 군중 팔로워 생성, 제거, 재배치
- 오브젝트 풀링 기반 팔로워 재사용
- 장애물, 구멍, 톱, 해머, 스피너 등 충돌 요소
- 적 그룹과 팔로워 1:1 소모 전투
- 보스 HP, 페이즈, 피격 연출, 클리어 처리
- Excel/NPOI 기반 레벨 데이터 로드 및 저장
- ScriptableObject 레벨 데이터 변환용 에디터 도구
- 게임 상태 관리: Ready, Play, LevelClear, GameOver

## Tech Stack

- Unity `6000.3.11f1`
- C#
- Unity Input System
- NPOI Excel parser
- ScriptableObject
- Unity Editor tooling

## Key Implementation Points

### Crowd System

`CrowdController`가 현재 군중 수와 실제 화면에 표시되는 팔로워 수를 분리해서 관리합니다. 총 인원은 게임 점수/전투 판단에 사용하고, 화면에는 `maxFollowers` 제한을 두어 성능 부담을 줄였습니다.

### Gate Operation

`GateController`는 플레이어가 통과한 게이트의 연산자를 `CrowdController.ApplyMathOperation()`으로 전달합니다. 덧셈, 뺄셈, 곱셈, 나눗셈 연산을 한 흐름에서 처리하도록 구성했습니다.

### Data Driven Level

`NpoiLevelDataProvider`는 `Assets/StreamingAssets/Levels/Levels.xlsx`에서 레벨 행을 읽어 거리, 오브젝트 타입, 값, 보조 값을 로드합니다. 기획자가 Excel 형태로 레벨을 수정할 수 있는 구조를 의도했습니다.

### Boss and Combat Loop

팔로워가 보스와 충돌하면 보스 HP가 감소하고 팔로워는 군중에서 제거됩니다. 보스 HP가 일정 비율 이하로 떨어지면 페이즈가 변경되며, 처치 시 레벨 클리어와 보상 계산으로 이어집니다.

## Folder Structure

```text
Assets/
  Scenes/
    GameScene.unity
  Scripts/
    Core/        # GameManager, GameStateManager
    Gameplay/    # Crowd, Gate, Boss, Enemy, Obstacle, Movement
    Data/        # LevelData, Excel provider, ScriptableObject provider
    UI/          # UI counter/progress components
  StreamingAssets/
    Levels/
      Levels.xlsx
  Editor/        # Excel conversion and build/check tools
```

## How to Run

1. Unity Hub에서 Unity `6000.3.11f1` 이상으로 프로젝트를 엽니다.
2. `Assets/Scenes/GameScene.unity`를 엽니다.
3. Play 버튼을 누릅니다.
4. 마우스 또는 터치 입력으로 좌우 이동하며 게이트와 장애물을 통과합니다.

## Portfolio Notes

이 프로젝트는 메인 대표작보다는 캐주얼 게임 구현 경험을 보여주는 보조 포트폴리오로 적합합니다. 특히 게임플레이 루프, 데이터 기반 레벨 구성, 오브젝트 풀링, Unity 컴포넌트 분리 경험을 설명할 때 활용할 수 있습니다.

## Next Improvements

- 실제 플레이 영상 또는 GIF 추가
- 주요 레벨 데이터 예시 표 정리
- Unity Test Runner 기반 핵심 로직 테스트 추가
- 모바일 해상도 UI 검증
- 빌드 파일 또는 WebGL 데모 배포
