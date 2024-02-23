using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ltg8
{
    public class PlatformPulley : MonoBehaviour
    {
        //public GameObject hitPlatformRef;
        public GameObject otherPlatformRef;

        public float movementSpeed = 1f;
        public float dampen = 1f;

        public float weightOn = 0f;

        void OnCollisionEnter(Collision col)
        {
            float scalar1 = (col.impulse.magnitude / dampen) + weightOn;//add weight calc here
            float scalar2 = (col.impulse.magnitude / dampen) - weightOn;//change do getting wieght on other

            this.transform.position = Vector3.MoveTowards(this.transform.position, 
                this.transform.position + Vector3.down * scalar1, movementSpeed * Time.deltaTime);

            otherPlatformRef.transform.position = Vector3.MoveTowards(otherPlatformRef.transform.position, 
                otherPlatformRef.transform.position + Vector3.up * scalar2, movementSpeed * Time.deltaTime);
        }

        void OnCollisionStay(Collision col)
        {
            //add weight
            //weightOn += col.rigidbody.mass;
            Debug.Log("Weight on: " + weightOn);
        }

        void OnCollisionExit(Collision col)
        {
            //remove weight
        }
    }
}
