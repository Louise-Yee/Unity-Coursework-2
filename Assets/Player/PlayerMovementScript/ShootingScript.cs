using UnityEngine;

public class ShootingScript : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 10f;

    [Header("Player Settings")]
    public bool isPlayerOne = true; // Set this in inspector to determine which player this is

    [Header("Player 1 Controls")]
    public KeyCode shootKeyP1 = KeyCode.Space;
    public KeyCode p1Up = KeyCode.W;
    public KeyCode p1Down = KeyCode.S;
    public KeyCode p1Left = KeyCode.A;
    public KeyCode p1Right = KeyCode.D;

    [Header("Player 2 Controls")]
    public KeyCode shootKeyP2 = KeyCode.RightControl;
    public KeyCode p2Up = KeyCode.UpArrow;
    public KeyCode p2Down = KeyCode.DownArrow;
    public KeyCode p2Left = KeyCode.LeftArrow;
    public KeyCode p2Right = KeyCode.RightArrow;

    void Update()
    {
        // Check for shooting based on which player this is
        if (isPlayerOne && Input.GetKeyDown(shootKeyP1))
        {
            Vector2 shootDirection = GetShootingDirection(true);
            if (shootDirection != Vector2.zero)
            {
                Shoot(shootDirection);
            }
        }
        else if (!isPlayerOne && Input.GetKeyDown(shootKeyP2))
        {
            Vector2 shootDirection = GetShootingDirection(false);
            if (shootDirection != Vector2.zero)
            {
                Shoot(shootDirection);
            }
        }
    }

    Vector2 GetShootingDirection(bool isFirstPlayer)
    {
        float horizontalInput = 0f;
        float verticalInput = 0f;

        if (isFirstPlayer)
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

        // If no direction is pressed, return zero vector
        if (horizontalInput == 0 && verticalInput == 0)
        {
            return Vector2.zero;
        }

        // Create and normalize the direction vector
        Vector2 direction = new Vector2(horizontalInput, verticalInput);
        return direction.normalized;
    }

    void Shoot(Vector2 direction)
    {
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
        bullet.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

        Debug.Log(
            $"Player {(isPlayerOne ? "1" : "2")} shooting bullet with direction: {direction}"
        );
    }
}
