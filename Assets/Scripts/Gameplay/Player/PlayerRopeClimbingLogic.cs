using System;
using poetools.Core.Abstraction;
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
        private float climbSpeed = 1;

        [SerializeField] 
        private float animationSpeed = 10f;

        [SerializeField] 
        private Gravity gravity;

        [SerializeField] 
        private GameObject playerBody;
        
        [SerializeField] 
        private GameObject playerRotateTransform;

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
            playerRotateTransform.transform.rotation = Quaternion.Lerp(playerRotateTransform.transform.rotation, Quaternion.LookRotation(_attachmentPoint.BottomToTopVector.normalized), animationSpeed * Time.deltaTime);

            // actually move the player up or down the rope based on input
            _currentRopePos += _climbMovementDelta * Time.deltaTime * climbSpeed;
            Vector3 targetPos = _attachmentPoint.BottomClimbTransform.position + _attachmentPoint.BottomToTopVector * (_currentRopePos / _attachmentPoint.ClimbDistance);
            playerBody.transform.position = Vector3.Lerp(playerBody.transform.position, targetPos, animationSpeed * Time.deltaTime);

            // check to see if we leave the top or bottom
            if (_currentRopePos >= _attachmentPoint.ClimbDistance && _attachmentPoint.topDismountTransform != null)
                ExitRope(RopeLocation.Top);
            
            else if (_currentRopePos <= 0 && _attachmentPoint.BottomDismountTransform != null)
                ExitRope(RopeLocation.Bottom);
            
            // debug
            Debug.DrawLine(_attachmentPoint.BottomClimbTransform.position, targetPos);
        }

        // only we determine when we should exit
        private void ExitRope(RopeLocation location)
        {
            Ltg8.Controls.PlayerFreeMovement.Enable();
            Ltg8.Controls.PlayerClimbingMovement.Disable();
            gravity.enabled = true;
            playerAnimator.SetBool(IsClimbing, false);

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
            gravity.enabled = false;
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
