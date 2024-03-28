using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    //Append boulder gameObject to script to get Breaks.cs values.
    public GameObject boulder;
    //Call the broken version of the object.
    [SerializeField] private GameObject broken;

    //If conditions are valid to break the object the Breaks script changes the value to true.
    //This bool makes sure that individual objects are broken.
    public bool Break = false;

    private void Update() 
    {       
        if(Break == true)
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
