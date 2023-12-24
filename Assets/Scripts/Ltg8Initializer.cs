using TriInspector;
using UnityEngine;

// Binds the dependencies of the global services to some implementations
namespace Ltg8
{
    [HideMonoScript]
    public class Ltg8Initializer : MonoBehaviour
    {
        [Required]
        public Ltg8Settings settings;
    
        [Title("Bindings")]
    
        [Required]
        public SaveSerializer saveSerializer;
    
        [Required]
        public FmodValueAnimator fmodValueAnimator;

        [Required]
        public PersistentAudio persistentAudio;

        [Required]
        public GameStateMachine gameStateMachine;
    
        private void Awake()
        {
            Ltg8.Settings = settings;
            Ltg8.Serializer = saveSerializer;
            Ltg8.FmodValueAnimator = fmodValueAnimator;
            Ltg8.PersistentAudio = persistentAudio;
            Ltg8.StateMachine = gameStateMachine;
            Ltg8.Save = new SaveData();
        }
    }
}