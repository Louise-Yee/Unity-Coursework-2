using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthSystem : MonoBehaviour
{
    public int maxHealth = 10; // Maximum health
    private int currentHealth; // Current health
    public float pushForce = 2f; // Force to push the zombie away
    private float revivalTime = 5f; // Time required to revive the player
    private float revivalCount = 0f;
    public int reviveHealth = 5; // Health restored upon revival

    public bool isPlayerOne;
    private bool isDowned = false; // Tracks if the player is downed
    private Player1Movement player1Movement;
    private Player2Movement player2Movement;
    private Animator animator;
    public GameObject revivalPrompt; // UI element for revival prompt
    private PlayerHealthSystem nearbyAlivePlayer;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Image image;

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
        revivalPrompt.SetActive(false); // Hide prompt initially
    }

    void Update()
    {
        // Check for game over
        if (currentHealth <= 0 && !isDowned)
        {
            EnterDownedState();
        }

        // Check for revival input
        if (nearbyAlivePlayer!=null){
            if (nearbyAlivePlayer.isDowned && Input.GetKey(KeyCode.R))
            {
                StartRevival();
            }
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

        // Check for nearby downed player
        PlayerHealthSystem otherPlayerHealth = other.GetComponent<PlayerHealthSystem>();
        if (other.CompareTag("Player") && otherPlayerHealth != null && otherPlayerHealth.isDowned)
        {
            text.gameObject.SetActive(true);
            image.gameObject.SetActive(true);
            nearbyAlivePlayer = otherPlayerHealth;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealthSystem otherPlayerHealth = other.GetComponent<PlayerHealthSystem>();
            if (otherPlayerHealth != null && otherPlayerHealth.isDowned)
            {
                image.fillAmount = 1;
                text.gameObject.SetActive(false);
                image.gameObject.SetActive(false);
                nearbyAlivePlayer = null;
            }
            revivalCount = 0;
        }
    }

    private void EnterDownedState()
    {
        isDowned = true;
        Debug.Log("Player is downed!");
        ReviveProgress.targetObject = transform;

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
        Debug.Log("Player is being revived...");
        nearbyAlivePlayer.RevivePlayer();
    }

    private void RevivePlayer()
    {
        revivalCount += Time.deltaTime;
        image.fillAmount = revivalCount/revivalTime;
        if (revivalCount >= revivalTime){
            // Revive the player
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
            revivalCount = 0;
            ReviveProgress.targetObject = null;
            text.gameObject.SetActive(false);
            image.gameObject.SetActive(false);
        }
    }
}
