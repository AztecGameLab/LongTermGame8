using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultRotationScript : MonoBehaviour
{

    private GameObject catapult;
    [SerializeField] private bool rotate;
    
    void Start()
    {
        catapult = gameObject;
    }
    
    void Update()
    {
        if (rotate)
        {
            catapult.transform.Rotate(0, 1, 0);
        }
    }

    public void rotateCatapult()
    {
        rotate = !rotate;
    }
}
