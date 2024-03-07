using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EsterlinearMovement : MonoBehaviour
{
    // Nice idea, but if there's more than this amount hiding spots, you need to go back in the code and make a new var and then add it to the list
    // public Vector3 hidingSpot1;
    // public Vector3 hidingSpot2;
    // public Vector3 hidingSpot3;
    // public Vector3 hidingSpot4;
    // public Vector3 hidingSpot5;
    // public Vector3 hidingSpotFinal;
    // -----------------------------
    
    // I like the idea, but to get the hiding spots, while you do need their positions which are vectors, you need to first have the gameobject itself
    // List<Vector3> hidingSpots = new List<Vector3>();
    
    
    public GameObject Ester;
    public GameObject Sigmund;
    
    // Hiding spot list container
     public GameObject[] hidingspots;

     void Start() {
        // As mentioned above, if you do this approach, everytime there is more hiding spots, we'll have to go back in, you know what I mean?
        // hidingSpots.Add(hidingSpot1);
        // hidingSpots.Add(hidingSpot2);
        // hidingSpots.Add(hidingSpot3);
        // hidingSpots.Add(hidingSpot4);
        // hidingSpots.Add(hidingSpot5);
        // hidingSpots.Add(hidingSpotFinal);
        
        /* I like that you thought ahead and wanted to place ester in the hiding spot, but it's easier for the level designers to just drag ester to the
         * hiding spot they want him to start at instead of placing him in the var for the the first spot. When testing, they might move around the spots
         * and it would be a bit tedious to keep track of the first and move it around
         */
        // Ester.transform.position = hidingSpot1;
        
        
        
        // Get all the hidingspots in the scene
         hidingspots = GameObject.FindGameObjectsWithTag("hiding spot");
    }
    
    void Update()
    {
        // for(int i = 0; i<hidingSpots.Count - 1; i++) {
        //     if(Math.Abs(v3AbsTotal(Sigmund.transform.position) - v3AbsTotal(hidingSpots[i])) < 40) {
        //         Ester.transform.position = hidingSpots[i+1];
        //         for(int ii = 0; ii <= i; ii++) {
        //             hidingSpots.RemoveAt(ii);
        //         }
        //         i = 0;
        //     }
        // }

        Ester.transform.position = Get2ndClosestHidingSpot();
    }

    Vector3 Get2ndClosestHidingSpot()
    {
        // Make vars to keep track of the closest and second closest spots
        float closestDistance = float.PositiveInfinity;
        float secondClosestDistance = float.PositiveInfinity;
        Vector3 closestSpot = Vector3.zero;
        Vector3 secondClosestSpot = Vector3.zero;
    
        // Check all hiding spots
        foreach (var spot in hidingspots)
        {
            // Get the distance of the current spot from Sigmund
            float distance = Vector3.Distance(Sigmund.transform.position, spot.transform.position);
        
            // Update closest and second closest spots accordingly
            if (distance < closestDistance)
            {
                secondClosestDistance = closestDistance;
                secondClosestSpot = closestSpot;
                closestDistance = distance;
                closestSpot = spot.transform.position;
            }
            else if (distance < secondClosestDistance)
            {
                secondClosestDistance = distance;
                secondClosestSpot = spot.transform.position;
            }
        }

        // Return the second closest spot
        return secondClosestSpot;
    }

    


    // Not too sure how this worked
    // int v3AbsTotal(Vector3 v) { 
    //     return ((int)(Math.Abs(v.x) + Math.Abs(v.y) + Math.Abs(v.z))); 
    // }

}
