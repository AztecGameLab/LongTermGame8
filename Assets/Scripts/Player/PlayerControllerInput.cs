using UnityEngine;
using UnityEngine.InputSystem;

namespace Ltg8.Player
{
    public class PlayerControllerInput : MonoBehaviour
    {
        [SerializeField] private PlayerController controller;

        private void Start()
        {
            Ltg8Controls.PlayerFreeMovementActions movement = Ltg8.Controls.PlayerFreeMovement;
            
            movement.LookX.performed += HandleLookX;
            movement.LookY.performed += HandleLookY;
            movement.Jump.performed += HandleJump;
            movement.Jump.canceled += HandleJump;
            movement.MoveX.performed += HandleMoveX;
            movement.MoveX.canceled += HandleMoveX;
            movement.MoveY.performed += HandleMoveY;
            movement.MoveY.canceled += HandleMoveY;
        }
        
        private void HandleJump(InputAction.CallbackContext context)
        {
            if (context.performed)
                controller.InputJumpHeld = true;
            
            else if (context.canceled)
                controller.InputJumpHeld = false;
        }

        private void HandleMoveX(InputAction.CallbackContext context)
        {
            Vector3 dir = controller.InputDirection;
            if (context.performed) dir.x = context.ReadValue<float>();
            else if (context.canceled) dir.x = 0;
            controller.InputDirection = dir;
        }

        private void HandleMoveY(InputAction.CallbackContext context)
        {
            Vector3 dir = controller.InputDirection;
            if (context.performed) dir.z = context.ReadValue<float>();
            else if (context.canceled) dir.z = 0;
            controller.InputDirection = dir;
        }

        private void HandleLookX(InputAction.CallbackContext context)
        {
            if (context.performed)
                controller.InputYaw += context.ReadValue<float>();
        }
        
        private void HandleLookY(InputAction.CallbackContext context)
        {
            if (context.performed)
                controller.InputPitch -= context.ReadValue<float>();
        }
    }
}
