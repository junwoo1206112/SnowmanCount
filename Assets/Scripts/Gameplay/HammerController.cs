using UnityEngine;

namespace SnowmanCount.Gameplay
{
    public class HammerController : MonoBehaviour
    {
        [Header("Swing Settings")]
        [SerializeField] private float swingSpeed = 2f;
        [SerializeField] private float swingAngle = 45f;
        [SerializeField] private float startOffset;

        private void Start()
        {
            // 모든 망치가 동시에 움직이지 않도록 랜덤한 오프셋 부여
            startOffset = Random.Range(0f, Mathf.PI * 2f);
        }

        private void Update()
        {
            // 시계추 운동 (Pendulum movement)
            float angle = Mathf.Sin(Time.time * swingSpeed + startOffset) * swingAngle;
            transform.localRotation = Quaternion.Euler(0f, 0f, angle);
        }

        public void SetSettings(float speed, float angle)
        {
            swingSpeed = speed;
            swingAngle = angle;
        }
    }
}
