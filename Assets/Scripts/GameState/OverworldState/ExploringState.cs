using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ltg8
{
    /// <summary>
    /// <b>Description</b>: Methods for entering and exiting the Exploring State, in addition to handling
    /// the opening of the player's inventory
    /// <list type="number">
    ///       <item> <description><b><see cref="OnEnter()"/></b></description> </item>
    ///       <item> <description><b><see cref="OnExit()"/></b></description> </item>
    ///       <item> <description><b><see cref="HandleOpenInventory"/></b></description> </item>
    /// </list>
    /// </summary>
    public class ExploringState : IOverworldState
    {
        public OverworldGameState OverworldState { get; set; } // Getter & Ssetter for the Overworld Game State
        
        /// <summary>
        /// <b>Description</b>: When entering this state; enable free player movement, add HandleOpenInventory to the handler
        /// event, and hide / lock the cursor.
        /// </summary>
        public UniTask OnEnter()
        {
            Ltg8.Controls.PlayerFreeMovement.Enable(); // Enable Free Player Movement
            // Adds HandleOpenInventory to the OpenInventory.performed event
            Ltg8.Controls.PlayerFreeMovement.OpenInventory.performed += HandleOpenInventory;
            
            // Hide and lock the cursor
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            
            return UniTask.CompletedTask;
        }

        /// <summary>
        /// <b>Description</b>: When exiting the state, disable free player movement, and remove HandleOpenInventory from the
        /// handler event
        /// </summary>
        public UniTask OnExit()
        {
            Ltg8.Controls.PlayerFreeMovement.Disable();
            Ltg8.Controls.PlayerFreeMovement.OpenInventory.performed -= HandleOpenInventory;
            return UniTask.CompletedTask;
        }
        
        /// <summary>
        /// <b>Description</b>: When opening the inventory, transition from the Exploring State to the Interacting State
        /// </summary>
        private void HandleOpenInventory(InputAction.CallbackContext context)
        {  OverworldState.StateMachine.TransitionTo(OverworldState.InteractingState).Forget(); }

        public void OnUpdate() {}
    }
}
