using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ltg8
{
    public class PlatformPulley : MonoBehaviour
    {
        //public GameObject hitPlatformRef;
        public GameObject otherPlatformRef;
        public PlatformPulley otherplatformPulleyRef;

        public float movementSpeed = 0.1f;
        public float dampen = 1f;

        public float weightOn = 0f;

        void OnCollisionEnter(Collision col)
        {
            //Use the Impluse as the force moving the platforms down and account for the weights already on each platform
            float scalar1 = (col.impulse.magnitude / dampen) + weightOn;
            float scalar2 = (col.impulse.magnitude / dampen) - otherplatformPulleyRef.weightOn;

            //Move both in respects to the impact and wieghts on each
            this.transform.position = Vector3.MoveTowards(this.transform.position, 
                this.transform.position + Vector3.down * scalar1, movementSpeed * Time.deltaTime);

            otherPlatformRef.transform.position = Vector3.MoveTowards(otherPlatformRef.transform.position, 
                otherPlatformRef.transform.position + Vector3.up * scalar2, movementSpeed * Time.deltaTime);

            //add to the current weight of this platform
            weightOn += col.rigidbody.mass;
        }

        void OnCollisionExit(Collision col)
        {
            //remove weight
            weightOn -= col.rigidbody.mass;
        }
    }
}
