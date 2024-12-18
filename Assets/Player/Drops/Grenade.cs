using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float explosionDelay = 1.5f; // Time before the grenade explodes
    public float explosionRadius = 1f; // Radius of the explosion
    public int damage = 100; // Damage dealt to zombies
    public Animator animator; // Reference to the Animator component
    public Rigidbody2D rb; // Rigidbody for physics-based movement
    private bool hasExploded = false; // To prevent multiple explosions
    private SpriteRenderer spriteRenderer; // For the grenade's default sprite

    [Header("Sound Settings")]
    public AudioSource audioSource; // Reference to the AudioSource component
    public AudioClip explosionSound; // Explosion sound effect

    void Start()
    {
        // Get the Animator and SpriteRenderer components if not assigned
        if (animator == null)
            animator = GetComponent<Animator>();

        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        spriteRenderer = GetComponent<SpriteRenderer>();

        // Start the explosion countdown
        StartCoroutine(ExplodeAfterDelay());
    }

    private IEnumerator ExplodeAfterDelay()
    {
        // Wait for the explosion delay
        yield return new WaitForSeconds(explosionDelay);

        // Stop the grenade's movement immediately before explosion
        if (rb != null)
        {
            rb.velocity = Vector2.zero; // Stop horizontal movement
            rb.isKinematic = true; // Disable physics interactions
        }

        // Trigger the explosion animation
        if (animator != null)
        {
            // Try multiple methods to trigger the animation
            animator.SetTrigger("Explode");
            animator.Play("GrenadeExplode", 0, 0f);

            // Get the animation length
            RuntimeAnimatorController animatorController = animator.runtimeAnimatorController;
            AnimationClip[] clips = animatorController.animationClips;
            float animationLength = 0f;

            foreach (AnimationClip clip in clips)
            {
                if (clip.name == "GrenadeExplode")
                {
                    animationLength = clip.length;
                    break;
                }
            }

            // If no specific length found, use a default
            if (animationLength == 0f)
                animationLength = 0.5f;

            // Wait for the animation to play
            yield return new WaitForSeconds(animationLength);
        }
        else { }

        // Perform explosion logic
        Explode();
    }

    private void Explode()
    {
        if (hasExploded)
            return;
        hasExploded = true; // Ensure explosion happens only once

        if (audioSource != null && explosionSound != null)
        {
            audioSource.PlayOneShot(explosionSound);
        }
        else
        {
            Debug.LogWarning("AudioSource or ExplosionSound is missing!");
        }

        // Find all colliders within the explosion radius
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Zombie"))
            {
                // Attempt to get the zombieHealth script on the zombie
                zombieHealth zombie = hitCollider.GetComponent<zombieHealth>();

                if (zombie != null)
                {
                    // Call the TakeDamage function with the grenade's damage
                    zombie.TakeDamage(damage);
                }
                else { }
            }
        }

        // Destroy the grenade after the explosion
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the explosion radius in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}