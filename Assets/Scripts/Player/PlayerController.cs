using poetools.Core.Abstraction;
using pt_player_3d.Scripts;
using UnityEngine;
using UnityEngine.Events;

namespace Ltg8.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] 
        private PhysicsComponent physics;

        [SerializeField] 
        private MovementSettings settings;

        [SerializeField] 
        private GroundCheck3d groundCheck;

        [SerializeField] 
        private Transform pitchTransform;
        
        [SerializeField] 
        private Transform yawTransform;

        public UnityEvent onJump;
        
        public Vector3 InputDirection { get; set; }
        public float InputPitch { get; set; }
        public float InputYaw { get; set; }
        public bool InputJumpHeld { get; set; }
        public bool InputInteractHeld { get; set; }

        private bool _wasInteractHeld;
        private float _lastJumpTime;

        public void ClearInputState()
        {
            InputJumpHeld = false;
            InputDirection = Vector2.zero;
        }

        private void Update()
        {
            Vector3 targetVelocity = InputDirection * settings.speed;
            targetVelocity = yawTransform.localToWorldMatrix.MultiplyVector(targetVelocity);
            targetVelocity.y = physics.Velocity.y;
            physics.Velocity = targetVelocity;
            
            // camera rotation
            InputPitch = Mathf.Clamp(InputPitch, -90, 90);
            pitchTransform.localRotation = Quaternion.Euler(InputPitch, 0, 0);
            yawTransform.localRotation = Quaternion.Euler(0, InputYaw, 0);

            if (InputJumpHeld && groundCheck.IsGrounded)
            {
                // jump
                physics.Velocity += Vector3.up * settings.jumpSpeed;
                
                if (Time.time - _lastJumpTime > 0.1f)
                    onJump.Invoke();
                
                _lastJumpTime = Time.time;
            }
            
            // proximity interact
            if (!_wasInteractHeld && InputInteractHeld)
            {
                // just started
                // search for nearest interactable
            }
            
            _wasInteractHeld = InputInteractHeld;
        }
    }
}
