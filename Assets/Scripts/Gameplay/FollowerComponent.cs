using UnityEngine;

namespace SnowmanCount.Gameplay
{
    public class FollowerComponent : MonoBehaviour
    {
        public float targetAngle;
        public float targetRadius;
        public bool isFalling;
        public bool isDueling;

        private void OnEnable()
        {
            isFalling = false;
            isDueling = false;

            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
                rb.useGravity = false;
            }
        }
    }
}
