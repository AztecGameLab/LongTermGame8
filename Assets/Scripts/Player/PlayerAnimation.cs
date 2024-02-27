using poetools.Core.Abstraction;
using UnityEngine;

namespace Ltg8.Player
{
    public class PlayerAnimation : MonoBehaviour
    {
        private static readonly int IsIdle = Animator.StringToHash("isIdle");

        [SerializeField] 
        private PlayerController controller;
        
        [SerializeField] 
        private PhysicsComponent player;
        
        [SerializeField] 
        private Animator animatorController;

        [SerializeField] 
        private float lookMagnitude;
        
        [SerializeField] 
        private float rotatePlayerSpeed;
        
        private Transform _playerTransform;
        private float _playerInitialMagnitude;
        private float _initialYaw;
        private bool _isIdle;

        private void Start()
        {
            animatorController.SetBool(IsIdle, true); // initial state: idle
            _playerTransform = GetComponent<Transform>();
            _playerInitialMagnitude = player.Velocity.magnitude;
        }

        private void Update()
        {
            RotatePlayer();
            CheckPlayerIdleState(player.Velocity.magnitude);
        }

        private void RotatePlayer()
        {
            // Threshold for player rotation based on mouse position
            if (Mathf.Abs(controller.InputYaw) <= lookMagnitude) return;
            
            float rotatePlayerY = controller.InputYaw * rotatePlayerSpeed * Time.deltaTime;
            _playerTransform.Rotate(0, rotatePlayerY, 0);
        }

        private void CheckPlayerIdleState(float currentMagnitude)
        {
            // If Sigmund isn't moving, plays idle animation
            if (currentMagnitude <= _playerInitialMagnitude)
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

        private void OnGUI()
        {
            // Debug UI code
            // static cl
            GUILayout.Label("Hello World");
        }
    }
}