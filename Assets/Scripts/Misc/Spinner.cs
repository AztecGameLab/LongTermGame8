using UnityEngine;

namespace Ltg8.Misc
{
    public class Spinner : MonoBehaviour
    {
        public float spinSpeed;
        public Vector3 axis;

        private void Update()
        {
            transform.Rotate(axis, spinSpeed * Time.deltaTime);
        }
    }
}