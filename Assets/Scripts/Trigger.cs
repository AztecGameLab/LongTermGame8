using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Handles OnTrigger events, and passes them to C# events.
/// </summary>
public class Trigger : MonoBehaviour
{
    private readonly List<Rigidbody> _currentRigidbodies = new List<Rigidbody>();
    private readonly List<Collider> _currentColliders = new List<Collider>();
    private readonly List<Rigidbody> _previousRigidbodies = new List<Rigidbody>();
    private readonly List<Collider> _previousColliders = new List<Collider>();

    [SerializeField]
    [Tooltip("Should this behavior draw debugging information and print to the console?")]
    private bool showDebug;

    [SerializeField]
    [Tooltip("Which layers should be ignored when checking collisions.")]
    private LayerMask excludeLayers;

    [Tooltip("An event that is called when a collider enters this trigger.")]
    public UnityEvent<Collider> onColliderTriggerEnter = new UnityEvent<Collider>();

    [Tooltip("An event that is called when a collider exits this trigger.")]
    public UnityEvent<Collider> onColliderTriggerExit = new UnityEvent<Collider>();

    [Tooltip("An event that is called when a collider enters this trigger.")]
    public UnityEvent<Rigidbody> onRigidbodyTriggerEnter = new UnityEvent<Rigidbody>();

    [Tooltip("An event that is called when a collider exits this trigger.")]
    public UnityEvent<Rigidbody> onRigidbodyTriggerExit = new UnityEvent<Rigidbody>();

    [Tooltip("An event that is called when any object enters the trigger")]
    public UnityEvent<GameObject> onTriggerEnter = new UnityEvent<GameObject>();
    
    [Tooltip("An event that is called when any object leaves the trigger")]
    public UnityEvent<GameObject> onTriggerExit = new UnityEvent<GameObject>();

    /// <summary>
    /// Gets or sets the layers that should be excluded from triggering this object.
    /// </summary>
    /// <value>
    /// The layers that should be excluded from triggering this object.
    /// </value>
    public LayerMask ExcludeLayers
    {
        get => excludeLayers;
        set => excludeLayers = value;
    }

    /// <summary>
    /// Gets all of the rigidbodies currently inside this trigger.
    /// </summary>
    /// <value>
    /// All of the rigidbodies currently inside this trigger.
    /// </value>
    public IReadOnlyCollection<Rigidbody> CurrentRigidbodies => _currentRigidbodies;

    /// <summary>
    /// Gets all of the colliders currently inside this trigger.
    /// </summary>
    /// <value>
    /// All of the colliders currently inside this trigger.
    /// </value>
    public IReadOnlyCollection<Collider> CurrentColliders => _currentColliders;

    private void OnTriggerStay(Collider other)
    {
        if (IsValidCollider(other) && enabled)
        {
            _currentColliders.Add(other);

            if (other.attachedRigidbody != null)
            {
                _currentRigidbodies.Add(other.attachedRigidbody);
            }
        }
    }

    private void AddRigidbodies()
    {
        foreach (Rigidbody currentRigidbody in _currentRigidbodies)
        {
            if (!_previousRigidbodies.Contains(currentRigidbody))
            {
                // this is new
                onTriggerEnter?.Invoke(currentRigidbody.gameObject);
                onRigidbodyTriggerEnter?.Invoke(currentRigidbody);

                if (showDebug)
                {
                    Debug.Log($"Rigidbody {currentRigidbody.name} entered the trigger {name}");
                }
            }
        }
    }

    private void RemoveRigidbodies()
    {
        foreach (Rigidbody previousRigidbody in _previousRigidbodies)
        {
            if (!_currentRigidbodies.Contains(previousRigidbody) && previousRigidbody != null)
            {
                // this has been removed
                onTriggerExit?.Invoke(previousRigidbody.gameObject);
                onRigidbodyTriggerExit?.Invoke(previousRigidbody);

                if (showDebug)
                {
                    Debug.Log($"Rigidbody {previousRigidbody.name} exited the trigger {name}");
                }
            }
        }
    }

    private void AddColliders()
    {
        foreach (Collider currentCollider in _currentColliders)
        {
            if (!_previousColliders.Contains(currentCollider))
            {
                // this is new
                onTriggerEnter?.Invoke(currentCollider.gameObject);
                onColliderTriggerEnter?.Invoke(currentCollider);

                if (showDebug)
                {
                    Debug.Log($"Collider {currentCollider.name} entered the trigger {name}");
                }
            }
        }
    }

    private void RemoveColliders()
    {
        foreach (Collider previousCollider in _previousColliders)
        {
            if (!_currentColliders.Contains(previousCollider) && previousCollider != null)
            {
                // this has been removed
                onTriggerExit?.Invoke(previousCollider.gameObject);
                onColliderTriggerExit?.Invoke(previousCollider);

                if (showDebug)
                {
                    Debug.Log($"Collider {previousCollider.name} exited the trigger {name}");
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (_currentRigidbodies.Count > _previousRigidbodies.Count)
        {
            // we gained some rigidbodies
            AddRigidbodies();
        }

        if (_currentRigidbodies.Count < _previousRigidbodies.Count)
        {
            // we lost some rigidbodies
            RemoveRigidbodies();
        }

        if (_currentColliders.Count > _previousColliders.Count)
        {
            // we gained some colliders
            AddColliders();
        }

        if (_currentColliders.Count < _previousColliders.Count)
        {
            // we lost some colliders
            RemoveColliders();
        }

        // Cleanup, to prepare for next physics update.
        _previousColliders.Clear();
        _previousRigidbodies.Clear();
        _previousColliders.AddRange(_currentColliders);
        _previousRigidbodies.AddRange(_currentRigidbodies);

        _currentRigidbodies.Clear();
        _currentColliders.Clear();
    }

    private bool IsValidCollider(Collider other)
    {
        // weird bit-mask code for checking if the object is a valid layer
        bool isExcludeLayer = excludeLayers.value == (excludeLayers.value | (1 << other.gameObject.layer));
        bool isTrigger = other.isTrigger;

        return !isTrigger && !isExcludeLayer;
    }

    private void OnGUI()
    {
        if (showDebug)
        {
            GUILayout.Label($"Colliders: {_currentColliders.Count}");
            GUILayout.Label($"Rigidbodies: {_currentRigidbodies.Count}");
        }
    }
}
