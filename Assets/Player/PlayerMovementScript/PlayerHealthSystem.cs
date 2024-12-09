using System.Collections;
using UnityEngine;

public class PlayerHealthSystem : MonoBehaviour
{
    public int maxHealth = 10; // Maximum health
    private int currentHealth; // Current health
    public float pushForce = 2f; // Force to push the zombie away
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
        animator = GetComponent<Animator>();
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
        if (other.CompareTag("Zombie") && !isDowned && !other.GetComponent<Animator>().GetBool("isDead"))
        {
            // Reduce health
            currentHealth--;
            // Debug.Log($"Player hit by Zombie! Health: {currentHealth}");
        }
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

        // Debug.Log($"Player revived with {currentHealth} health!");
    }
}
