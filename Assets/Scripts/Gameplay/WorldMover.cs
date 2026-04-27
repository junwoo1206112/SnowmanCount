using UnityEngine;
using SnowmanCount.Core;

namespace SnowmanCount.Gameplay
{
    public class WorldMover : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float destroyZ = -20f;

        private void Update()
        {
            transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);

            if (transform.position.z < destroyZ)
            {
                Destroy(gameObject);
            }
        }
    }
}
