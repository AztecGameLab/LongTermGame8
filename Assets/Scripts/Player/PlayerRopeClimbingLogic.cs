using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ltg8.Player
{
    public class PlayerRopeClimbingLogic : MonoBehaviour
    {
        private static readonly int IsClimbing = Animator.StringToHash("isClimbing");
        
        [SerializeField] 
        private float enterBuffer = 0.1f;

        [SerializeField] 
        private GameObject playerBody;

        [SerializeField] 
        private Animator playerAnimator;
        
        private float _climbMovementDelta;
        private float _currentRopePos;
        private RopeAttachmentPoint _attachmentPoint;

        private void Start()
        {
            Ltg8.Controls.PlayerClimbingMovement.ClimbMove.performed += HandleClimbMove;
            Ltg8.Controls.PlayerClimbingMovement.ClimbMove.canceled += HandleClimbMove;
        }
        
        private void HandleClimbMove(InputAction.CallbackContext ctx)
        {
            // process the players input
            _climbMovementDelta = ctx.ReadValue<float>();
        }

        private void Update()
        {
            if (_attachmentPoint == null)
                return;

            // update animation if we have input
            playerAnimator.SetBool(IsClimbing, _climbMovementDelta != 0);

            // actually move the player up or down the rope based on input
            _currentRopePos += _climbMovementDelta;
            Vector3 targetPos = _attachmentPoint.BottomToTopVector * _currentRopePos;
            playerBody.transform.position = Vector3.Lerp(playerBody.transform.position, targetPos, 15 * Time.deltaTime);

            // check to see if we leave the top or bottom
            if (_currentRopePos >= _attachmentPoint.ClimbDistance && _attachmentPoint.topDismountTransform != null)
                ExitRope(RopeLocation.Top);
            
            else if (_currentRopePos <= 0 && _attachmentPoint.BottomDismountTransform != null)
                ExitRope(RopeLocation.Bottom);
        }

        // only we determine when we should exit
        private void ExitRope(RopeLocation location)
        {
            Ltg8.Controls.PlayerFreeMovement.Enable();
            Ltg8.Controls.PlayerClimbingMovement.Disable();

            switch (location)
            {
                case RopeLocation.Top:
                    playerBody.transform.position = _attachmentPoint.topDismountTransform.position;
                    break;
                case RopeLocation.Bottom:
                    playerBody.transform.position = _attachmentPoint.BottomDismountTransform.position;
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(location), location, null);
            }
            
            _attachmentPoint = null;
        }

        // other scripts call into us to enter
        public void EnterRope(RopeAttachmentPoint point, RopeLocation location)
        {
            Ltg8.Controls.PlayerFreeMovement.Disable();
            Ltg8.Controls.PlayerClimbingMovement.Enable();
            _attachmentPoint = point;
            
            switch (location)
            {
                case RopeLocation.Top:
                    _currentRopePos = point.ClimbDistance - enterBuffer;
                    break;
                case RopeLocation.Bottom:
                    _currentRopePos = enterBuffer;
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(location), location, null);
            }
        }

        public enum RopeLocation { Top, Bottom }
    }
}
