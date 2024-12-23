using UnityEngine;

public class VehicleDrop : MonoBehaviour
{
    // Reference to the grenade prefab
    public GameObject gearPrefab;
    // Drop position offset (slightly above or below the zombie's position)
    public Vector2 dropOffset = new Vector2(0, -0.5f);

    void Start() { }

    // Method to drop the items (grenade and health pickup)
    public void DropItem()
    {
        if (gearPrefab != null)
        {
            // Calculate drop position for 2D
            Vector2 dropPosition = (Vector2)transform.position + dropOffset;

            // Instantiate the grenade prefab at the calculated position
            Instantiate(gearPrefab, dropPosition, Quaternion.identity);
        }
    }
}
