using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Vector2 _moveInput;
    private bool _jumpPressed;
    private bool _isGrounded;
    [SerializeField] private Rigidbody playerRb;
    [SerializeField] private float walkMoveSpeed;
    [SerializeField] private float sprintMoveSpeed;
    [SerializeField] private float jumpHeight;

    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Move(_moveInput);
    }
    
    // FreeMovement Logic
    private void Move(Vector2 playerLocation)
    {
        Vector3 playerDirection = new Vector3(playerLocation.x * (walkMoveSpeed * Time.fixedDeltaTime), 0f, playerLocation.y * (walkMoveSpeed * Time.fixedDeltaTime));
        
        playerRb.position += playerDirection;
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
    
    private void OnCollisionEnter() { _isGrounded = true; }
    private void OnCollisionExit() { _isGrounded = false; }

    public void OnMove(InputAction.CallbackContext context) 
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    public void OnLookX(InputAction.CallbackContext context)
    {
        
    }
    
    public void OnLookY(InputAction.CallbackContext context)
    {
        
    }
    
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started) _jumpPressed = true;
        
        // Note: Left off here. Implement logic for Raycasts
        // To Do: Change scene terrain to have the tag Ground
        // implement raycast and logic for jumping
        // I think we should make separate isGrounded bool method
        // then an if statement saying "If the Player isGrounded, call Jump()"
        Physics.SphereCast(playerRb.position, 5f, Vector3.down, out RaycastHit _, 5f);
        if(_jumpPressed && _isGrounded) Jump(); // if jumping & grounded, jumps
    }
}
