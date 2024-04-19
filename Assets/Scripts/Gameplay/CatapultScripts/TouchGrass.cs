using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchGrass : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (gameObject.TryGetComponent(out CharacterController characterController))
        {
            if (gameObject.GetComponent<SphereCollider>().enabled)
            {
                gameObject.GetComponent<BoxCollider>().enabled = false;
                gameObject.GetComponent<SphereCollider>().enabled = false;
                gameObject.GetComponent<Rigidbody>().useGravity = false;
                gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                gameObject.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);
                gameObject.transform.eulerAngles = new Vector3(0, 0, 0);
                gameObject.GetComponent<CharacterController>().enabled = true;
            }
        }
        else
        {
            if (gameObject.GetComponent<SphereCollider>().enabled)
            {
                gameObject.GetComponent<SphereCollider>().enabled = false;
                gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                gameObject.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);
                gameObject.transform.eulerAngles = new Vector3(0, 0, 0);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
