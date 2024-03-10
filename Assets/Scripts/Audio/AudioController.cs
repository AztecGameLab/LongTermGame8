using UnityEngine;

namespace Audio
{

    public class AudioController : MonoBehaviour
    {
        private AudioSource _source; // The source where the audio will play from
        private AudioClip _clip; // The audio clip that will play
        private float _startPoint; // Where the audio segment will start
        private float _endPoint; // Where the audio segment will end

        // Sets all the variables above to the given values
        public void SetParameters(AudioSource source, AudioClip clip, float startPoint = 0,
            float endPoint = int.MaxValue)
        {
            // Start point defaults to the beginning of the audio clip, End Point defaults to some large value 
            _source = source;
            _clip = clip;
            _startPoint = startPoint;
            _endPoint = endPoint;
        }
        
        // Play an audio clip using the local source, clip and start-time values
        public void PlayAudio()
        {
            if (!_source.isPlaying) // Ensure the source is not already playing audio
            {
                _source.clip = _clip;
                _source.time = _startPoint;
                _source.Play();
            }
        }

        // Plays an audio clip with values different than the local source, clip & start-time values
        public void PlayAudio(AudioSource source, AudioClip clip, float time)
        {
            if (!source.isPlaying) // Ensures the source is not already playing audio
            {
                source.clip = clip;
                source.time = time;
                source.Play();
            }
        }

        // Stop an audio clip
        public void StopAudio()
        {
            if (_source.isPlaying) // Ensures the source is actually playing audio
            {
                _source.Stop();
            }
        }
        
        // Toggle an audio on/off
        public void ToggleAudio(bool cancel = false)
        {
            if (cancel) // If cancel is true, toggle the audio off
            {
                CancelInvoke(nameof(ToggleAudio)); // If the Audio has already been "invoked" to toggle on, cancel it
                _source.Stop(); 
                return;
            }
            _source.clip = _clip;
            _source.time = _startPoint;
            _source.Play();
            Invoke(nameof(ToggleAudio), _endPoint - _startPoint); // Toggle the audio off once the endpoint is reached
        }
    }
}