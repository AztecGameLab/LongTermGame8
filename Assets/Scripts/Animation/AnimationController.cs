using UnityEngine;

namespace Animation
{
    public class AnimationController : MonoBehaviour
    {

        private Animator _animator;
        private float _time;
        private float _speed;

        // ReSharper disable once MemberCanBePrivate.Global
        public void SetParameters(Animator animator, float speed = 1, float time = 0)
        {
            _animator = animator;
            _time = time/speed;
            _speed = speed;
        }
        
        // ReSharper disable once MemberCanBePrivate.Global
        public void PlayAnimation()
        {
            _animator.speed = _speed;
            _animator.enabled = true;
        }
        
        // ReSharper disable once MemberCanBePrivate.Global
        public void PlayAnimation(Animator animator, float speed)
        {
            SetParameters(animator, speed);
            _animator.speed = _speed;
            _animator.enabled = true;
        }
        
        public void PauseAnimation()
        {
            _animator.enabled = false;
        }
        
        public void PauseAnimation(Animator animator)
        {
            SetParameters(animator);
            _animator.enabled = false;
        }

        public void ResetAnimation(Animator animator)
        {
            _animator.playbackTime = 0;
        }
        
        public void PlayandPauseAnimation(Animator animator, float speed = 1, float time = 0)
        {
            SetParameters(animator, speed, time);
            PlayAnimation();
            Invoke(nameof(PauseAnimation), _time);
        }
    }
}
