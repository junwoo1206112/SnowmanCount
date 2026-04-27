# Gate Math

## 요구사항
1. GateController는 플레이어가 게이트를 통과할 때를 감지한다 (트리거)
2. 파란 게이트(Blue): + 또는 x 연산 수행 → 병력 증가
3. 빨간 게이트(Red): - 또는 ÷ 연산 수행 → 병력 감소
4. 연산 결과는 CrowdController의 병력 수에 즉시 반영된다
5. 연산 결과는 상단 UI에 pop-up 애니메이션과 함께 표시된다
