using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private InputAction moveAction;
    private InputAction jumpAction;
    private PlayerInput playerIn;
    private Vector2 _moveInput;
    private bool _jumpPressed = false;
    private bool _isGrounded = true;
    private bool _isIdle = true;
    private bool _isWalking = false;
    [SerializeField] private Rigidbody playerRb;
    [SerializeField] private float walkMoveSpeed;
    [SerializeField] private float sprintMoveSpeed;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float playerHeight;
    [SerializeField] private EventReference walkingSounds;
    [SerializeField] private LayerMask groundLayer;

    private void Start()
    {
        playerIn = GetComponent<PlayerInput>();
        playerRb = GetComponent<Rigidbody>();

        moveAction = playerIn.actions["Move"];
        jumpAction = playerIn.actions["Jump"];

        moveAction.performed += context => OnMove(context);
        jumpAction.performed += context => OnJump(context);

        moveAction.canceled += _ => onMoveCanceled();
    }

    private void FixedUpdate()
    {
        _isGrounded = isGrounded();
        if(_isWalking)
            Move(_moveInput);
    }
    
    // FreeMovement Logic
    private void Move(Vector2 playerLocation)
    {
        Vector3 playerDirection = new Vector3(playerLocation.x * (walkMoveSpeed * Time.fixedDeltaTime), 0f, playerLocation.y * (walkMoveSpeed * Time.fixedDeltaTime));
            
        playerRb.position += playerDirection;

        RuntimeManager.PlayOneShot(walkingSounds);
    }

    //TO DO: Add logic that jumps at different gravity values for planets
    private void Jump()
    {
        playerRb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
        _jumpPressed = false;
    }

    // Use ONCollisionEnter and Exit for audio
    // Until Raycast is implemented to check if the player is grounded
    // use these variables
    
    // private void OnCollisionEnter() { _isGrounded = true; }
    // private void OnCollisionExit() { _isGrounded = false; }

    public void OnMove(InputAction.CallbackContext context) 
    {
        _isIdle = false;
        _isWalking = true;
        _moveInput = context.ReadValue<Vector2>();
    }

    private void onMoveCanceled(){
        _isIdle = true;
        _isWalking = false;
    }

    public void OnLookX(InputAction.CallbackContext context)
    {
        
    }
    
    public void OnLookY(InputAction.CallbackContext context)
    {
        
    }
    
    public void OnJump(InputAction.CallbackContext context)
    {
        _jumpPressed = true;
        
        Debug.Log(_jumpPressed);
        Debug.Log(_isGrounded);

        if(_jumpPressed && _isGrounded) 
            Jump(); // if jumping & grounded, jumps
    }

    private bool isGrounded(){
        //creates a raycast down from half the player height.
        Vector3 castPoint = transform.position;
        castPoint.y = transform.position.y + playerHeight * .5f;
        return Physics.Raycast(castPoint, Vector3.down, playerHeight * .5f , groundLayer);
    }
}
