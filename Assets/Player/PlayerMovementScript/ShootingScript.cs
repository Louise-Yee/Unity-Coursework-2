using UnityEngine;

public class ShootingScript : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 10f;

    [Header("Player Settings")]
    public bool isPlayerOne = true; // Set this in inspector to determine which player this is

    [Header("Player 1 Controls")]
    private KeyCode shootKeyP1 = KeyCode.V;
    private KeyCode p1Up = KeyCode.W;
    private KeyCode p1Down = KeyCode.S;
    private KeyCode p1Left = KeyCode.A;
    private KeyCode p1Right = KeyCode.D;

    [Header("Player 2 Controls")]
    private KeyCode shootKeyP2 = KeyCode.Slash;
    private KeyCode p2Up = KeyCode.UpArrow;
    private KeyCode p2Down = KeyCode.DownArrow;
    private KeyCode p2Left = KeyCode.LeftArrow;
    private KeyCode p2Right = KeyCode.RightArrow;

    // Store the last set direction for each player
    private Vector2 currentDirection = Vector2.right; // Default to right

    // Reference to the Animator component
    private Animator animator;

    void Start()
    {
        // Get the Animator component attached to the GameObject
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Get current direction inputs
        Vector2 newDirection = GetDirectionInput();

        // If a direction is pressed, update the current direction
        if (newDirection != Vector2.zero)
        {
            currentDirection = newDirection;
        }

        // Check for shooting based on which player this is
        if (isPlayerOne && Input.GetKeyDown(shootKeyP1) && !animator.GetComponent<Animator>().GetBool("isDown"))
        {
            Shoot(currentDirection);
        }
        else if (!isPlayerOne && Input.GetKeyDown(shootKeyP2) && !animator.GetComponent<Animator>().GetBool("isDown"))
        {
            Shoot(currentDirection);
        }
    }

    Vector2 GetDirectionInput()
    {
        float horizontalInput = 0f;
        float verticalInput = 0f;

        if (isPlayerOne)
        {
            // Player 1 direction input (WASD)
            if (Input.GetKey(p1Up))
                verticalInput = 1f;
            if (Input.GetKey(p1Down))
                verticalInput = -1f;
            if (Input.GetKey(p1Right))
                horizontalInput = 1f;
            if (Input.GetKey(p1Left))
                horizontalInput = -1f;
        }
        else
        {
            // Player 2 direction input (Arrow Keys)
            if (Input.GetKey(p2Up))
                verticalInput = 1f;
            if (Input.GetKey(p2Down))
                verticalInput = -1f;
            if (Input.GetKey(p2Right))
                horizontalInput = 1f;
            if (Input.GetKey(p2Left))
                horizontalInput = -1f;
        }

        // Create and normalize the direction vector
        Vector2 direction = new Vector2(horizontalInput, verticalInput);
        return direction.normalized;
    }

    void Shoot(Vector2 direction)
    {
        // If no direction is set, don't shoot
        if (direction == Vector2.zero)
            return;

        // Instantiate the bullet at the fire point's position
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        // Get or add Rigidbody2D component
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = bullet.AddComponent<Rigidbody2D>();
        }

        // Calculate bullet direction and velocity
        Vector2 bulletVelocity = direction * bulletSpeed;
        rb.velocity = bulletVelocity;

        // Rotate bullet sprite to face the shooting direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Debug.Log(
        //     $"Player {(isPlayerOne ? "1" : "2")} shooting bullet with direction: {direction}"
        // );
    }
}
