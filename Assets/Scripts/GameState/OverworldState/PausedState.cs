using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Ltg8
{
    public class PausedState : IOverworldState
    {
        public OverworldGameState OverworldState { get; set; }

        private UIControllerPauseMenu _pauseMenu;

        public async UniTask OnEnter()
        {
            _pauseMenu = Object.FindAnyObjectByType<UIControllerPauseMenu>();
            _pauseMenu.onClose.AddListener(HandleOnClose);
            _pauseMenu.Open();
            
            Cursor.visible = true; 
            Cursor.lockState = CursorLockMode.None;
            
            Ltg8.Controls.GameplayCommon.Pause.performed += HandleClosePause;
        }
        
        public async UniTask OnExit()
        {
            _pauseMenu.Close();
            
            _pauseMenu.onClose.RemoveListener(HandleOnClose);
            Ltg8.Controls.GameplayCommon.Pause.performed -= HandleClosePause;
        }
        
        private void HandleOnClose()
        {
            OverworldState.StateMachine.TransitionTo(OverworldState.ExploringState).Forget();
        }
        
        private void HandleClosePause(InputAction.CallbackContext context)
        {
            _pauseMenu.Close();
        }

        public void OnUpdate() {}
    }
}