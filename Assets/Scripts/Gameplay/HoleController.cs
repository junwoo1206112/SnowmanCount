using UnityEngine;
using System.Collections;

namespace SnowmanCount.Gameplay
{
    public class HoleController : MonoBehaviour
    {
        private CrowdController crowdController;

        private void Start()
        {
            crowdController = FindFirstObjectByType<CrowdController>();
        }

        private void OnTriggerEnter(Collider other)
        {
            FollowerComponent follower = other.GetComponent<FollowerComponent>();
            if (follower == null || follower.isFalling) return;

            if (crowdController == null)
            {
                crowdController = FindFirstObjectByType<CrowdController>();
            }

            if (crowdController != null)
            {
                StartCoroutine(FellInHoleRoutine(follower));
            }
        }

        private IEnumerator FellInHoleRoutine(FollowerComponent follower)
        {
            follower.isFalling = true;
            
            // 군집 리스트에서 먼저 제거하여 숫자에 즉시 반영
            crowdController.RemoveFromList(follower.gameObject);

            // 물리 효과 적용 (아래로 떨어짐)
            Rigidbody rb = follower.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
                
                // 살짝 아래로 힘을 주어 빨리 떨어지게 함
                rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
            }
            else
            {
                // Rigidbody가 없는 경우를 대비한 최소한의 처리
                follower.transform.Translate(Vector3.down * 2f);
            }

            // 0.7초 동안 떨어지는 연출 후 풀로 반환
            yield return new WaitForSeconds(0.7f);

            if (crowdController != null)
            {
                crowdController.ReturnFollowerToPool(follower.gameObject);
            }
        }
    }
}
