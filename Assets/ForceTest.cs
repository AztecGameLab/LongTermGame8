using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ForceTest : MonoBehaviour
{

    [SerializeField] private Rigidbody rigid;

    [SerializeField] private int distance;
    [SerializeField] private int land_vertical_y;
    [SerializeField] private float launch_angle;
    private float vertical_difference;
    
    // Start is called before the first frame update
    void Start()
    {
        //rigid.velocity = new Vector3(20.76686525f, 20.76686525f, 0);
        //rigid.velocity = new Vector3(15.90924353f, 27.5556181f, 0);
        //rigid.velocity = new Vector3(26.95391122f, 15.5618479f, 0);
        vertical_difference = rigid.transform.position.y - land_vertical_y;
        rigid.velocity = new Vector3((float)(CalculateVelocity()*Math.Cos(launch_angle)), (float)(CalculateVelocity()*Math.Sin(launch_angle)), 0);
    }

    private float CalculateVelocity() {

        double numerator = Math.Sqrt(472.0380705f) * (distance+1);

        double denominator = Math.Sqrt(96.2361f) * Math.Sqrt(
            Math.Cos(launch_angle) * (
                (distance+1) * Math.Sin(launch_angle) +
                vertical_difference * Math.Cos(launch_angle)
            )
        );
        return (float)(numerator / denominator);
    }

// Update is called once per frame
    void Update()
    {
        
    }
}
