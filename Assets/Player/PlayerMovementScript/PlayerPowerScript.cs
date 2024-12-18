using UnityEngine;
using UnityEngine.UI; // Added for UI components

public class PlayerInventory : MonoBehaviour
{
    // Maximum number of grenades the player can hold
    private const int maxGrenades = 3;

    // Current number of grenades
    private int grenadeCount = 3;

    // Player identifier
    public int playerID = 1; // Set this in the inspector or dynamically

    // Reference to the player's movement script
    private MonoBehaviour playerMovement;

    private PlayerHealthSystem playerHealth;

    // Reference to the grenade prefab
    public GameObject grenadePrefab;

    // Grenade throw force
    public float throwForce = 2f;

    // UI References
    public Image[] grenadeImages; // Array of grenade images

    // UI or debugging log to show grenade count (optional)
    void UpdateUI()
    {
        // Iterate through all grenade images
        for (int i = 0; i < grenadeImages.Length; i++)
        {
            if (i < grenadeCount)
            {
                // Grenades the player has: full opacity
                grenadeImages[i].color = new Color(
                    grenadeImages[i].color.r,
                    grenadeImages[i].color.g,
                    grenadeImages[i].color.b,
                    1f
                );
            }
            else
            {
                // Grenades the player doesn't have: 50% opacity
                grenadeImages[i].color = new Color(
                    grenadeImages[i].color.r,
                    grenadeImages[i].color.g,
                    grenadeImages[i].color.b,
                    0.5f
                );
            }
        }
    }

    // Method to pick up a grenade
    public void PickupGrenade()
    {
        if (grenadeCount < maxGrenades)
        {
            grenadeCount++;
            UpdateUI();
        }
        else { }
    }

    // Method to use a grenade
    public void UseGrenade()
    {
        if (grenadeCount > 0)
        {
            grenadeCount--;
            UpdateUI();

            // Get the direction from the movement script
            Vector2 throwDirection = Vector2.zero;
            if (playerMovement is Player1Movement player1Movement)
            {
                throwDirection = player1Movement.GetMoveInput();
                if (throwDirection == Vector2.zero)
                {
                    throwDirection = player1Movement.lastMoveDirection;
                }
            }
            else if (playerMovement is Player2Movement player2Movement)
            {
                throwDirection = player2Movement.GetMoveInput();
                if (throwDirection == Vector2.zero)
                {
                    throwDirection = player2Movement.lastMoveDirection;
                }
            }

            if (grenadePrefab == null)
            {
                return;
            }

            if (throwDirection != Vector2.zero)
            {
                // Instantiate and throw the grenade
                GameObject grenade = Instantiate(
                    grenadePrefab,
                    transform.position,
                    Quaternion.identity
                );
                Rigidbody2D grenadeRb = grenade.GetComponent<Rigidbody2D>();

                if (grenadeRb != null)
                {
                    grenadeRb.velocity = throwDirection.normalized * throwForce;
                }
                else { }
            }
            else { }
        }
        else { }
    }

    // Method to heal the player
    public void HealPlayer(float healAmount)
    {
        if (playerHealth != null)
        {
            playerHealth.Heal(healAmount);
        }
    }

    // Trigger to simulate picking up a grenade or health item
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("GrenadeDrop"))
        {
            if (grenadeCount < maxGrenades)
            {
                PickupGrenade();
                Destroy(other.gameObject); // Remove the grenade pickup from the scene
            }
            else { }
        }

        // when player is not at max health then heals
        if (other.CompareTag("HealthDrop") && !playerHealth.IsAtMaxHealth)
        {
            // Heal the player but don't exceed the maximum health
            float healAmount = 4f; // Example heal amount, adjust based on your needs
            HealPlayer(healAmount);

            Destroy(other.gameObject); // Remove the health pickup from the scene
        }
    }

    // Initialize the player movement script based on player ID
    void Start()
    {
        if (playerID == 1)
        {
            playerMovement = GetComponent<Player1Movement>();
        }
        else if (playerID == 2)
        {
            playerMovement = GetComponent<Player2Movement>();
        }

        if (playerMovement == null) { }

        playerHealth = GetComponent<PlayerHealthSystem>();

        // Initialize UI on start
        UpdateUI();
    }

    // Update method to check for player-specific inputs
    void Update()
    {
        if (playerID == 1) // player 1
        {
            if (Input.GetKeyDown(KeyCode.G) && !playerHealth.isDowned) // press g and player is not downed
            {
                UseGrenade();
            }
        }
        else if (playerID == 2)
        {
            if (Input.GetKeyDown(KeyCode.Comma) && !playerHealth.isDowned) // press comma and player is not downed
            {
                UseGrenade();
            }
        }
    }
}
