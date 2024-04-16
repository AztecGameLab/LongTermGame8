using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moustache : MonoBehaviour
{
    [SerializeField] private Material moustache;
    [SerializeField] private GameObject sigmund;

    public void GiveMoustache()
    {
        var oldMats = sigmund.GetComponent<SkinnedMeshRenderer>().materials;
        var mats = new Material[oldMats.Length + 1];
        for (int i = 0; i < oldMats.Length; i++)
        {
            mats[i] = oldMats[i];
        }
        mats[oldMats.Length] = moustache;
        sigmund.GetComponent<SkinnedMeshRenderer>().materials = mats;
    }
}
