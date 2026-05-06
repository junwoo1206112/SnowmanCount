using UnityEngine;

namespace SnowmanCount.Gameplay
{
    public class EnemyMinion : MonoBehaviour
    {
        [Header("Swarm Settings")]
        [SerializeField] private float aggroRange = 30f;
        [SerializeField] private float swarmSpeed = 18f;
        [SerializeField] private float clashRange = 1.5f;

        [Header("Separation")]
        [SerializeField] private float separationRadius = 2f;
        [SerializeField] private float separationStrength = 5f;

        [Header("Target Noise")]
        [SerializeField] private float noiseRange = 2f;

        private bool hasCollided;
        private bool isAggroed;
        private CrowdController crowdController;
        private EnemyGroup parentGroup;
        private Collider minionCollider;

        public void Setup(EnemyGroup group)
        {
            parentGroup = group;
            crowdController = FindFirstObjectByType<CrowdController>();
            hasCollided = false;
            isAggroed = false;

            minionCollider = GetComponent<Collider>();
            if (minionCollider == null)
            {
                SphereCollider sphere = gameObject.AddComponent<SphereCollider>();
                sphere.radius = 1.5f;
                minionCollider = sphere;
            }
            minionCollider.isTrigger = true;
            minionCollider.enabled = true;
        }

        private void Update()
        {
            if (hasCollided || crowdController == null) return;

            if (!isAggroed)
            {
                float dist = Vector3.Distance(
                    transform.position, crowdController.transform.position);

                if (dist < aggroRange)
                {
                    isAggroed = true;
                    transform.SetParent(null);
                    if (minionCollider != null) minionCollider.enabled = true;
                }

                return;
            }

            GameObject targetFollower = crowdController.FindNearestAvailableFollower(
                transform.position);

            Vector3 targetPos;

            if (targetFollower != null)
            {
                targetPos = targetFollower.transform.position;
            }
            else
            {
                targetPos = crowdController.transform.position
                    + new Vector3(Random.Range(-noiseRange, noiseRange), 0f, 0f);
            }

            targetPos.y = transform.position.y;
            Vector3 moveDir = (targetPos - transform.position).normalized;

            Vector3 separation = Vector3.zero;
            Collider[] nearby = Physics.OverlapSphere(transform.position, separationRadius);

            foreach (var col in nearby)
            {
                if (col.gameObject == gameObject) continue;
                if (col.GetComponent<EnemyMinion>() == null) continue;

                Vector3 away = transform.position - col.transform.position;
                away.y = 0f;
                float dist = away.magnitude;

                if (dist < separationRadius && dist > 0.01f)
                {
                    separation += away.normalized * (1f - dist / separationRadius);
                }
            }

            Vector3 velocity = moveDir * swarmSpeed * Time.deltaTime
                + separation * separationStrength * Time.deltaTime;

            if (velocity.magnitude > swarmSpeed * Time.deltaTime * 2f)
            {
                velocity = velocity.normalized * swarmSpeed * Time.deltaTime * 2f;
            }

            transform.position += velocity;

            if (targetFollower != null)
            {
                float d = Vector3.Distance(transform.position, targetFollower.transform.position);
                if (d <= clashRange)
                {
                    hasCollided = true;
                    FollowerComponent fc = targetFollower.GetComponent<FollowerComponent>();
                    if (fc != null) fc.isDueling = true;
                    crowdController.RemoveSpecificFollower(targetFollower);
                    parentGroup?.OnMinionDefeated();
                    Destroy(gameObject);
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (hasCollided) return;
            if (crowdController == null) return;

            bool isFollower = other.GetComponent<FollowerComponent>() != null;
            bool isPlayer = other.CompareTag("Player");

            if (!isFollower && !isPlayer) return;

            hasCollided = true;

            if (isFollower)
            {
                FollowerComponent fc = other.GetComponent<FollowerComponent>();
                if (fc != null) fc.isDueling = true;
                crowdController.RemoveSpecificFollower(other.gameObject);
                parentGroup?.OnMinionDefeated();
                Destroy(gameObject);
            }
            else
            {
                crowdController.RemoveCrowd(1);
                DefeatEnemy();
            }
        }

        private void DefeatEnemy()
        {
            parentGroup?.OnMinionDefeated();
            Destroy(gameObject);
        }
    }
}
