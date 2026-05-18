using UnityEngine;

namespace SnowmanCount.Gameplay
{
    public class SawController : MonoBehaviour
    {
        [SerializeField] private float rotationsPerSecond = 2f;

        private void Update()
        {
            transform.Rotate(0f, 360f * rotationsPerSecond * Time.deltaTime, 0f);
        }
    }
}
