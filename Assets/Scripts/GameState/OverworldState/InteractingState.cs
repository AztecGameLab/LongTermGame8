using Cysharp.Threading.Tasks;
using Ltg8.Inventory;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ltg8
{
    /// <summary>
    /// <b>Description</b>: Methods for entering and exiting the Interacting State, in addition to handling
    /// the opening of the player's inventory
    /// <list type="number">
    ///       <item> <description><b><see cref="OnEnter()"/></b></description> </item>
    ///       <item> <description><b><see cref="OnExit()"/></b></description> </item>
    ///       <item> <description><b><see cref="HandleOnClose()"/></b></description> </item>
    ///       <item> <description><b><see cref="HandleCloseInventory"/></b></description> </item>
    /// </list>
    /// </summary>
    public class InteractingState : IOverworldState
    {
        public OverworldGameState OverworldState { get; set; } // Getters and Setters for the Overworld GameState

        private InventoryView _inventory; // NOTE: Come back to this 

        /// <summary>
        /// <b>Description</b>: When entering this state; open the inventory, lock/hide the curosr, and add event listeners where needed
        /// </summary>
        public async UniTask OnEnter()
        {
            _inventory = Object.FindAnyObjectByType<InventoryView>(); // Return the first, active, loaded InventoryView Object
            _inventory.onClose.AddListener(HandleOnClose); // Add a HandleOnClose listerner into onClose 
            await _inventory.Open(Ltg8.Save.Inventory); // Open the Inventory
            
            // Show and lock the cursor 
            Cursor.visible = true; 
            Cursor.lockState = CursorLockMode.None;
            
            // Enable the player Inventory, and add HandleCloseInventory to the Close.performed event
            Ltg8.Controls.PlayerInventory.Enable();
            Ltg8.Controls.PlayerInventory.Close.performed += HandleCloseInventory;
        }
        
        /// <summary>
        /// <b>Description</b>: When exiting this state; remove certain listeners, and close the player's inventory
        /// </summary>
        public async UniTask OnExit()
        {
            await _inventory.Close(); // Close the inventory
            
            _inventory.onClose.RemoveListener(HandleOnClose); // Remove the HandleOnClose listener from the inventory onClose event
            Ltg8.Controls.PlayerInventory.Disable(); // Disable player inventory
            Ltg8.Controls.PlayerInventory.Close.performed -= HandleCloseInventory; // Remove HandleCloseInventory from the close.performed event
        }
        
        /// <summary>
        /// <b>Description</b>: When closing the inventory, transition from the Interacting State to the Exploring State
        /// </summary>
        private void HandleOnClose()
        { OverworldState.StateMachine.TransitionTo(OverworldState.ExploringState).Forget(); }
        
        /// <summary>
        /// <b>Description</b>: When closing the inventory, close the inventory :)
        /// </summary>
        private void HandleCloseInventory(InputAction.CallbackContext context)
        { _inventory.Close().Forget(); }

        public void OnUpdate() {}
    }
}
