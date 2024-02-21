using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MoveScript : MonoBehaviour
{
    [SerializeField] private GameObject character;
    [SerializeField] private Rigidbody rigid;
    [SerializeField] private BoxCollider bCollider;
    private bool jump = false;

// Start is called before the first frame update
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            if (jump)
            {
                rigid.AddForce(transform.forward * 5);
            }
            else
            {
                rigid.AddForce(transform.forward);
            }
            
        }
        if (Input.GetKey(KeyCode.A))
        {
            rigid.AddTorque(0,-3,0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            rigid.AddTorque(0,3,0);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rigid.AddForce(new Vector3(0,400,0));
        }
    }

    void OnCollisionExit(Collision collisionInfo)
    {
        print("No longer in contact with " + collisionInfo.transform.name);
    }

    
}
