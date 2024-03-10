using UnityEngine;

namespace Animation
{
    public class AnimationController : MonoBehaviour
    {

        private Animator _animator;
        private float _time;
        private float _speed;

        // ReSharper disable once MemberCanBePrivate.Global
        // Sets all the variables above to the given values
        public void SetParameters(Animator animator, float speed = 1, float time = 0)
        {
            // Speed defaults to 1 and time to 0
            _animator = animator;
            _time = time/speed;
            _speed = speed;
        }
        
        // ReSharper disable once MemberCanBePrivate.Global
        // Plays an animation, using the local speed and animator values
        public void PlayAnimation()
        {
            _animator.speed = _speed;
            _animator.enabled = true;
        }
        
        // ReSharper disable once MemberCanBePrivate.Global
        // Plays an animation, using values other than the local speed & animator values
        public void PlayAnimation(Animator animator, float speed)
        {
            SetParameters(animator, speed);
            _animator.speed = _speed;
            _animator.enabled = true;
        }
        
        // Pauses an animation 
        public void PauseAnimation()
        {
            _animator.enabled = false;
        }
        
        // Pauses an animation for an animator other than the local one
        public void PauseAnimation(Animator animator)
        {
            SetParameters(animator);
            _animator.enabled = false;
        }

        // Resets an animation to the beginning 
        public void ResetAnimation(Animator animator)
        {
            _animator.playbackTime = 0;
        }
        
        /* Plays the animation, and then pauses it after some time. Currently doesn't use the
         local animator as the default*/
        public void PlayandPauseAnimation(Animator animator, float speed = 1, float time = 0)
        {
            SetParameters(animator, speed, time);
            PlayAnimation();
            Invoke(nameof(PauseAnimation), _time);
        }
    }
}
