using UnityEngine;

public class zombieHealth : MonoBehaviour
{
    public float maxHealth = 1f;
    public float currentHealth;
    private Animator animator; // Reference to the Animator component
    private ZombieDrop zombieDrop; // Reference to ZombieDrop script
    public Collider2D zombieCollider; // Reference to the Zombie's Collider2D

    [Header("Audio")]
    public AudioClip zombieHitSound; // Sound when zombie dies
    private AudioSource audioSource; // Reference to AudioSource component

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        zombieDrop = GetComponent<ZombieDrop>(); // Get the ZombieDrop component
        zombieCollider = GetComponent<Collider2D>(); // Get the Zombie's Collider2D component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void TakeDamage(float damage, string obj)
    {
        currentHealth -= damage;
        PlayAudio(zombieHitSound);
        if (currentHealth <= 0)
        {
            if (obj.Contains("Bullet2") || obj.Contains("GrenadePrefab2")){
                GameManager.p2ZombieKilled++;
            }
            else if (obj.Contains("Bullet") || obj.Contains("GrenadePrefab")){
                GameManager.p1ZombieKilled++;
            }
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
    }

    // Getter to check if the zombie is dead
    public bool IsDead()
    {
        return animator.GetBool("isDead");
    }

    private void PlayAudio(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
