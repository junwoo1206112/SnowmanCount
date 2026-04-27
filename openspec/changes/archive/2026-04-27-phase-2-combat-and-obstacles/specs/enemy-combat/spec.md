# Enemy Combat

## 요구사항
1. EnemyGroup은 BoxCollider.isTrigger = true 상태로 생성된다
2. 적군 충돌 감지는 `other.CompareTag("Player")`로 검증한다
3. 충돌 시 `crowdController.ApplyMathOperation("-", count)`를 호출한다
4. hasTriggered 플래그로 중복 충돌을 방지한다
5. 충돌 후 적군 GameObject는 Destroy 된다
