using System;
using Ltg8.Inventory;
using TriInspector;
using UnityEngine;

// Binds the dependencies of the global services to some implementations
namespace Ltg8
{
    [HideMonoScript]
    public class Ltg8Initializer : MonoBehaviour
    {
        [Required] public Ltg8Settings settings;
    
        [Title("Bindings")]
    
        [Required] public DiskSaveSerializer diskSaveSerializer;
        [Required] public InMemorySaveSerializer inMemorySaveSerializer;
        [Required] public FmodValueAnimator fmodValueAnimator;
        [Required] public PersistentAudio persistentAudio;
        [Required] public Camera mainCamera;
        [Required] public TextBoxPresenter textBoxPresenter;
        [Required] public ItemRegistry itemRegistry;
        [Required] public FadeScreenTransition levelChangeTransition;
     
        private void Awake()
        {
            Ltg8.Settings = settings;
            Ltg8.FmodValueAnimator = fmodValueAnimator;
            Ltg8.PersistentAudio = persistentAudio;
            Ltg8.GameState = new AsyncStateMachine<IGameState>();
            Ltg8.TextBoxPresenter = textBoxPresenter;
            Ltg8.Save = new SaveData();
            Ltg8.MainCamera = mainCamera;
            Ltg8.Controls = new Ltg8Controls();
            Ltg8.Controls.Enable();
            Ltg8.ItemRegistry = itemRegistry;
            Ltg8.LevelChangeTransition = levelChangeTransition;
            
// #if UNITY_EDITOR
            switch (Ltg8.Settings.editorSaveStrategy)
            {
                case EditorSaveStrategy.NonPersistent:
                {
                    Ltg8.Serializer = inMemorySaveSerializer;
                    break;
                }
                case EditorSaveStrategy.FromDisk:
                {
                    Ltg8.Serializer = diskSaveSerializer;
                    break;
                }
                default: throw new ArgumentOutOfRangeException();
            }
// #else
//             Ltg8.Serializer = diskSaveSerializer;
// #endif
        }

        private void Update()
        {
            Ltg8.GameState.CurrentState?.OnUpdate();
        }
    }
}
