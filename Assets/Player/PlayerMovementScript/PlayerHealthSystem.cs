using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthSystem : MonoBehaviour
{
    public float maxHealth = 10; // Maximum health
    private float currentHealth; // Current health
    public float pushForce = 2f; // Force to push the zombie away
    private float damageCooldown = 1f; // Delay between consecutive damage events from the same zombie
    private Dictionary<GameObject, float> damageCooldowns = new Dictionary<GameObject, float>(); // Tracks cooldowns per zombie
    private float revivalTime = 5f; // Time required to revive the player
    private float revivalCount = 0f;
    public int reviveHealth = 5; // Health restored upon revival
    public bool isPlayerOne;
    public bool isDowned = false; // Tracks if the player is downed
    private bool isImmune = false; // Tracks if the player is immune to damage
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
    private Coroutine decreaseCoroutine;

    // UI References
    public Image healthImage; // Reference to the health bar image
    public Text reviveText; // Reference to the "Requires Revive" text
    public Image reviveTextBackground; // Background for the revive text

    // Revive text fade parameters
    public float fadeDuration = 1f; // Duration of fade in/out
    public float minFontSize = 15f; // Minimum font size
    public float maxFontSize = 20f; // Maximum font size

    public bool IsAtMaxHealth
    {
        get { return currentHealth == maxHealth; }
    }

    [Header("Audio")]
    public AudioSource reviveAudioSource; // Reference to AudioSource component
    public AudioSource takeDamageAudioSource; // Reference to AudioSource component
    public AudioClip[] hurtSounds; // The revival sound effect

    public AudioClip reviveSound; // The revival sound effect
    public float minPitch = 0.5f; // Minimum pitch for the revival sound
    public float maxPitch = 1.5f; // Maximum pitch for the revival sound

    private bool isReviving = false; // Track if currently reviving
    private Coroutine reviveCoroutine; // Store the revive coroutine

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
        revivalPrompt.SetActive(false);

        // Initialize UI
        UpdateHealthUI();

        // Initialize audio source if not set
        if (reviveAudioSource == null)
        {
            reviveAudioSource = gameObject.AddComponent<AudioSource>();
            reviveAudioSource.playOnAwake = false;
            reviveAudioSource.loop = true;
        }

        if (takeDamageAudioSource == null)
        {
            takeDamageAudioSource = gameObject.AddComponent<AudioSource>();
            takeDamageAudioSource.playOnAwake = false;
        }

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
        if (currentHealth <= 0 && !isDowned)
        {
            EnterDownedState();
        }

        // Update the cooldown timer for each zombie
        List<GameObject> zombiesToReset = new List<GameObject>();
        foreach (var zombie in damageCooldowns.Keys)
        {
            damageCooldowns[zombie] -= Time.deltaTime;
            if (damageCooldowns[zombie] <= 0)
            {
                zombiesToReset.Add(zombie); // Mark zombies whose cooldown has expired
            }
        }

        // Remove zombies with expired cooldowns
        foreach (var zombie in zombiesToReset)
        {
            damageCooldowns.Remove(zombie);
        }

        // Check for revival input
        if (nearbyAlivePlayer != null)
        {
            if (nearbyAlivePlayer.isDowned)
            {
                if (Input.GetKeyDown(KeyCode.H) && !isReviving)
                {
                    // Start reviving
                    isReviving = true;
                    if (reviveCoroutine != null)
                    {
                        StopCoroutine(reviveCoroutine);
                    }
                    reviveCoroutine = StartCoroutine(ReviveCoroutine());

                    // Start playing the sound
                    if (reviveSound != null)
                    {
                        reviveAudioSource.clip = reviveSound;
                        reviveAudioSource.Play();
                    }
                }
                else if (Input.GetKeyUp(KeyCode.H) && isReviving)
                {
                    StopReviving(); // Replace previous stop logic with centralized method

                    // Start decreasing
                    if (decreaseCoroutine == null)
                    {
                        decreaseCoroutine = StartCoroutine(GradualReviveDecrease());
                    }
                }
            }
        }
        else if (isReviving) // Add this check to stop reviving if nearby player becomes null
        {
            StopReviving();
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
            // Calculate the fill amount based on current health
            float fillAmount = (float)currentHealth / maxHealth;

            // Update the fill amount directly
            healthImage.fillAmount = fillAmount;

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
            // Check if the zombie is not in cooldown or if its cooldown has expired
            if (
                !damageCooldowns.ContainsKey(other.gameObject)
                || damageCooldowns[other.gameObject] <= 0
            )
            {
                if (hurtSounds != null && hurtSounds.Length > 0)
                {
                    AudioClip randomHurtSound = hurtSounds[Random.Range(0, hurtSounds.Length)];
                    takeDamageAudioSource.PlayOneShot(randomHurtSound);
                }
                // Reduce health
                currentHealth--;
                UpdateHealthUI();

                // Reset/Add zombie to cooldown tracker
                damageCooldowns[other.gameObject] = damageCooldown;
            }
        }

        // Check for nearby downed player
        PlayerHealthSystem otherPlayerHealth = other.GetComponent<PlayerHealthSystem>();
        if (other.CompareTag("Player") && otherPlayerHealth != null && otherPlayerHealth.isDowned)
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
            if (otherPlayerHealth != null && otherPlayerHealth.isDowned)
            {
                revivalPrompt.gameObject.SetActive(false);
                StopReviving(); // Add this call to handle stopping revival properly

                // Start decreasing the revive progress bar
                if (decreaseCoroutine == null)
                {
                    decreaseCoroutine = StartCoroutine(GradualReviveDecrease());
                }
                nearbyAlivePlayer = null;
            }
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
        animator.SetBool("isDead", true);

        // Show revive text with fade effect
        StartCoroutine(ReviveTextFadeCoroutine());
    }

    private void StopReviving()
    {
        isReviving = false;

        // Stop any ongoing revival coroutine
        if (reviveCoroutine != null)
        {
            StopCoroutine(reviveCoroutine);
            reviveCoroutine = null;
        }

        // Stop and reset audio
        if (reviveAudioSource != null)
        {
            reviveAudioSource.Stop();
            reviveAudioSource.pitch = minPitch;
        }

        // Stop any ongoing decrease coroutine
        if (decreaseCoroutine != null)
        {
            StopCoroutine(decreaseCoroutine);
            decreaseCoroutine = null;
        }
    }

    public void StartRevival()
    {
        nearbyAlivePlayer.RevivePlayer();
    }

    private void RevivePlayer()
    {
        revivalCount += Time.deltaTime;
        reviveButtonImage.fillAmount = revivalCount / revivalTime;

        // Debug.Log(revivalCount+", "+revivalTime);
        if (revivalCount >= revivalTime)
        {
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

            animator.SetBool("isDead", false);
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

    private IEnumerator GradualReviveDecrease()
    {
        float decreaseRate = 1f / revivalTime; // Decrease over the same duration as revival

        while (reviveButtonImage.fillAmount > 0 && !isReviving)
        {
            reviveButtonImage.fillAmount -= decreaseRate * Time.deltaTime;
            revivalCount = reviveButtonImage.fillAmount * revivalTime;
            yield return null;
        }

        if (!isReviving)
        {
            revivalCount = 0;
            if (reviveButtonText != null)
                reviveButtonText.gameObject.SetActive(false);
            if (reviveButtonImage != null)
                reviveButtonImage.gameObject.SetActive(false);
        }

        decreaseCoroutine = null;
    }

    private IEnumerator ReviveCoroutine()
    {
        while (isReviving && revivalCount < revivalTime)
        {
            revivalCount += Time.deltaTime;
            float progress = revivalCount / revivalTime;
            reviveButtonImage.fillAmount = progress;

            // Update sound pitch based on progress
            if (reviveAudioSource != null && reviveAudioSource.isPlaying)
            {
                reviveAudioSource.pitch = Mathf.Lerp(minPitch, maxPitch, progress);
            }

            if (progress >= 1f)
            {
                nearbyAlivePlayer.RevivePlayer();
                isReviving = false;
                reviveAudioSource.Stop();
                break;
            }

            yield return null;
        }
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
}
