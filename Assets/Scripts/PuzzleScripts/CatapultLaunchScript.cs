using System;
using UnityEngine;

public class CatapultLaunchScript : MonoBehaviour
{

    [SerializeField] private GameObject projectile;
    [SerializeField] private Rigidbody projectile_rb;
    private GameObject _catapult;
    private GameObject catapult_spoon;

    [SerializeField] private int distance;
    [SerializeField] private float land_vertical_y;
    private float launch_angle;
    
    private float vertical_difference;
    private bool child_found;
    
    void Start()
    {
        _catapult = transform.gameObject;
        catapult_spoon = FindFirstChild(_catapult, "Spoon").gameObject;
        launch_angle = catapult_spoon.transform.rotation.eulerAngles.x * Mathf.Deg2Rad;
        vertical_difference = projectile_rb.transform.position.y - land_vertical_y;
    }

    private void SetProjectile(GameObject projectile)
    {
        this.projectile = projectile;
        projectile_rb = projectile.GetComponent<Rigidbody>();
    }
    
    public void LaunchProjectile()
    {
        float velocity_h = (float)(CalculateVelocity() * Math.Cos(launch_angle));
        float velocity_v = (float)(CalculateVelocity() * Math.Sin(launch_angle));
        float velocity_x = velocity_h * _catapult.transform.forward.x;
        float velocity_z = velocity_h * _catapult.transform.forward.z;
        projectile_rb.useGravity = true;
        projectile_rb.velocity = new Vector3(velocity_x, velocity_v, velocity_z);
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

    private Transform FindFirstChild(GameObject parent, string child_name)
    {
        if (child_found) { child_found = false; return null; }
        Transform child = parent.transform.Find(child_name);
        if (child)
        { child_found = true; return child; }
        if(parent.transform.childCount > 0)
        {
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                child = FindFirstChild(parent.transform.GetChild(i).gameObject, child_name);
                if (child)
                {
                    return child;
                }
            }
        }
        return null;
    }

    void Update()
    {
    }
}
