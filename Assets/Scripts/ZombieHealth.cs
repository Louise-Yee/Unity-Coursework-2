using UnityEngine;

public class zombieHealth : MonoBehaviour
{
    public float maxHealth = 1f;
    private float currentHealth;
    private Animator animator; // Reference to the Animator component

    void Start()
    {
        currentHealth = maxHealth;
        // Get the Animator component attached to this GameObject
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage. Health: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log($"{gameObject.name} has died!");
        animator.SetBool("isDead", true);
        Destroy(gameObject,1f);
    }
}