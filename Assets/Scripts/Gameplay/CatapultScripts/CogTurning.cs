using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Catapult;

public class CogTurning : MonoBehaviour
{

    [SerializeField] private GameObject cogLaunch;
    [SerializeField] private GameObject cogRotate;
    [SerializeField] private GameObject cogLever;

    private bool turnLaunchCog;
    private bool turnRotateCog;
    private bool catapult_activating;

    private float frames;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void setTime(float time)
    {
        frames = (time / Time.deltaTime);
    }
    
    public void turnCog(string action)
    {
        switch (action)
        {
            case "Launch":
                if (turnLaunchCog)
                {
                    return;
                }
                turnLaunchCog = !turnLaunchCog;
                catapult_activating = !catapult_activating;
                break;
            case "Rotate":
                turnRotateCog = !turnRotateCog;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (turnLaunchCog)
        {
            if (catapult_activating)
            {
                if (cogLever.transform.eulerAngles.x < 60)
                {
                    cogLever.transform.Rotate(0, (60/frames), 0);
                    cogLaunch.transform.Rotate(0, 1f, 0);
                }
                else
                {
                    turnLaunchCog = !turnLaunchCog;
                }
            }
            else
            {
                if (cogLever.transform.eulerAngles.x > 1)
                {
                    cogLever.transform.Rotate(0, -(60/frames), 0);
                    cogLaunch.transform.Rotate(0, -5f, 0);
                }
                else
                {
                    turnLaunchCog = !turnLaunchCog;
                }
            }
        }
        else if (turnRotateCog)
        {
            cogRotate.transform.Rotate(0, 1, 0);
        }
    }
}