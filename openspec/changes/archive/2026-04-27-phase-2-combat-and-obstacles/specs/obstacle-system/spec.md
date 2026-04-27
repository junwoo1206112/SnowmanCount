# Obstacle System

## 요구사항
1. ObstacleController는 BoxCollider.isTrigger = true 상태로 생성된다
2. OnTriggerStay로 구역 내 지속 데미지를 처리한다
3. damageInterval(기본 1초) 마다 1 데미지를 준다
4. 첫 데미지에 딜레이가 없어야 한다 (lastDamageTime = -999f)
5. `other.CompareTag("Player")`로 대상을 검증한다
