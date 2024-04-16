using UnityEngine;

namespace Misc
{
    public class ForceApplier : MonoBehaviour
    {
        public float strength;
        public Rigidbody target;

        public void Apply()
        {
            target.AddForce(transform.forward * strength);
        }
    }
}
