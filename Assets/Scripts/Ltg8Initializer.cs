using TriInspector;
using UnityEngine;
using UnityEngine.InputSystem;

// Binds the dependencies of the global services to some implementations
[HideMonoScript]
public class Ltg8Initializer : MonoBehaviour
{
    [Required]
    public Ltg8Settings settings;
    
    [Title("Bindings")]
    
    [Required]
    public Ltg8SaveSystem saveSystem;
    
    [Required]
    public Ltg8AudioSystem audioSystem;

    private void Awake()
    {
        Ltg8.Settings = settings;
        Ltg8.Save = saveSystem;
        Ltg8.Audio = audioSystem;
    }
}