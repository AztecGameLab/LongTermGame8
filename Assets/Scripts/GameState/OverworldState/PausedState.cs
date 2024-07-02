using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Ltg8
{
    public class PausedState : IOverworldState
    {
        public OverworldGameState OverworldState { get; set; }

        private UIControllerPauseMenu _pauseMenu;
        private TextBoxView[] _dialogues;

        public UniTask OnEnter()
        {
            _pauseMenu = Object.FindAnyObjectByType<UIControllerPauseMenu>();
            _pauseMenu.onClose.AddListener(HandleOnClose);
            _pauseMenu.Open();

            _dialogues = Object.FindObjectsByType<TextBoxView>(FindObjectsSortMode.None);
            foreach (var dialogue in _dialogues)
            {
                dialogue.gameObject.SetActive(false);
            }

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            Ltg8.Controls.GameplayCommon.Pause.performed += HandleClosePause;
            return UniTask.CompletedTask;
        }

        public UniTask OnExit()
        {
            _pauseMenu.onClose.RemoveListener(HandleOnClose);
            _pauseMenu.Close();

            foreach (var dialogue in _dialogues)
            {
                dialogue.gameObject.SetActive(true);
            }

            Ltg8.Controls.GameplayCommon.Pause.performed -= HandleClosePause;
            return UniTask.CompletedTask;
        }

        private void HandleOnClose()
        {
            OverworldState.StateMachine.TransitionTo(OverworldState.ExploringState).Forget();
        }

        private void HandleClosePause(InputAction.CallbackContext context)
        {
            _pauseMenu.Close();
        }

        public void OnUpdate()
        {
        }
    }
}