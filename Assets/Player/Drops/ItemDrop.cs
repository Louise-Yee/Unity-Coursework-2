using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public float levitationSpeed = 2f; // Speed of the levitation (oscillation)
    public float levitationHeight = 0.2f; // Maximum height to move up and down
    public float fadeOutDuration = 5f; // Duration for the fade-out effect before destruction
    public float destroyDelay = 0f; // Delay before the destruction (optional)

    private Vector2 startPosition;
    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer
    private float fadeTimer = 0f; // Timer to track fade out
    private bool isFadingOut = false; // Flag to track if fading out

    void Start()
    {
        // Store the initial position of the item
        startPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer
    }

    void Update()
    {
        // Levitate the item up and down
        float newY = startPosition.y + Mathf.Sin(Time.time * levitationSpeed) * levitationHeight;
        transform.position = new Vector2(startPosition.x, newY);

        // Check if the fade-out should begin (e.g., after a certain amount of time or condition)
        if (!isFadingOut)
        {
            // Start fade-out after the item has been present for a set time (e.g., 5 seconds)
            // For demonstration, let's fade out after 5 seconds (can adjust)
            fadeTimer += Time.deltaTime;
            if (fadeTimer >= 5f) // Adjust the time before fading starts
            {
                isFadingOut = true;
            }
        }

        // Fade out effect
        if (isFadingOut)
        {
            float alpha = Mathf.Lerp(1f, 0f, fadeTimer / fadeOutDuration);
            Color currentColor = spriteRenderer.color;
            spriteRenderer.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);

            // Destroy item after the fade-out is complete
            if (fadeTimer >= fadeOutDuration)
            {
                Destroy(gameObject, destroyDelay); // Destroy the object with optional delay
            }

            fadeTimer += Time.deltaTime;
        }
    }
}
