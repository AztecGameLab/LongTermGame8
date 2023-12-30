using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;
using STOP_MODE = FMOD.Studio.STOP_MODE;

// 
public class PlayerController : MonoBehaviour
{
    // TO DO: Add logic that jumps different heights depending on gravity
    private EventInstance _playerFootsteps;
    private InputAction _moveAction, _jumpAction;
    private PlayerInput _playerInput;
    private Vector3 _initialPosition;
    private Vector2 _playerInputValue;
    private bool _initialJump;
    private float _velocityMagnitude;
    
    [SerializeField] private Rigidbody playerRb;
    [SerializeField] private float walkMoveSpeed, jumpHeight;
    [SerializeField] private LayerMask groundLayer;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        
        // Applying Jump keybind to call OnJump based on _playerInput defined on map
        _jumpAction = _playerInput.actions["Jump"];
        _jumpAction.performed += OnJump; // when action has been performed, jump
        _jumpAction.performed -= OnJump;

        // Audio instance for walking
        _playerFootsteps = AudioManager.Instance.CreateEventInstance(FMODEvents.Instance.GroundWalkingSfx);
        // This is for 3D Sounds with spatial qualities
        // RuntimeManager.AttachInstanceToGameObject(EventInstance instance, Transform, Rigidbody)
        RuntimeManager.AttachInstanceToGameObject(_playerFootsteps, playerRb.transform, playerRb);
        
        _initialPosition = playerRb.position;
        _initialJump = false;
    }

    private void FixedUpdate()
    {
        Move(_playerInputValue);
        UpdateSound();
        Debug.Log(playerRb.velocity);
    }
    
    // Separate move logic to be used in FixedUpdate
    private void Move(Vector2 playerLocation)
    {
        Vector3 playerMovement = new(playerLocation.x, 0f, playerLocation.y);
        playerMovement = playerMovement.normalized * (walkMoveSpeed * Time.fixedDeltaTime);
        playerRb.MovePosition(playerRb.position + playerMovement);

        CalculateVelocity();
    }
    
    public void OnMove(InputAction.CallbackContext context) 
    {
        _playerInputValue = context.ReadValue<Vector2>();
        RuntimeManager.AttachInstanceToGameObject(_playerFootsteps, playerRb.transform, playerRb);
    }

    public void OnLookX(InputAction.CallbackContext context)
    {
        
    }
    
    public void OnLookY(InputAction.CallbackContext context)
    {
        
    }
    
    public void OnJump(InputAction.CallbackContext context)
    {
        if (!IsGrounded()) return;
        playerRb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
        _initialJump = true;
    }

    private bool IsGrounded() // Jump physics check
    {
        return Physics.Raycast(transform.position, Vector3.down, 0.1f,groundLayer);
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (!_initialJump) return;
        
        if (other.gameObject.CompareTag("Dirt"))
        {
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.GroundJumpSfx, transform.position);
        }
    }

    private void UpdateSound()
    {
        // If the Player velocity is not 0 and Player is touching the ground, audio plays
        // When velocity is 0 and player IsGrounded OR in the air, footsteps stop
        if (_velocityMagnitude != 0 && IsGrounded())
        {
            _playerFootsteps.getPlaybackState(out PLAYBACK_STATE playbackState);
            if (playbackState.Equals(PLAYBACK_STATE.STOPPED)) _playerFootsteps.start();
        }
        else _playerFootsteps.stop(STOP_MODE.IMMEDIATE);
    }

    private void CalculateVelocity()
    {
        Vector3 currentPlayerPosition = playerRb.position;
        Vector3 distanceMoved = currentPlayerPosition - _initialPosition;
        Vector3 playerVelocity = distanceMoved / Time.fixedDeltaTime;
        
        _initialPosition = currentPlayerPosition;
        _velocityMagnitude = playerVelocity.magnitude;
    }
    
}
