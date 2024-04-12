using Cysharp.Threading.Tasks;
using Ltg8.Inventory;
using Ltg8.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ltg8
{
    public class PausedState : IOverworldState
    {
        public OverworldGameState OverworldState { get; set; }

        private PauseView _pauseView;

        public async UniTask OnEnter()
        {
            _pauseView = Object.FindAnyObjectByType<PauseView>();
            _pauseView.onClose.AddListener(HandleOnClose);
            _pauseView.Open();
            
            Cursor.visible = true; 
            Cursor.lockState = CursorLockMode.None;
            
            Ltg8.Controls.GameplayCommon.Pause.performed += HandleClosePause;
        }
        
        public async UniTask OnExit()
        {
            _pauseView.Close();
            
            _pauseView.onClose.RemoveListener(HandleOnClose);
            Ltg8.Controls.GameplayCommon.Pause.performed -= HandleClosePause;
        }
        
        private void HandleOnClose()
        {
            OverworldState.StateMachine.TransitionTo(OverworldState.ExploringState).Forget();
        }
        
        private void HandleClosePause(InputAction.CallbackContext context)
        {
            _pauseView.Close();
        }

        public void OnUpdate() {}
    }
}