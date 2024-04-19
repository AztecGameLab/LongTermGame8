﻿using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ltg8
{
    public class OverworldGameState : IGameState
    {
        private readonly string _scenePath;

        public AsyncStateMachine<IOverworldState> StateMachine { get; }
        public ExploringState ExploringState { get; }
        public InteractingState InteractingState { get; }
        public PausedState PausedState { get; }
        
        public OverworldGameState(string scenePath)
        {
            if (string.IsNullOrEmpty(scenePath))
            {
                scenePath = Ltg8.Settings.defaultOverworldScenePath;
            }
            
            _scenePath = scenePath;
            
            ExploringState = new ExploringState {
                OverworldState = this,
            };
            
            InteractingState = new InteractingState {
                OverworldState = this,
            };

            PausedState = new PausedState {
                OverworldState = this,
            };
            
            StateMachine = new AsyncStateMachine<IOverworldState>();
        }
        
        public async UniTask OnEnter()
        {
            await SceneManager.LoadSceneAsync(_scenePath, LoadSceneMode.Additive);
            SceneManager.SetActiveScene(SceneManager.GetSceneByPath(_scenePath));

            Ltg8.Save.PlayerSceneName = _scenePath;
            
            await StateMachine.TransitionTo(ExploringState); // todo: maybe some worlds transition and don't default to exploring...
        }
        
        public async UniTask OnExit()
        {
            await SceneManager.UnloadSceneAsync(_scenePath);
            StateMachine.TransitionTo(null).Forget();
        }

        public void OnUpdate()
        {
            StateMachine.CurrentState?.OnUpdate();
        }
    }
}
