using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultRotationScript : MonoBehaviour
{

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;
    

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
            catapult_platform.transform.Rotate(0, 0.3f, 0);
        }
    }

    public void rotateCatapult()
    {
        rotate = !rotate;
        //toggleAudio(audioSource, audioClip, 7);
    }

    private void toggleAudio(AudioSource source, AudioClip clip, float loopPoint1 = 0)
    {
        if (!source.isPlaying)
        {
            source.clip = clip;
            source.time = loopPoint1;
            source.Play();
        }
        else
        {
            source.Stop();
        }
    }
}
