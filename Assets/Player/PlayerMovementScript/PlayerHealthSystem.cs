using System.Collections;
using UnityEngine;

public class PlayerHealthSystem : MonoBehaviour
{
    public int maxHealth = 10; // Maximum health
    private int currentHealth; // Current health

    public float pushForce = 2f; // Force to push the zombie away
    public float revivalTime = 5f; // Time required to revive the player
    public int reviveHealth = 5; // Health restored upon revival

    private bool isDowned = false; // Tracks if the player is downed
    private bool isBeingRevived = false; // Tracks if the player is being revived

    private Player1Movement player1Movement;

    void Start()
    {
        player1Movement = GetComponent<Player1Movement>();
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

            // Apply force to the zombie
            Rigidbody2D zombieRigidbody = other.GetComponent<Rigidbody2D>();
            if (zombieRigidbody != null)
            {
                zombieRigidbody.AddForce(pushDirection * pushForce, ForceMode2D.Impulse);
                Debug.Log("Zombie pushed away!");
            }
        }
    }

    private void EnterDownedState()
    {
        isDowned = true;
        Debug.Log("Player is downed!");

        // Reduce movement speed for the downed state
        player1Movement.isOnGround(); // Set movement speed for downed state
    }

    public void StartRevival()
    {
        if (isDowned && !isBeingRevived)
        {
            Debug.Log("Player is being revived...");
            isBeingRevived = true;
            player1Movement.isBeingRevived();

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
        player1Movement.isRevived(); // Restore movement speed
        Debug.Log($"Player revived with {currentHealth} health!");
    }
}
