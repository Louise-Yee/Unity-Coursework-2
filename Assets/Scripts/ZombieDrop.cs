using UnityEngine;

public class ZombieDrop : MonoBehaviour
{
    // Reference to the grenade prefab
    public GameObject grenadePrefab;

    // Reference to the health pickup prefab
    public GameObject healthPickupPrefab;

    // Drop position offset (slightly above or below the zombie's position)
    public Vector2 dropOffset = new Vector2(0, -0.5f);

    // Chance for health pickup to drop (25% chance)
    private float healthDropChance = 0.25f;

    void Start() { }

    // Method to drop the items (grenade and health pickup)
    public void DropItem()
    {
        if (grenadePrefab != null && gameObject.name == "PoweredZombie")
        {
            // Calculate drop position for 2D
            Vector2 dropPosition = (Vector2)transform.position + dropOffset;

            // Instantiate the grenade prefab at the calculated position
            Instantiate(grenadePrefab, dropPosition, Quaternion.identity);
        }

        // Randomly determine whether to drop a health pickup (25% chance)
        if (healthPickupPrefab != null && Random.value < healthDropChance)
        {
            // Calculate drop position for 2D
            Vector2 dropPosition = (Vector2)transform.position + dropOffset;

            // Instantiate the health pickup prefab at the calculated position
            Instantiate(healthPickupPrefab, dropPosition, Quaternion.identity);
        }
    }
}
