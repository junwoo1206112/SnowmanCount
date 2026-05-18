using UnityEngine;

namespace SnowmanCount.Gameplay
{
    public class SpinnerController : MonoBehaviour
    {
        [SerializeField] private float degreesPerSecond = 90f;

        private void Update()
        {
            transform.Rotate(0f, 0f, degreesPerSecond * Time.deltaTime);
        }
    }
}
