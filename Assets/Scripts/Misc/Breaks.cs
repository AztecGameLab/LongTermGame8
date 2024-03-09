using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breaks : MonoBehaviour
{
    //A boolean value to check if the object reached conditions necessary to break other objects.
    public bool canBreak = false;
    //A float value to compare the force exerted from the object on the moment of collision.
    //Set the force treshold to 100 can be altered later.
    public float forceThresh = 100;

    private void OnCollisionEnter(Collision collision) 
    {
        //Gets the impulse value on collision.
        Vector3 Imp = collision.impulse / Time.fixedDeltaTime;
        Debug.Log("Impulse = " + Imp.magnitude);

        //If the collided object has a tag breakale check if the amount of force on collision passes to break an object.
        if(collision.gameObject.tag == "breakable")
        {
            //Imp.magnitude is the float vlaue of force exerted by the colliding object.
            if(Imp.magnitude > forceThresh)
            {
                canBreak = true;
            }       
        }
    }
}
