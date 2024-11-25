using UnityEngine;

public class Player1Movement : MonoBehaviour
{
    public float moveSpeed;
    public new Rigidbody2D rigidbody2D;
    private Vector2 moveInput;

    void Update()
    {
        // // Get movement input
        // moveInput.x = Input.GetAxisRaw("Horizontal");
        // moveInput.y = Input.GetAxisRaw("Vertical");

        // Player 1's custom input keys
        moveInput.x =
            Input.GetKey(KeyCode.A) ? -1
            : Input.GetKey(KeyCode.D) ? 1
            : 0;
        moveInput.y =
            Input.GetKey(KeyCode.W) ? 1
            : Input.GetKey(KeyCode.S) ? -1
            : 0;
        
        // Normalize the movement input
        moveInput.Normalize();
        
        // Move the player
        rigidbody2D.velocity = moveInput * moveSpeed;
    }

    // This can be used by other scripts to get the current movement direction
    public Vector2 GetMoveInput()
    {
        return moveInput;
    }
}