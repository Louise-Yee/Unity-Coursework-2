using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthSystem : MonoBehaviour
{
    public float maxHealth = 20; // Maximum health
    public float currentHealth; // Current health
    public float dyingHealth = 20; // Dying health
    public float pushForce = 2f; // Force to push the zombie away
    private float revivalTime = 5f; // Time required to revive the player
    private float revivalCount = 0f;
    private int reviveHealth = 10; // Health restored upon revival
    public bool isPlayerOne;
    public bool isDead = false; // Tracks if the player is dead
    public bool isDowned = false; // Tracks if the player is downed
    private bool isImmune = false; // Tracks if the player is immune to damage
    public bool isBeingRevived = false;
    public float immunityDuration = 1.5f; // Duration of the immunity period
    private Player1Movement player1Movement;
    private Player2Movement player2Movement;
    private Animator animator;
    public GameObject revivalPrompt; // UI element for revival prompt
    private PlayerHealthSystem nearbyAlivePlayer;

    [SerializeField]
    Text reviveButtonText;

    [SerializeField]
    Image reviveButtonImage;

    // UI References
    public Image healthImage; // Reference to the health bar image
    public Text reviveText; // Reference to the "Requires Revive" text
    public Image reviveTextBackground; // Background for the revive text

    // Revive text fade parameters
    public float fadeDuration = 1f; // Duration of fade in/out
    public float minFontSize = 10f; // Minimum font size
    public float maxFontSize = 14f; // Maximum font size

    public bool IsAtMaxHealth
    {
        get { return currentHealth == maxHealth; }
    }

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

        // Initialize UI
        UpdateHealthUI();

        // Disable revive text initially
        if (reviveText != null)
        {
            reviveText.gameObject.SetActive(false);
        }
        if (reviveTextBackground != null)
        {
            reviveTextBackground.gameObject.SetActive(false);
        }
    }

    public void Reset(){
        dyingHealth = 20;
        isDead = false;
        isDowned = false;
        isImmune = false;
        isBeingRevived = false;
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

        // Initialize UI
        UpdateHealthUI();

        // Disable revive text initially
        if (reviveText != null)
        {
            reviveText.gameObject.SetActive(false);
        }
        if (reviveTextBackground != null)
        {
            reviveTextBackground.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (isDead){
            if (isPlayerOne){
                GameManager.player1Dead = true;
            }
            else{
                GameManager.player2Dead = true;
            }
            reviveText.text = "Dead";
        }
        // Check for game over
        if (currentHealth <= 0 && !isDowned)
        {
            EnterDownedState();
        }

        if (currentHealth <= 0 && isDowned && !isDead){
            UpdateHealthUI();
        }

        // Check for revival input
        if (nearbyAlivePlayer != null)
        {
            if (!nearbyAlivePlayer.isDead && nearbyAlivePlayer.isDowned && Input.GetKey(KeyCode.H))
            {
                StartRevival();
            }
        }
    }

    public void Heal(float amount)
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += amount;
        }
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        if (healthImage != null)
        {
            if (isDowned){
                float fillAmount = dyingHealth / maxHealth;

                healthImage.fillAmount = fillAmount;
                healthImage.color = new Color(1,0.001546457f,1);
                if (!isBeingRevived){
                    dyingHealth -= Time.deltaTime/2;
                    if (dyingHealth <= 0){
                        isDead = true;
                    }
                }
            }
            else{
                // Calculate the fill amount based on current health
                float fillAmount = currentHealth / maxHealth;

                // Update the fill amount directly
                healthImage.fillAmount = fillAmount;
                healthImage.color = new Color(1,1,1);
            }

            // Ensure the fill origin is correctly configured
            if (isPlayerOne)
            {
                healthImage.fillOrigin = (int)Image.OriginHorizontal.Left; // Fill from right for Player 1
            }
            else
            {
                healthImage.fillOrigin = (int)Image.OriginHorizontal.Right; // Fill from left for Player 2
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (
            other.CompareTag("Zombie")
            && !isDowned
            && !isImmune
            && !other.GetComponent<Animator>().GetBool("isDead")
        )
        {
            // Reduce health
            currentHealth--;
            // Update health UI
            UpdateHealthUI();
        }

        // Check for nearby downed player
        PlayerHealthSystem otherPlayerHealth = other.GetComponent<PlayerHealthSystem>();
        if (other.CompareTag("Player") && otherPlayerHealth != null && otherPlayerHealth.isDowned && !otherPlayerHealth.isDead)
        {
            revivalPrompt.gameObject.SetActive(true);
            reviveButtonText.gameObject.SetActive(true);
            reviveButtonImage.gameObject.SetActive(true);
            nearbyAlivePlayer = otherPlayerHealth;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealthSystem otherPlayerHealth = other.GetComponent<PlayerHealthSystem>();
            if (otherPlayerHealth != null && otherPlayerHealth.isDowned && !otherPlayerHealth.isDead)
            {
                revivalPrompt.gameObject.SetActive(false);
                reviveButtonImage.fillAmount = 1;
                reviveButtonText.gameObject.SetActive(false);
                reviveButtonImage.gameObject.SetActive(false);
                otherPlayerHealth.isBeingRevived = false;
                nearbyAlivePlayer = null;
            }
            revivalCount = 0;
        }
    }

    private void EnterDownedState()
    {
        isDowned = true;
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

        // Show revive text with fade effect
        StartCoroutine(ReviveTextFadeCoroutine());
    }

    private IEnumerator ReviveTextFadeCoroutine()
    {
        // Enable revive text and background
        if (reviveText != null)
        {
            reviveText.gameObject.SetActive(true);
        }
        if (reviveTextBackground != null)
        {
            reviveTextBackground.gameObject.SetActive(true);
        }

        // Continuous fade in/out effect
        while (isDowned)
        {
            // Fade in
            yield return StartCoroutine(FadeText(minFontSize, maxFontSize, fadeDuration));

            // Fade out
            yield return StartCoroutine(FadeText(maxFontSize, minFontSize, fadeDuration));
        }
    }

    private IEnumerator FadeText(float startSize, float endSize, float duration)
    {
        if (reviveText == null)
            yield break;

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            // Interpolate font size
            float currentSize = Mathf.Lerp(startSize, endSize, elapsedTime / duration);
            reviveText.fontSize = (int)currentSize;

            // Optional: Add alpha fade if needed
            // Color textColor = reviveText.color;
            // textColor.a = Mathf.Lerp(0.5f, 1f, elapsedTime / duration);
            // reviveText.color = textColor;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure final size is set
        reviveText.fontSize = (int)endSize;
    }

    private IEnumerator GrantImmunity()
    {
        UpdateHealthUI();
        isImmune = true; // Enable immunity

        yield return new WaitForSeconds(immunityDuration); // Wait for the immunity period

        isImmune = false; // Disable immunity\
    }

    public void StartRevival()
    {
        nearbyAlivePlayer.isBeingRevived = true;
        nearbyAlivePlayer.RevivePlayer();
    }

    private void RevivePlayer()
    {
        revivalCount += Time.deltaTime;
        reviveButtonImage.fillAmount = revivalCount / revivalTime;
        if (revivalCount >= revivalTime)
        {
            // Revive the player
            isDowned = false;
            isBeingRevived = false;
            dyingHealth = 20;
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
            reviveButtonText.gameObject.SetActive(false);
            reviveButtonImage.gameObject.SetActive(false);

            // Disable revive text
            if (reviveText != null)
            {
                reviveText.gameObject.SetActive(false);
            }
            if (reviveTextBackground != null)
            {
                reviveTextBackground.gameObject.SetActive(false);
            }

            // Grant immunity after revival
            StartCoroutine(GrantImmunity());
        }
    }
}