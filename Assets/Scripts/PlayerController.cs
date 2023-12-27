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
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask groundLayer;

    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        _isGrounded = isGrounded();
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
    
    // private void OnCollisionEnter() { _isGrounded = true; }
    // private void OnCollisionExit() { _isGrounded = false; }

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
        
        if(_jumpPressed && _isGrounded) Jump(); // if jumping & grounded, jumps
    }

    private bool isGrounded(){
        //creates a raycast down from the player transform offset by half the player height.
        Vector3 castPoint = transform.position;
        castPoint.y = transform.position.y + playerHeight * .5f;
        return Physics.Raycast(castPoint, Vector3.down, playerHeight * .5f , groundLayer);
    }
}
