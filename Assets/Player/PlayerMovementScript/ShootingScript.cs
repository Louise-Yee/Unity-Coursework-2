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

    [Header("Audio")]
    public AudioClip shootSound; // Sound for shooting
    public AudioClip dryFireSound; // Sound for weapon dry (no ammo)
    public AudioClip reloadStartSound; // Sound when reload starts
    public AudioClip reloadFinishSound; // Sound when reload finishes
    private AudioSource audioSource; // Reference to AudioSource component

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

        // Get AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
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

        // Check for shooting
        if (
            isPlayerOne
            && Input.GetKeyDown(shootKeyP1)
            && !animator.GetBool("isDown")
            && !isReloading
        )
        {
            HandleShooting();
        }
        else if (
            !isPlayerOne
            && Input.GetKeyDown(shootKeyP2)
            && !animator.GetBool("isDown")
            && !isReloading
        )
        {
            HandleShooting();
        }
    }

    void HandleShooting()
    {
        if (currentBullets > 0)
        {
            Shoot(currentDirection);
            PlayAudio(shootSound); // Play shooting sound
        }
        else
        {
            PlayAudio(dryFireSound); // Play dry fire sound
            StartCoroutine(Reload());
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
        currentBullets--;
        UpdateBulletCountUI();

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = bullet.AddComponent<Rigidbody2D>();
        }

        rb.velocity = direction * bulletSpeed;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (currentBullets <= 0)
        {
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload()
    {
        isReloading = true;

        if (bulletCountText != null)
        {
            bulletCountText.gameObject.SetActive(false);
        }

        if (reloadSpinner != null)
        {
            reloadSpinner.gameObject.SetActive(true);
        }

        float elapsedTime = 0f;
        float reloadSoundInterval = 0.5f; // Interval between reload start sounds
        float nextSoundTime = reloadSoundInterval;

        while (elapsedTime < reloadTime)
        {
            // Play reload start sound multiple times during reload
            if (elapsedTime >= nextSoundTime)
            {
                PlayAudio(reloadStartSound);
                nextSoundTime += reloadSoundInterval; // Schedule the next sound
            }

            if (reloadSpinner != null)
            {
                reloadSpinner.transform.Rotate(0, 0, -360 * Time.deltaTime / reloadTime);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (reloadSpinner != null)
        {
            reloadSpinner.gameObject.SetActive(false); // Hide spinner after reload
        }

        currentBullets = maxBullets;
        isReloading = false;

        // Play reload finish sound
        PlayAudio(reloadFinishSound);

        if (bulletCountText != null)
        {
            bulletCountText.gameObject.SetActive(true);
        }

        UpdateBulletCountUI();
    }

    private void UpdateBulletCountUI()
    {
        if (bulletCountText != null)
        {
            bulletCountText.text = $"{currentBullets}/{maxBullets}";
        }
    }

    private void PlayAudio(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
