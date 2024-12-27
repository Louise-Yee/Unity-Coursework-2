using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifetime = 2f;
    public float damage = 1f; // Amount of damage each bullet does

    void Start()
    {
        Collider2D bulletCollider = GetComponent<Collider2D>();
        Collider2D[] dropColliders = FindObjectsOfType<Collider2D>();

        // Ignore collisions with grenade and health pickups
        foreach (Collider2D dropCollider in dropColliders)
        {
            if (dropCollider.CompareTag("GrenadeDrop") || dropCollider.CompareTag("HealthDrop") || dropCollider.CompareTag("GearDrop"))
            {
                Physics2D.IgnoreCollision(bulletCollider, dropCollider);
            }
        }

        // Destroy the bullet after the specified lifetime
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Zombie") && (collision is CapsuleCollider2D))
        {
            zombieHealth zombieHealth = collision.GetComponent<zombieHealth>();

            if (zombieHealth != null && !zombieHealth.IsDead() && zombieHealth.currentHealth!=0)
            {
                // Only damage and destroy the bullet if the zombie is not dead
                zombieHealth.TakeDamage(damage,gameObject.name);
                Destroy(gameObject);
            }
        }
        else if (!collision.CompareTag("Player")) // Don't destroy on player collision
        {
            Destroy(gameObject); // Destroy the bullet on any other collision
        }
    }
}
