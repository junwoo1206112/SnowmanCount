using UnityEngine;

namespace SnowmanCount.Gameplay
{
    public class FollowerComponent : MonoBehaviour
    {
        public float targetAngle;
        public float targetRadius;
        public bool isFalling;

        private void OnEnable()
        {
            isFalling = false;
            
            // 물리 효과를 초기화하기 위해 Rigidbody가 있다면 속도를 0으로 만듭니다.
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.useGravity = false;
                rb.isKinematic = true;
            }
        }
    }
}
