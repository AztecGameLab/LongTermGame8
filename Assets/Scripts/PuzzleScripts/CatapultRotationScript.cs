using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultRotationScript : MonoBehaviour
{

    private GameObject catapult_platform;
    [SerializeField] private bool rotate;
    
    void Start()
    {
        catapult_platform = gameObject;
    }
    
    void Update()
    {
        if (rotate)
        {
            catapult_platform.transform.Rotate(0, 1, 0);
        }
    }

    public void rotateCatapult()
    {
        rotate = !rotate;
    }
}
