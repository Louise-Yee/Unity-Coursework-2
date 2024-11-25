using UnityEngine;

public class TopDownMovement : MonoBehaviour
{
    public float moveSpeed;
    public new Rigidbody2D rigidbody2D;
    private Vector2 moveInput;

    void Update()
    {
        // Get movement input
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        
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