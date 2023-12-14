using TriInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

// Binds the dependencies of the global services to some implementations
[HideMonoScript]
public class Ltg8Initializer : MonoBehaviour
{
    [Required]
    public Ltg8Settings settings;
    
    [Title("Bindings")]
    
    [Required]
    public Ltg8SaveSerializer saveSerializer;
    
    [Required]
    public Ltg8AudioSystem audioSystem;
    
    private void Awake()
    {
        Ltg8.Settings = settings;
        Ltg8.Serializer = saveSerializer;
        Ltg8.Audio = audioSystem;
        Ltg8.Save = new Ltg8SaveData();
    }
}