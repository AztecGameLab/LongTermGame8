using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    //Append boulder gameObject to script to get Breaks.cs values.
    public GameObject boulder;
    //Call the broken version of the object.
    [SerializeField] private GameObject broken;

    private void Update() 
    {
        //calls the canBreak bool from the Breaks scprit.
        bool canBreak = boulder.GetComponent<Breaks>().canBreak;

        if(canBreak == true)
        {
            SetBroken();
        }
    }

    //Call the function when object can be broken.
    //Replace the current object with the borken version.
    public void SetBroken()
    {
        //spawn broken object on collision.
        Instantiate(broken, transform.position, Quaternion.identity);
        broken.SetActive(true);
        //sets the unbroken object to inactive.
        gameObject.SetActive(false);
    }
}
