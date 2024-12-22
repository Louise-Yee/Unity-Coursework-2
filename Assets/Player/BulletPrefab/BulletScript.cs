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
            if (dropCollider.CompareTag("GrenadeDrop") || dropCollider.CompareTag("HealthDrop"))
            {
                Physics2D.IgnoreCollision(bulletCollider, dropCollider);
            }
        }

        // Destroy the bullet after the specified lifetime
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Zombie"))
        {
            zombieHealth zombieHealth = collision.GetComponent<zombieHealth>();

            if (zombieHealth != null && !zombieHealth.IsDead())
            {
                if (gameObject.name.Contains("Bullet2")){
                    GameManager.p2ZombieKilled++;
                }
                else{
                    GameManager.p1ZombieKilled++;
                }
                // Only damage and destroy the bullet if the zombie is not dead
                zombieHealth.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
        else if (!collision.CompareTag("Player")) // Don't destroy on player collision
        {
            Destroy(gameObject); // Destroy the bullet on any other collision
        }
    }
}
