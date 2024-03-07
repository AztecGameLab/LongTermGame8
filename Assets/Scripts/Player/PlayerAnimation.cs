using poetools.Core.Abstraction;
using UnityEngine;

namespace Ltg8.Player
{
    public class PlayerAnimation : MonoBehaviour
    {
        private static readonly int IsIdle = Animator.StringToHash("isIdle");
        private static readonly int Jump = Animator.StringToHash("jump");

        [SerializeField] 
        private PlayerController controller;
        
        [SerializeField] 
        private PhysicsComponent player;
        
        [SerializeField] 
        private Animator animatorController;

        [SerializeField] 
        private float rotatePlayerSpeed;
        
        [SerializeField]
        private Transform playerTransform;
        
        private float _initialYaw;
        private bool _isIdle;

        private void Start()
        {
            animatorController.SetBool(IsIdle, true); // initial state: idle
            controller.onJump.AddListener(() => animatorController.SetTrigger(Jump));
        }

        private void Update()
        {
            RotatePlayer();
            CheckPlayerIdleState(player.Velocity.magnitude);
        }

        private void RotatePlayer()
        {
            Vector3 facingDirection = player.Velocity;
            facingDirection.y = 0;
            facingDirection = facingDirection.normalized;
            
            // Threshold for player rotation based on mouse position
            if (facingDirection.sqrMagnitude < 0.1f) return;

            playerTransform.rotation = Quaternion.Lerp(playerTransform.rotation, Quaternion.LookRotation(facingDirection), rotatePlayerSpeed * Time.deltaTime);
            // float rotatePlayerY = controller.InputYaw * rotatePlayerSpeed * Time.deltaTime;
            // playerTransform.Rotate(0, rotatePlayerY, 0);
        }

        private void CheckPlayerIdleState(float currentMagnitude)
        {
            Vector3 facingDirection = player.Velocity;
            facingDirection.y = 0;
            facingDirection = facingDirection.normalized;
            
            // If Sigmund isn't moving, plays idle animation
            if (facingDirection.sqrMagnitude < 0.1f)
            {
                ChangeAnimationState(true);
            }
            // else if grounded and walking, plays walking animation
            else if (!controller.InputJumpHeld) 
            {
                ChangeAnimationState(false);
            }
        }

        private void ChangeAnimationState(bool newState)
        {
            animatorController.SetBool(IsIdle, newState);
        }
    }
}