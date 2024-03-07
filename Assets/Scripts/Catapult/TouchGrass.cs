using UnityEngine;

namespace Catapult
{
    public class TouchGrass : MonoBehaviour
    {
        private void OnTriggerEnter(Collider projectileCollider)
        {
            var projectileRb = gameObject.GetComponent<Rigidbody>();
            var projectileSc = gameObject.GetComponent<SphereCollider>();
            var zeroOut = new Vector3(0, 0, 0);
            if (projectileSc.enabled)
            {
                if (gameObject.TryGetComponent(out CharacterController _))
                {
                    projectileRb.useGravity = false;
                    gameObject.GetComponent<BoxCollider>().enabled = false;
                    gameObject.GetComponent<CharacterController>().enabled = true;
                }
                projectileSc.enabled = false;
                projectileRb.velocity = zeroOut;
                projectileRb.angularVelocity = zeroOut;
                gameObject.transform.eulerAngles = zeroOut;
                Destroy(gameObject.GetComponent<SphereCollider>());
                Destroy(gameObject.GetComponent<TouchGrass>());
            }
        }
    }
}
