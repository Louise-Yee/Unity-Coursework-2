using UnityEngine;

public class Player2Movement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public new Rigidbody2D rigidbody2D;
    private Vector2 moveInput;

    // Reference to the Animator component
    private Animator animator;
    public Vector2 lastMoveDirection { get; private set; }

    // Footstep sound
    [Header("Footstep Sound")]
    public AudioClip footstepSound;
    public AudioSource audioSource;
    public float footstepInterval = 0.5f; // Time between footstep sounds

    private float footstepTimer;

    void Start()
    {
        // Get the Animator component attached to the GameObject
        animator = GetComponent<Animator>();

        // Setup AudioSource if not assigned
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = footstepSound;
            audioSource.playOnAwake = false;
        }
    }

    void Update()
    {
        // Custom input keys for Player 2
        moveInput.x =
            Input.GetKey(KeyCode.LeftArrow) ? -1
            : Input.GetKey(KeyCode.RightArrow) ? 1
            : 0;
        moveInput.y =
            Input.GetKey(KeyCode.UpArrow) ? 1
            : Input.GetKey(KeyCode.DownArrow) ? -1
            : 0;

        // Normalize the movement input
        moveInput.Normalize();

        // Update the Rigidbody velocity for movement
        rigidbody2D.velocity = moveInput * moveSpeed;

        // Update lastMoveDirection if the player is moving
        if (moveInput != Vector2.zero)
        {
            lastMoveDirection = moveInput;
        }

        // Play footstep sound
        PlayFootstepSound();

        // Update animation parameters
        UpdateAnimatorParameters(moveInput);
    }

    // Update Animator parameters to control animations
    private void UpdateAnimatorParameters(Vector2 movement)
    {
        // Set the horizontal and vertical direction
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        // Set the speed parameter to determine idle or walking
        animator.SetFloat("Speed", movement.magnitude);

        if (movement != Vector2.zero && !animator.GetBool("isDown"))
        {
            animator.Play("player_walk");
        }
        else if (movement == Vector2.zero && !animator.GetBool("isDown"))
        {
            animator.Play("player_idle");
        }
    }

    // Play footstep sound at intervals when moving
    private void PlayFootstepSound()
    {
        if (moveInput != Vector2.zero && !animator.GetBool("isDown"))
        {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer <= 0f)
            {
                if (audioSource != null && footstepSound != null)
                {
                    audioSource.PlayOneShot(footstepSound);
                }
                footstepTimer = footstepInterval;
            }
        }
        else
        {
            footstepTimer = 0f; // Reset timer when not moving
        }
    }

    // This can be used by other scripts to get the current movement direction
    public Vector2 GetMoveInput()
    {
        return moveInput;
    }

    // Methods to handle different movement states
    public void isOnGround()
    {
        this.moveSpeed = 2f;
    }

    public void isRevived()
    {
        this.moveSpeed = 5f;
    }

    public void isBeingRevived()
    {
        this.moveSpeed = 0f;
    }
}
