using UnityEngine;

public class vehicleHealth : MonoBehaviour
{
    private float maxHealth = 4f;
    private float currentHealth;
    private VehicleDrop vehicleDrop; // Reference to VehicleDrop script
    private Collider2D vehicleCollider; // Reference to the Vehicle's Collider2D
    [SerializeField] GameObject smoke;
    [SerializeField] GameObject scratch;

    void Start()
    {
        smoke.SetActive(false);
        scratch.SetActive(false);
        currentHealth = maxHealth;
        vehicleDrop = GetComponent<VehicleDrop>(); // Get the ZombieDrop component
        vehicleCollider = GetComponent<Collider2D>(); // Get the Zombie's Collider2D component
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth == 2){
            smoke.SetActive(true);
        }
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (vehicleCollider != null)
        {
            vehicleCollider.enabled = false;
        }
        if (vehicleDrop != null)
        {
            Debug.Log("Car destroyed");
            vehicleDrop.DropItem(); // Trigger the drop
        }
        if (scratch != null){
            scratch.SetActive(true);
        }
        Destroy(gameObject); // Destroy the zombie after 1 second
    }
}
