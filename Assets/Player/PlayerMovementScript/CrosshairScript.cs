using UnityEngine;

public class CrosshairController : MonoBehaviour
{
    public Player1Movement playerMovement;
    public Player2Movement player2Movement;

    [Tooltip("Crosshair sprite or image renderer")]
    public SpriteRenderer crosshairSprite;

    [Tooltip("Distance of crosshair from player")]
    public float crosshairDistance = 5f;

    [Tooltip("Smoothing speed for crosshair movement")]
    public float smoothSpeed = 10f;

    [Tooltip("Acceleration curve for crosshair movement")]
    public AnimationCurve movementCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    private Vector3 targetCrosshairPosition;
    private Vector3 currentCrosshairPosition;
    private float currentInterpolationTime = 0f;
    private Vector2 previousMoveInput = Vector2.zero;
    public bool isPlayerOne = false;

    void Start()
    {
        if (isPlayerOne)
        {
            playerMovement = FindObjectOfType<Player1Movement>(); // Find Player1Movement
        }
        else
        {
            player2Movement = FindObjectOfType<Player2Movement>(); // Find Player2Movement
        }

        // Initialize positions
        currentCrosshairPosition = transform.position;
        targetCrosshairPosition = currentCrosshairPosition;
    }

    void Update()
    {
        // Get the player's current movement input based on player selection
        Vector2 moveInput = Vector2.zero;

        if (isPlayerOne)
        {
            moveInput = playerMovement.GetMoveInput(); // Use Player1Movement input
        }
        else
        {
            moveInput = player2Movement.GetMoveInput(); // Use Player2Movement input
        }

        // Determine target crosshair position
        if (moveInput != Vector2.zero)
        {
            // Position the crosshair in the direction of movement
            targetCrosshairPosition =
                (isPlayerOne ? playerMovement.transform : player2Movement.transform).position
                + new Vector3(moveInput.x, moveInput.y, 0).normalized * crosshairDistance;
        }
        else
        {
            // If not moving, use the last movement direction
            Vector3 lastMoveDirection = isPlayerOne
                ? playerMovement.lastMoveDirection
                : player2Movement.lastMoveDirection;
            targetCrosshairPosition =
                (isPlayerOne ? playerMovement.transform : player2Movement.transform).position
                + new Vector3(lastMoveDirection.x, lastMoveDirection.y, 0).normalized
                    * crosshairDistance;
        }

        // Check if movement input has changed
        if (moveInput != previousMoveInput)
        {
            // Reset interpolation when direction changes
            currentInterpolationTime = 0f;
            previousMoveInput = moveInput;
        }

        // Interpolate crosshair position
        currentInterpolationTime += Time.deltaTime * smoothSpeed;
        float curveFactor = movementCurve.Evaluate(currentInterpolationTime);

        currentCrosshairPosition = Vector3.Lerp(
            currentCrosshairPosition,
            targetCrosshairPosition,
            curveFactor
        );

        // Apply the interpolated position
        transform.position = currentCrosshairPosition;

        // Smoothly rotate to face the movement direction
        if (moveInput != Vector2.zero)
        {
            float angle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.Euler(0, 0, angle - 90),
                curveFactor
            );
        }
    }
}
