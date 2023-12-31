﻿using System;
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
        [Required] public GameStateMachine gameStateMachine;
    
        private void Awake()
        {
            Ltg8.Settings = settings;
            Ltg8.FmodValueAnimator = fmodValueAnimator;
            Ltg8.PersistentAudio = persistentAudio;
            Ltg8.StateMachine = gameStateMachine;
            Ltg8.Save = new SaveData();
#if UNITY_EDITOR
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
#else
            Ltg8.Serializer = diskSaveSerializer;
#endif
        }
    }
}
