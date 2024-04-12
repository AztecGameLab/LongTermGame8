using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ltg8
{
    public class ExploringState : IOverworldState
    {
        public OverworldGameState OverworldState { get; set; }
        
        public UniTask OnEnter()
        {
            Ltg8.Controls.PlayerFreeMovement.Enable();
            Ltg8.Controls.PlayerFreeMovement.OpenInventory.performed += HandleOpenInventory;
            Ltg8.Controls.GameplayCommon.Pause.performed += HandleOpenPause;
            
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            
            return UniTask.CompletedTask;
        }

        public UniTask OnExit()
        {
            Ltg8.Controls.PlayerFreeMovement.Disable();
            Ltg8.Controls.PlayerFreeMovement.OpenInventory.performed -= HandleOpenInventory;
            Ltg8.Controls.GameplayCommon.Pause.performed -= HandleOpenInventory;
            return UniTask.CompletedTask;
        }
        
        private void HandleOpenInventory(InputAction.CallbackContext context)
        { 
            OverworldState.StateMachine.TransitionTo(OverworldState.InteractingState).Forget();
        }

        private void HandleOpenPause(InputAction.CallbackContext context)
        {
            OverworldState.StateMachine.TransitionTo(OverworldState.PausedState).Forget();
        }
        
        public void OnUpdate() {}
    }
}
