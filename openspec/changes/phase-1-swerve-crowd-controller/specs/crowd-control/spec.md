# Crowd Control

## 요구사항
1. CrowdController는 게임 시작 시 N명의 눈사람 병력을 생성한다
2. 모든 병력은 PlayerPivot 주변 일정 반경 내에서 Lerp 이동하며 따라다닌다
3. 병력 수가 변경되면 즉시 UI에 반영된다
4. 병력이 0 이하가 되면 GameOver 상태로 전환된다
5. 각 병력은 서로 겹치지 않고 자연스러운 대형을 유지한다
