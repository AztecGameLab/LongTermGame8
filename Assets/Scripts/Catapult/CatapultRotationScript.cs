using Audio;
using UnityEngine;

namespace Catapult
{
    public class CatapultRotationScript : MonoBehaviour
    {
        
        //Variables responsible for audio functions
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip audioClip;
        
        private GameObject _catapultPlatform;
        
        [SerializeField] private AudioController audioController; // Script for controlling audio
        
        private bool _rotate; // Determines whether platform should rotate or not
    
        private void Start()
        {
            _catapultPlatform = gameObject; // The platform is the script's parent object
        }

        private void Update()
        {
            if (_rotate) // If the catapult should rotate
            {
                _catapultPlatform.transform.Rotate(0, 0.3f, 0); // Rotate! 
            }
        }

        public void RotateCatapult()
        {
            _rotate = !_rotate; // Toggle Rotation
            if (audioSource.isPlaying)
            {
                // Stops audio from playing when rotation stops
                audioController.ToggleAudio(true);
            }
            else
            {
                // Sets up Stone-Grinding Audio for Rotation
                audioController.SetParameters(audioSource, audioClip, 8, 18); 
                audioController.ToggleAudio();
            }
        }
    }
}
