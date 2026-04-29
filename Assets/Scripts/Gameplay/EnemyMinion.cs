using UnityEngine;

namespace SnowmanCount.Gameplay
{
    public class EnemyMinion : MonoBehaviour
    {
        private bool hasCollided;
        private CrowdController crowdController;
        private EnemyGroup parentGroup;
        private ObjectPooler pool;

        public void Setup(EnemyGroup group, ObjectPooler pooler = null)
        {
            parentGroup = group;
            pool = pooler;
            crowdController = FindFirstObjectByType<CrowdController>();
            
            hasCollided = false;

            // 충돌체 보장
            Collider col = GetComponent<Collider>();
            if (col == null)
            {
                col = gameObject.AddComponent<SphereCollider>();
            }
            col.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (hasCollided) return;

            // 아군 유닛(Follower) 또는 플레이어(리더)와 충돌 확인
            bool isFollower = other.GetComponent<FollowerComponent>() != null;
            bool isPlayer = other.CompareTag("Player");

            if (isFollower || isPlayer)
            {
                hasCollided = true;

                if (crowdController != null)
                {
                    if (isFollower)
                    {
                        // 해당 팔로워만 제거
                        crowdController.RemoveSpecificFollower(other.gameObject);
                    }
                    else if (isPlayer)
                    {
                        // 리더와 부딪히면 1명 감소 처리
                        crowdController.RemoveCrowd(1);
                    }
                }

                // 적 미니언 소멸 연출
                if (parentGroup != null)
                {
                    parentGroup.OnMinionDefeated();
                }

                if (pool != null)
                {
                    pool.ReturnToPool(this.gameObject);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
