using System;
using System.Collections.Generic;
using DefaultNamespace;
using FMOD.Studio;
using FMODUnity;
using poetools.Console.Commands;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class Ltg8AudioSystem : MonoBehaviour, IConsoleDebugInfo
{
    /// <summary>
    /// A collection of FMOD instances that should persistent between scenes.
    /// </summary>
    public readonly Dictionary<string, EventInstance> PersistentAudio = new Dictionary<string, EventInstance>();

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
    
#region DEBUG
    
    public string DebugName => name;
    private string _debugEventPath;
    private string _debugId;
    private string _debugDuration;
    private string _debugVolume;
    private readonly List<string> _debugRemovalQueue = new List<string>();
    
    public void DrawDebugInfo()
    {
        GUILayout.Label($"Animating instances: {_currentlyAnimatingInstances}");
        
        foreach (AnimatedInstance instance in _animatedInstances)
        {
            if (instance.IsValid)
                GUILayout.Label($"{instance.Instance.GetPath()} [{instance.Elapsed / instance.Duration}] -> {instance.Target}");
        }
        
        GUILayout.Label($"Persistent Audio: {PersistentAudio.Count}");
        
        GUILayout.BeginHorizontal();
        GUILayout.Label("Volume");
        _debugVolume = GUILayout.TextField(_debugVolume);
        GUILayout.EndHorizontal();
        
        GUILayout.BeginHorizontal();
        GUILayout.Label("Duration");
        _debugDuration = GUILayout.TextField(_debugDuration);
        GUILayout.EndHorizontal();
            
        _debugRemovalQueue.Clear();

        foreach (string id in PersistentAudio.Keys)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(id);
            if (GUILayout.Button("x"))
            {
                PersistentAudio[id].stop(STOP_MODE.ALLOWFADEOUT);
                PersistentAudio[id].release();
                _debugRemovalQueue.Add(id);
            }
            if (GUILayout.Button("Set Volume"))
                AnimateVolume(PersistentAudio[id], float.Parse(_debugVolume), float.Parse(_debugDuration));
            GUILayout.EndHorizontal();
        }

        foreach (string idToRemove in _debugRemovalQueue)
            PersistentAudio.Remove(idToRemove);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Path");
        _debugEventPath = GUILayout.TextField(_debugEventPath);
        GUILayout.EndHorizontal();
        
        GUILayout.BeginHorizontal();
        GUILayout.Label("Id");
        _debugId = GUILayout.TextField(_debugId);
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Play") && !PersistentAudio.ContainsKey(_debugId))
        {
            EventInstance instance = RuntimeManager.CreateInstance(_debugEventPath);
            instance.start();
            PersistentAudio.Add(_debugId, instance);
            _debugId = string.Empty;
            _debugEventPath = string.Empty;
        }
    }
    
#endregion

}
