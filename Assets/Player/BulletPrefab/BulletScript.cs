using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifetime = 2f;
    public float damage = 1f; // Amount of damage each bullet does

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Zombie"))
        {
            // Debug.Log("Hit Zombie!");
            // If you have a health system on the zombie, you can damage it here
            zombieHealth zombieHealth = collision.GetComponent<zombieHealth>();
            if (zombieHealth != null)
            {
                zombieHealth.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        else if (!collision.CompareTag("Player")) // Don't destroy on player collision
        {
            // Debug.Log($"Hit non-zombie object: {collision.gameObject.name}");
            Destroy(gameObject);
        }
    }
}