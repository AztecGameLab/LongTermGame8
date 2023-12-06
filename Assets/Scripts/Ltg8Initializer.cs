using Audio;
using Serialization;
using TNRD;
using TriInspector;
using UnityEngine;

// Binds the dependencies of the global services to some implementations
[HideMonoScript]
public class Ltg8Initializer : MonoBehaviour
{
    [Required]
    public Ltg8Settings settings;
    
    [Title("Bindings")]
    
    public SerializableInterface<ISerializer> serializerBinding = new SerializableInterface<ISerializer>(NullSerializer.Instance);
    public SerializableInterface<IMusicPlayer> musicPlayerBinding = new SerializableInterface<IMusicPlayer>(NullMusicPlayer.Instance);

    private void Awake()
    {
        Ltg8.Settings = settings;
        Ltg8.Serializer = serializerBinding.Value;
        Ltg8.MusicPlayer = musicPlayerBinding.Value;
    }
}
