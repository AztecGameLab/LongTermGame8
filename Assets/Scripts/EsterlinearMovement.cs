using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NewBehaviourScript : MonoBehaviour
{
    public Vector3 hidingSpot1;
    public Vector3 hidingSpot2;
    public Vector3 hidingSpot3;
    public Vector3 hidingSpot4;
    public Vector3 hidingSpot5;
    public Vector3 hidingSpotFinal;
    public GameObject Ester;
    public GameObject Sigmund;
    List<Vector3> hidingSpots = new List<Vector3>();

    void Start() {
        
        hidingSpots.Add(hidingSpot1);
        hidingSpots.Add(hidingSpot2);
        hidingSpots.Add(hidingSpot3);
        hidingSpots.Add(hidingSpot4);
        hidingSpots.Add(hidingSpot5);
        hidingSpots.Add(hidingSpotFinal);
        Ester.transform.position = hidingSpot1;
    }

    int v3AbsTotal(Vector3 v) {
        return ((int)(Math.Abs(v.x) + Math.Abs(v.y) + Math.Abs(v.z)));
    } 
    void Update()
    {
        for(int i = 0; i<hidingSpots.Count - 1; i++) {
            if(Math.Abs(v3AbsTotal(Sigmund.transform.position) - v3AbsTotal(hidingSpots[i])) < 40) {
                Ester.transform.position = hidingSpots[i+1];
                for(int ii = 0; ii <= i; ii++) {
                    hidingSpots.RemoveAt(ii);
                }
                i = 0;
            }
        }
    }
}
