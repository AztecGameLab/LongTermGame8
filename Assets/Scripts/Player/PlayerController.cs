using Cinemachine;
using poetools.Core.Abstraction;
using pt_player_3d.Scripts;
using UnityEngine;
using UnityEngine.Events;

namespace Ltg8.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PhysicsComponent physics;
        [SerializeField] private MovementSettings settings;
        [SerializeField] private GroundCheck3d groundCheck;
        [SerializeField] private Transform pitchTransform;
        [SerializeField] private Transform yawTransform;
        [SerializeField] private CinemachineVirtualCamera playerCamera;
        [SerializeField] private ParticleSystem sprintingParticles;
        [SerializeField] private int sensitivity = 1;

        public UnityEvent onJump;
        
        public Vector3 InputDirection { get; set; }
        public float InputPitchDelta { get; set; }
        public float InputYawDelta { get; set; }
        public bool InputJumpHeld { get; set; }

        private bool _wasInteractHeld;
        private bool _wasJumpHeld;
        private float _lastJumpTime;
        private bool _isSprinting;
        private bool _wasSprinting;
        private float _pitch;
        private float _yaw;
        private CinemachineBrain _brain;

        public void ClearInputState()
        {
            InputJumpHeld = false;
            InputDirection = Vector2.zero;
        }

        private void Start()
        {
            _brain = FindAnyObjectByType<CinemachineBrain>();
        }

        public void ChangePlayerSensitivity(int set)
        {
            sensitivity = set;
        }
        
        private void Update()
        {
            // sprinting
            if (groundCheck.IsGrounded)
                _isSprinting = Input.GetKey(KeyCode.LeftShift);

            bool shouldCameraAnim = _isSprinting && physics.Velocity.sqrMagnitude > 0.5f * 0.5f;
            playerCamera.m_Lens.FieldOfView = Mathf.Lerp(playerCamera.m_Lens.FieldOfView, shouldCameraAnim ? settings.sprintFov : settings.normalFov, settings.fovSpeed * Time.deltaTime);

            // if we are airborne, stop particles
            if (!groundCheck.IsGrounded && sprintingParticles.isPlaying)
                sprintingParticles.Stop();
            
            // if we stop sprinting, stop particles
            if (!_isSprinting && sprintingParticles.isPlaying)
                sprintingParticles.Stop();
            
            // if we start sprinting, play particles
            if (_isSprinting && !sprintingParticles.isPlaying)
                sprintingParticles.Play();
            
            // movement
            Vector3 targetVelocity = InputDirection.normalized * (_isSprinting ? settings.sprintSpeed : settings.speed); // todo: real input sys
            targetVelocity = Ltg8.MainCamera.transform.localToWorldMatrix.MultiplyVector(targetVelocity);
            targetVelocity.y = physics.Velocity.y;
            bool isAccelerating = InputDirection != Vector3.zero;
            
            if (groundCheck.IsGrounded)
            {
                physics.Velocity = isAccelerating 
                    ? Vector3.MoveTowards(physics.Velocity, targetVelocity, settings.groundAcceleration * Time.deltaTime) 
                    : Vector3.Lerp(physics.Velocity, targetVelocity, settings.groundDeceleration * Time.deltaTime);
            }
            else // is airborne
            {
                physics.Velocity = Vector3.MoveTowards(physics.Velocity, targetVelocity, settings.airAcceleration * Time.deltaTime);
            }
            
            // camera rotation
            if (ReferenceEquals(_brain.ActiveVirtualCamera, playerCamera))
            {
                _pitch += InputPitchDelta;
                _yaw += InputYawDelta * sensitivity;
                _pitch = Mathf.Clamp(_pitch, -90, 90);
                pitchTransform.localRotation = Quaternion.Euler(_pitch, 0, 0);
                yawTransform.localRotation = Quaternion.Euler(0, _yaw, 0);
            }

            if (InputJumpHeld && !_wasJumpHeld && groundCheck.IsGrounded)
            {
                // jump
                physics.Velocity += Vector3.up * settings.jumpSpeed;
                
                if (Time.time - _lastJumpTime > 0.1f)
                    onJump.Invoke();
                
                _lastJumpTime = Time.time;
            }
            
            _wasJumpHeld = InputJumpHeld;
            _wasSprinting = _isSprinting;
        }
    }
}
