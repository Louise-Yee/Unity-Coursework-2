using System.Collections.Generic;
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

    // New variable to track if the player is moving automatically
    public bool isAutoMoving = true;
    private Dictionary<int, Vector2> positionDict;
    private int positionKey = 1;

    void Start()
    {
        SetupComponents();
        // Initialize the dictionary
        positionDict = new Dictionary<int, Vector2>
        {
            // Example of adding entries to the dictionary
            { 1, new Vector2(-1, 0) },
            { 2, new Vector2(12.23259f, 0.425317f) },
            { 3, new Vector2(-1, 0) },
            { 4, new Vector2(3.5858f, 3.5184f) },
            { 5, new Vector2(-1, 0) }
        };
    }

    public void Reset(){
        moveSpeed = 5f;
        isShooting = false;
        shootTimer = 0;
        isAutoMoving = true;
        positionKey = 1;
        SetupComponents();
        // Initialize the dictionary
        positionDict = new Dictionary<int, Vector2>
        {
            // Example of adding entries to the dictionary
            { 1, new Vector2(-1, 0) },
            { 2, new Vector2(12.23259f, 0.425317f) },
            { 3, new Vector2(-1, 0) },
            { 4, new Vector2(3.5858f, 3.5184f) },
            { 5, new Vector2(-1, 0) }
        };
    }

    void Update()
    {
        if (!isAutoMoving){
            HandleInput();
            UpdateMovementAndAnimation();
            UpdateShooting();
        }
        else{
            // AutoMoveToPosition(new Vector2(-1, 0));
            AutoMoveToPosition(positionDict[positionKey]);
        }
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

    public void AutoMoveToPosition(Vector2 targetPosition){
        // If the player reaches the target position, stop auto-moving
        if (isAutoMoving && Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            isAutoMoving = false; // Stop auto-movement when close to target
            moveInput = Vector2.zero; // Stop movement
            positionKey++;
        }
        
        // If still auto-moving, set move input towards target
        if (isAutoMoving && Vector2.Distance(transform.position, targetPosition) >= 0.1f)
        {
            MoveToPosition(targetPosition);
        }
    }

    private void ProcessMovementInput()
    {
        if (GameManager.player1Dead){
            moveInput.x = 0;
            moveInput.y = 0;
        }
        else{
            moveInput.x =
                Input.GetKey(KeyCode.A) ? -1
                : Input.GetKey(KeyCode.D) ? 1
                : 0;
            moveInput.y =
                Input.GetKey(KeyCode.W) ? 1
                : Input.GetKey(KeyCode.S) ? -1
                : 0;
        }
        moveInput.Normalize();

        // Update last move direction only when actually moving
        if (moveInput != Vector2.zero)
        {
            lastMoveDirection = moveInput;
        }
    }

    private void MoveToPosition(Vector2 targetPosition)
    {
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;

        // Set move input towards the target position
        moveInput = direction; 
        lastMoveDirection = direction; // Update last move direction for animation

        // Ensure animator is updated for walking animation
        animator.SetFloat("MoveX", moveInput.x);
        animator.SetFloat("MoveY", moveInput.y);
        animator.SetFloat("MoveMagnitude", moveInput.magnitude);
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
        if (moveInput != Vector2.zero && !animator.GetBool("isDown"))
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
