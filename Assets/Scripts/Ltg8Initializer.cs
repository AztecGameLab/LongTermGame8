using Audio;
using Serialization;
using TNRD;
using UnityEngine;

// Binds the dependencies of the global services to some implementations
public class Ltg8Initializer : MonoBehaviour
{
    public Ltg8Settings settings;
    public SerializableInterface<ISerializer> serializerBinding;
    public SerializableInterface<IMusicPlayer> musicPlayerBinding;

    private void Awake()
    {
        Ltg8.Settings = settings;
        Ltg8.Serializer = serializerBinding.Value;
        Ltg8.MusicPlayer = musicPlayerBinding.Value;
    }
}
