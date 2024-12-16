using System.Collections;
using System.Collections.Generic;
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
    public Image grenadeImage; // Reference to the grenade image in UI
    public Text grenadeCountText; // Reference to the grenade count text in UI

    // UI or debugging log to show grenade count (optional)
    void UpdateUI()
    {
        // Update grenade count text
        if (grenadeCountText != null)
        {
            grenadeCountText.text = grenadeCount.ToString();
        }

        // Update grenade image opacity
        if (grenadeImage != null)
        {
            // Set opacity to 50% when no grenades, 100% otherwise
            grenadeImage.color =
                grenadeCount > 0
                    ? new Color(
                        grenadeImage.color.r,
                        grenadeImage.color.g,
                        grenadeImage.color.b,
                        1f
                    )
                    : new Color(
                        grenadeImage.color.r,
                        grenadeImage.color.g,
                        grenadeImage.color.b,
                        0.5f
                    );
        }

        Debug.Log("Player " + playerID + " Grenades: " + grenadeCount);
    }

    // Method to pick up a grenade
    public void PickupGrenade()
    {
        if (grenadeCount < maxGrenades)
        {
            grenadeCount++;
            UpdateUI();
        }
        else
        {
            Debug.Log("Player " + playerID + " Maximum grenades reached!");
        }
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
                Debug.LogError("Grenade prefab is not assigned to PlayerInventory.");
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
                    Debug.Log(
                        "Player " + playerID + " Grenade thrown in direction: " + throwDirection
                    );
                }
                else
                {
                    Debug.LogError("Grenade prefab is missing a Rigidbody2D component.");
                }
            }
            else
            {
                Debug.Log("Player " + playerID + " is not moving. Grenade not thrown.");
            }
        }
        else
        {
            Debug.Log("Player " + playerID + " No grenades to use!");
        }
    }

    // Trigger to simulate picking up a grenade
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GrenadePickup"))
        {
            PickupGrenade();
            Destroy(other.gameObject); // Remove the grenade pickup from the scene
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

        if (playerMovement == null)
        {
            Debug.LogError("Player " + playerID + " Movement script not found!");
        }

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
