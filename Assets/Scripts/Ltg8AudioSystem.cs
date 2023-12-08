using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

public class Ltg8AudioSystem : MonoBehaviour
{
    /// <summary>
    /// A collection of FMOD instances that should persistent between scenes.
    /// </summary>
    public readonly Dictionary<string, EventInstance> PersistentAudio = new Dictionary<string, EventInstance>();

    private readonly AnimatedInstance[] animatedInstances = new AnimatedInstance[50];
    private int currentlyAnimatingInstances;

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
        if (currentlyAnimatingInstances > 0)
        {
            for (int i = 0; i < animatedInstances.Length; ++i)
            {
                if (animatedInstances[i].IsValid && animatedInstances[i].Instance.handle == instance.handle)
                {
                    animatedInstances[i].IsValid = false;
                    currentlyAnimatingInstances--;
                }
            }
        }

        // Check for an available slot in the instance array, and set it up for animation.
        for (int i = 0; i < animatedInstances.Length; ++i)
        {
            if (!animatedInstances[i].IsValid)
            {
                instance.getVolume(out animatedInstances[i].Initial);
                animatedInstances[i].IsValid = true;
                animatedInstances[i].Instance = instance;
                animatedInstances[i].Duration = duration;
                animatedInstances[i].Target = target;
                animatedInstances[i].Elapsed = 0;
                currentlyAnimatingInstances++;
            }
        }
    }
    
    private void Awake()
    {
        for (int i = 0; i < animatedInstances.Length; ++i)
            animatedInstances[i].IsValid = false;
    }

    private void Update()
    {
        // Set the volume for all animating instances.
        if (currentlyAnimatingInstances > 0)
        {
            for (int i = 0; i < animatedInstances.Length; ++i)
            {
                if (animatedInstances[i].IsValid)
                {
                    animatedInstances[i].Elapsed += Time.deltaTime;
                    float t = animatedInstances[i].Elapsed / animatedInstances[i].Duration;
                    float volume = Mathf.Lerp(animatedInstances[i].Initial, animatedInstances[i].Target, t);
                    animatedInstances[i].Instance.setVolume(volume);

                    // If the animation is finished, we stop it.
                    if (t >= 1)
                        animatedInstances[i].IsValid = false;
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
}
