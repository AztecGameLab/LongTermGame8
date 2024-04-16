using UnityEngine;
using Audio;

namespace Catapult
{
    public class TouchGrass : MonoBehaviour
    {
        // When the projectile hits the ground (via the Sphere Collider Trigger)
        private void OnTriggerEnter(Collider projectileCollider)
        {
            var projectileRb = gameObject.GetComponent<Rigidbody>();
            var projectileSc = gameObject.GetComponent<SphereCollider>(); 
            var zeroOut = new Vector3(0, 0, 0); // Used to set Vectors to 0,0,0
            if (!projectileSc.enabled) return; // If the Sphere Collider isn't enabled, return
            if (gameObject.TryGetComponent(out CharacterController _)) // If the projectile is player
            {
                // Disable projectile physics, re-enable player physics
                projectileRb.useGravity = false;
                gameObject.GetComponent<BoxCollider>().enabled = false;
                gameObject.GetComponent<CharacterController>().enabled = true;
            }
            else
            {
                gameObject.transform.Find("Trigger").gameObject.SetActive(true);
            }

            // Disable the Sphere Collider so the trigger disables and the script is not re-called
            projectileSc.enabled = false;
            // Disable projectile motion
            projectileRb.velocity = zeroOut;
            projectileRb.angularVelocity = zeroOut;
            gameObject.transform.eulerAngles = zeroOut;
            // Play the "Thud" audio when the projectile lands
            gameObject.GetComponent<AudioController>().PlayAudio();
            Invoke(nameof(DestroyComponents),1f);
        }
        
        // Destroys all the components that are specific to a projectile
        private void DestroyComponents(){
            Destroy(gameObject.GetComponent<AudioSource>());
            Destroy(gameObject.GetComponent<SphereCollider>());
            Destroy(gameObject.GetComponent<TouchGrass>());
            Destroy(gameObject.GetComponent<AudioController>());
        }
    }
}
