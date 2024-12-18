using UnityEngine;

public class zombieHealth : MonoBehaviour
{
    public float maxHealth = 1f;
    private float currentHealth;
    private Animator animator; // Reference to the Animator component
    private ZombieDrop zombieDrop; // Reference to ZombieDrop script
    private Collider2D zombieCollider; // Reference to the Zombie's Collider2D

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        zombieDrop = GetComponent<ZombieDrop>(); // Get the ZombieDrop component
        zombieCollider = GetComponent<Collider2D>(); // Get the Zombie's Collider2D component
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        animator.SetBool("isDead", true);
        if (zombieCollider != null)
        {
            zombieCollider.enabled = false;
        }
        if (zombieDrop != null)
        {
            zombieDrop.DropItem(); // Trigger the drop
        }
        Destroy(gameObject, 1f); // Destroy the zombie after 1 second
        GameManager.zombiesKilled++;
    }

    // Getter to check if the zombie is dead
    public bool IsDead()
    {
        return animator.GetBool("isDead");
    }
}
