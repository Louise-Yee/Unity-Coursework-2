using System.Collections;
using UnityEngine;

public class PlayerHealthSystem : MonoBehaviour
{
    public int maxHealth = 10; // Maximum health
    private int currentHealth; // Current health

    public float pushForce = 2f; // Force to push the zombie away
    float pushDuration = 0.2f; // Duration of the push in seconds

    public float revivalTime = 5f; // Time required to revive the player
    public int reviveHealth = 5; // Health restored upon revival

    public bool isPlayerOne;
    private bool isDowned = false; // Tracks if the player is downed
    private bool isBeingRevived = false; // Tracks if the player is being revived

    private Player1Movement player1Movement;
    private Player2Movement player2Movement;
    private Animator animator;

    void Start()
    {
        Animator animator = GetComponent<Animator>();
        if (isPlayerOne)
        {
            player1Movement = GetComponent<Player1Movement>();
        }
        else
        {
            player2Movement = GetComponent<Player2Movement>();
        }

        // Initialize the player's health
        currentHealth = maxHealth;
    }

    void Update()
    {
        // Check for game over
        if (currentHealth <= 0 && !isDowned)
        {
            EnterDownedState();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Zombie") && !isDowned)
        {
            // Reduce health
            currentHealth--;
            Debug.Log($"Player hit by Zombie! Health: {currentHealth}");

            // Calculate the direction to push the zombie away
            Vector2 pushDirection = (other.transform.position - transform.position).normalized;

            // Apply force to the zombie for a limited duration
            Rigidbody2D zombieRigidbody = other.GetComponent<Rigidbody2D>();
            if (zombieRigidbody != null)
            {
                StartCoroutine(ApplyPushForce(zombieRigidbody, pushDirection));
                Debug.Log("Zombie pushed away!");
            }
        }
    }

    private IEnumerator ApplyPushForce(Rigidbody2D zombieRigidbody, Vector2 direction)
    {
        Vector2 originalVelocity = zombieRigidbody.velocity;

        // Apply the push force
        zombieRigidbody.velocity = direction * pushForce;

        // Wait for the push duration
        yield return new WaitForSeconds(pushDuration);

        // Reset the zombie's velocity
        zombieRigidbody.velocity = originalVelocity;
    }

    private void EnterDownedState()
    {
        isDowned = true;
        Debug.Log("Player is downed!");

        // Reduce movement speed for the downed state
        if (isPlayerOne)
        {
            player1Movement.isOnGround();
        }
        else
        {
            player2Movement.isOnGround();
        }
        animator.SetBool("isDown", true);
        // Set movement speed for downed state
    }

    public void StartRevival()
    {
        if (isDowned && !isBeingRevived)
        {
            Debug.Log("Player is being revived...");
            isBeingRevived = true;
            if (isPlayerOne)
            {
                player1Movement.isBeingRevived();
            }
            else
            {
                player2Movement.isBeingRevived();
            }

            // Start revival coroutine
            StartCoroutine(RevivePlayer());
        }
    }

    private IEnumerator RevivePlayer()
    {
        yield return new WaitForSeconds(revivalTime);

        // Revive the player
        isBeingRevived = false;
        isDowned = false;
        currentHealth = reviveHealth;
        if (isPlayerOne)
        {
            player1Movement.isRevived(); // Restore movement speed
        }
        else
        {
            player2Movement.isRevived();
        }
        animator.SetBool("isDown", false);

        Debug.Log($"Player revived with {currentHealth} health!");
    }
}
