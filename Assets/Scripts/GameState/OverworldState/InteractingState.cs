using Cysharp.Threading.Tasks;
using Ltg8.Inventory;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ltg8
{
    public class InteractingState : IOverworldState
    {
        public OverworldGameState OverworldState { get; set; }

        private InventoryView _inventory;

        public async UniTask OnEnter()
        {
            _inventory = Object.FindAnyObjectByType<InventoryView>();
            _inventory.onClose.AddListener(HandleOnClose);
            await _inventory.Open(Ltg8.Save.Inventory);
            
            Ltg8.Controls.PlayerInventory.Enable();
            Ltg8.Controls.PlayerInventory.Close.performed += HandleCloseInventory;
        }
        
        public async UniTask OnExit()
        {
            await _inventory.Close();
            
            _inventory.onClose.RemoveListener(HandleOnClose);
            Ltg8.Controls.PlayerInventory.Disable();
            Ltg8.Controls.PlayerInventory.Close.performed -= HandleCloseInventory;
        }
        
        private void HandleOnClose()
        {
            OverworldState.StateMachine.TransitionTo(OverworldState.ExploringState).Forget();
        }
        
        private void HandleCloseInventory(InputAction.CallbackContext context)
        {
            _inventory.Close().Forget();
        }

        public void OnUpdate() {}
    }
}
