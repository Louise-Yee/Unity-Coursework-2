using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ShootingScript : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 10f;

    [Header("Player Settings")]
    public bool isPlayerOne = true; // Set this in inspector to determine which player this is
    public int maxBullets = 10; // Maximum bullets before reloading
    public float reloadTime = 2f; // Time it takes to reload

    [Header("UI Elements")]
    public Text bulletCountText; // UI Text for bullet count (e.g., "5/10")
    public Image reloadSpinner; // UI Image for reload spinner

    // Controls
    private KeyCode shootKeyP1 = KeyCode.V;
    private KeyCode p1Up = KeyCode.W;
    private KeyCode p1Down = KeyCode.S;
    private KeyCode p1Left = KeyCode.A;
    private KeyCode p1Right = KeyCode.D;

    private KeyCode shootKeyP2 = KeyCode.Slash;
    private KeyCode p2Up = KeyCode.UpArrow;
    private KeyCode p2Down = KeyCode.DownArrow;
    private KeyCode p2Left = KeyCode.LeftArrow;
    private KeyCode p2Right = KeyCode.RightArrow;

    // Shooting and Reloading State
    private int currentBullets;
    private bool isReloading = false;
    public Vector2 currentDirection = Vector2.right; // Default to right

    // Reference to the Animator component
    private Animator animator;

    void Start()
    {
        // Get the Animator component attached to the GameObject
        animator = GetComponent<Animator>();

        // Initialize bullets
        currentBullets = maxBullets;

        // Initialize UI
        UpdateBulletCountUI();
        if (reloadSpinner != null)
        {
            reloadSpinner.gameObject.SetActive(false); // Hide reload spinner initially
        }
    }

    void Update()
    {
        // if (isReloading)
        //     return; // Prevent shooting while reloading

        // Get current direction inputs
        Vector2 newDirection = GetDirectionInput();

        // If a direction is pressed, update the current direction
        if (newDirection != Vector2.zero)
        {
            currentDirection = newDirection;
        }
        // Check for shooting
        if (
            isPlayerOne
            && Input.GetKeyDown(shootKeyP1)
            && !animator.GetBool("isDown")
            && !isReloading
        )
        {
            Shoot(currentDirection);
        }
        else if (
            !isPlayerOne
            && Input.GetKeyDown(shootKeyP2)
            && !animator.GetBool("isDown")
            && !isReloading
        )
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
            if (Input.GetKey(p2Up))
                verticalInput = 1f;
            if (Input.GetKey(p2Down))
                verticalInput = -1f;
            if (Input.GetKey(p2Right))
                horizontalInput = 1f;
            if (Input.GetKey(p2Left))
                horizontalInput = -1f;
        }

        Vector2 direction = new Vector2(horizontalInput, verticalInput);
        return direction.normalized;
    }

    void Shoot(Vector2 direction)
    {
        // Decrease the bullet count
        currentBullets--;
        UpdateBulletCountUI();

        if (currentBullets <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        // Instantiate bullet
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        // Debug the bullet instantiation
        if (bullet != null)
        {
            Debug.Log("Bullet instantiated");
        }
        else
        {
            Debug.Log("Bullet is not instantiated");
        }

        // Get the SpriteRenderer component and check its properties
        SpriteRenderer spriteRenderer = bullet.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            Debug.Log("Bullet has SpriteRenderer");
            // Ensure the sprite is visible (enabled)
            spriteRenderer.enabled = true; // Just in case it's disabled
        }
        else
        {
            Debug.Log("Bullet does not have SpriteRenderer!");
        }

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = bullet.AddComponent<Rigidbody2D>();
            Debug.Log("Rigidbody is null, adding Rigidbody2D");
        }

        // Set bullet velocity and rotation
        rb.velocity = direction * bulletSpeed;

        // Calculate the angle of the bullet to face the movement or shooting direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Apply the calculated rotation
        bullet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Debugging
        Debug.Log($"Bullet fired at angle: {angle}, direction: {direction}");

        // Debug the bullet's spawn position
        Debug.Log($"Bullet spawned at: {firePoint.position}");

        if (currentBullets <= 0)
        {
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload()
    {
        isReloading = true;

        // Hide the bullet count text during reload
        if (bulletCountText != null)
        {
            bulletCountText.gameObject.SetActive(false);
        }

        // Activate and rotate the reload spinner
        if (reloadSpinner != null)
        {
            reloadSpinner.gameObject.SetActive(true);
            float elapsedTime = 0f;

            while (elapsedTime < reloadTime)
            {
                reloadSpinner.transform.Rotate(0, 0, -360 * Time.deltaTime / reloadTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            reloadSpinner.gameObject.SetActive(false); // Hide spinner after reload
        }

        currentBullets = maxBullets;
        isReloading = false;

        // Show the bullet count text after reload
        if (bulletCountText != null)
        {
            bulletCountText.gameObject.SetActive(true);
        }

        UpdateBulletCountUI();

        Debug.Log(
            $"Player {(isPlayerOne ? "1" : "2")} has reloaded. Bullets refilled to {maxBullets}."
        );
    }

    private void UpdateBulletCountUI()
    {
        if (bulletCountText != null)
        {
            bulletCountText.text = $"{currentBullets}/{maxBullets}";
        }
    }
}
