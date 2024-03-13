using System;
using Cinemachine;
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
        private bool _wasJumpHeld;
        private float _lastJumpTime;
        private bool _isSprinting;

        public void ClearInputState()
        {
            InputJumpHeld = false;
            InputDirection = Vector2.zero;
        }

        private void Update()
        {
            if (groundCheck.IsGrounded)
                _isSprinting = Input.GetKey(KeyCode.LeftShift);

            bool shouldCameraAnim = _isSprinting && physics.Velocity.sqrMagnitude > 0.5f * 0.5f;
            
            foreach (Camera cam in FindObjectsByType<Camera>(FindObjectsSortMode.None))
            {
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, shouldCameraAnim ? settings.sprintFov : settings.normalFov, settings.fovSpeed * Time.deltaTime);
            }

            foreach (CinemachineVirtualCamera cam in FindObjectsByType<CinemachineVirtualCamera>(FindObjectsSortMode.None))
            {
                cam.m_Lens.FieldOfView = Mathf.Lerp(cam.m_Lens.FieldOfView, shouldCameraAnim ? settings.sprintFov : settings.normalFov, settings.fovSpeed * Time.deltaTime);
            }
            
            Vector3 targetVelocity = InputDirection.normalized * (_isSprinting ? settings.sprintSpeed : settings.speed); // todo: real input sys
            targetVelocity = yawTransform.localToWorldMatrix.MultiplyVector(targetVelocity);
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
            InputPitch = Mathf.Clamp(InputPitch, -90, 90);
            pitchTransform.localRotation = Quaternion.Euler(InputPitch, 0, 0);
            yawTransform.localRotation = Quaternion.Euler(0, InputYaw, 0);

            if (InputJumpHeld && !_wasJumpHeld && groundCheck.IsGrounded)
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
            _wasJumpHeld = InputJumpHeld;
        }
    }
}
