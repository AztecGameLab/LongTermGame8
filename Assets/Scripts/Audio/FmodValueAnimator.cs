using FMOD.Studio;
using poetools.Console.Commands;
using UnityEngine;

namespace Ltg8
{
    public class FmodValueAnimator : MonoBehaviour, IConsoleDebugInfo
    {
        private readonly AnimatedInstance[] _animatedInstances = new AnimatedInstance[50];
        private int _currentlyAnimatingInstances;

        /// <summary>
        /// Automatically changes an instances volume over time.
        /// This will NOT release the instance when finished.
        /// This WILL smoothly replace previous animations.
        /// </summary>
        /// <param name="instance">The instance to be animated. Should already be playing.</param>
        /// <param name="target">The final volume.</param>
        /// <param name="duration">How long it should take to reach the final volume.</param>
        public void AnimateVolume(EventInstance instance, float target, float duration)
        {
            // Check if this instance is already being animated, and cancel it if so.
            if (_currentlyAnimatingInstances > 0)
            {
                for (int i = 0; i < _animatedInstances.Length; ++i)
                {
                    if (_animatedInstances[i].IsValid && _animatedInstances[i].Instance.handle == instance.handle)
                    {
                        _animatedInstances[i].IsValid = false;
                        _currentlyAnimatingInstances--;
                    }
                }
            }

            // Check for an available slot in the instance array, and set it up for animation.
            for (int i = 0; i < _animatedInstances.Length; ++i)
            {
                if (!_animatedInstances[i].IsValid)
                {
                    instance.getVolume(out _animatedInstances[i].Initial);
                    _animatedInstances[i].IsValid = true;
                    _animatedInstances[i].Instance = instance;
                    _animatedInstances[i].Duration = duration;
                    _animatedInstances[i].Target = target;
                    _animatedInstances[i].Elapsed = 0;
                    _currentlyAnimatingInstances++;
                    break;
                }
            }
        }
    
        private void Awake()
        {
            for (int i = 0; i < _animatedInstances.Length; ++i)
                _animatedInstances[i].IsValid = false;
        }

        private void Update()
        {
            // Set the volume for all animating instances.
            if (_currentlyAnimatingInstances > 0)
            {
                for (int i = 0; i < _animatedInstances.Length; ++i)
                {
                    if (_animatedInstances[i].IsValid)
                    {
                        _animatedInstances[i].Elapsed += Time.deltaTime;
                        float t = _animatedInstances[i].Elapsed / _animatedInstances[i].Duration;
                        float volume = Mathf.Lerp(_animatedInstances[i].Initial, _animatedInstances[i].Target, t);
                        _animatedInstances[i].Instance.setVolume(volume);

                        // If the animation is finished, we stop it.
                        if (t >= 1)
                        {
                            _animatedInstances[i].IsValid = false;
                            _currentlyAnimatingInstances--;
                        }
                    }
                }
            }
        }

        private struct AnimatedInstance
        {
            public EventInstance Instance;
            public bool IsValid;
            public float Elapsed;
            public float Duration;
            public float Target;
            public float Initial;
        }
    
        public string DebugName => name;
    
        public void DrawDebugInfo()
        {
            GUILayout.Label($"Animating instances: {_currentlyAnimatingInstances}");
        
            foreach (AnimatedInstance instance in _animatedInstances)
            {
                if (instance.IsValid)
                    GUILayout.Label($"{instance.Instance.GetPath()} [{instance.Elapsed / instance.Duration}] -> {instance.Target}");
            }
        }
    }
}
