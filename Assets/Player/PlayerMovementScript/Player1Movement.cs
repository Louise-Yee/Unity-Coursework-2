using Unity.VisualScripting;
using UnityEngine;

public class Player1Movement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public new Rigidbody2D rigidbody2D;
    private Vector2 moveInput;
    public Vector2 lastMoveDirection { get; private set; } = Vector2.down; // Initialize with down direction

    [Header("Animation")]
    private Animator animator;

    [Header("Footstep Sound")]
    public AudioClip footstepSound;
    public AudioSource audioSource;
    public float footstepInterval = 0.5f;
    private float footstepTimer;

    [Header("Shooting")]
    public KeyCode shootKey = KeyCode.V;
    public float shootAnimationDuration = 0.15f;
    private bool isShooting = false;
    private float shootTimer = 0f;
    private ShootingScript reloadState;

    void Start()
    {
        SetupComponents();
    }

    void Update()
    {
        HandleInput();
        UpdateMovementAndAnimation();
        UpdateShooting();
    }

    private void FixedUpdate()
    {
        // Physics updates should always be in FixedUpdate
        rigidbody2D.velocity = moveInput * moveSpeed;
    }

    private void SetupComponents()
    {
        animator = GetComponent<Animator>();
        reloadState = GetComponent<ShootingScript>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = footstepSound;
            audioSource.playOnAwake = false;
        }
    }

    private void HandleInput()
    {
        // Handle shooting input
        if (Input.GetKeyDown(shootKey) && !isShooting && !reloadState.isReloading)
        {
            StartShooting();
        }

        // Only process movement input if not shooting
        if (!isShooting)
        {
            ProcessMovementInput();
        }
    }

    private void ProcessMovementInput()
    {
        moveInput.x =
            Input.GetKey(KeyCode.A) ? -1
            : Input.GetKey(KeyCode.D) ? 1
            : 0;
        moveInput.y =
            Input.GetKey(KeyCode.W) ? 1
            : Input.GetKey(KeyCode.S) ? -1
            : 0;
        moveInput.Normalize();

        // Update last move direction only when actually moving
        if (moveInput != Vector2.zero)
        {
            lastMoveDirection = moveInput;
        }
    }

    private void UpdateMovementAndAnimation()
    {
        // If shooting, ensure we're not moving
        if (isShooting)
        {
            moveInput = Vector2.zero;
        }

        UpdateAnimation();
        UpdateFootsteps();
    }

    private void UpdateAnimation()
    {
        animator.SetFloat("MoveX", moveInput.x);
        animator.SetFloat("MoveY", moveInput.y);
        animator.SetFloat("MoveMagnitude", moveInput.magnitude);
        animator.SetFloat("LastMoveX", lastMoveDirection.x);
        animator.SetFloat("LastMoveY", lastMoveDirection.y);
    }

    private void UpdateFootsteps()
    {
        if (moveInput != Vector2.zero && !animator.GetBool("isDead"))
        {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer <= 0f && audioSource != null && footstepSound != null)
            {
                audioSource.PlayOneShot(footstepSound);
                footstepTimer = footstepInterval;
            }
        }
        else
        {
            footstepTimer = 0f;
        }
    }

    private void UpdateShooting()
    {
        if (isShooting)
        {
            shootTimer -= Time.deltaTime;
            if (shootTimer <= 0)
            {
                StopShooting();
            }
        }
    }

    private void StartShooting()
    {
        isShooting = true;
        shootTimer = shootAnimationDuration;
        animator.SetBool("isShooting", true);
        moveInput = Vector2.zero; // Ensure we stop moving when starting to shoot
    }

    private void StopShooting()
    {
        isShooting = false;
        animator.SetBool("isShooting", false);
    }

    // Public methods for external access
    public Vector2 GetMoveInput() => moveInput;

    // State modification methods
    public void isOnGround() => moveSpeed = 2f;

    public void isRevived() => moveSpeed = 5f;

    public void isBeingRevived() => moveSpeed = 0f;
}
