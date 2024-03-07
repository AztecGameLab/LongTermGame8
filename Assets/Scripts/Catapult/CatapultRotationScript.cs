using Audio;
using UnityEngine;

namespace Catapult
{
    public class CatapultRotationScript : MonoBehaviour
    {

        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip audioClip;
        
        private GameObject _catapultPlatform;
        [SerializeField] private AudioController audioController;
        
        private bool _rotate;
    
        private void Start()
        {
            _catapultPlatform = gameObject;
        }

        private void Update()
        {
            if (_rotate)
            {
                _catapultPlatform.transform.Rotate(0, 0.3f, 0);
            }
        }

        public void RotateCatapult()
        {
            _rotate = !_rotate;
            audioController.SetParameters(audioSource, audioClip, 8, 18);
            if (audioSource.isPlaying)
            {
                audioController.ToggleAudio(true);
            }
            else
            {
                audioController.ToggleAudio();
            }
        }
    }
}
