using System.Collections;
using UnityEngine;

public class ZombieMovement : MonoBehaviour
{
    public Transform[] players; // Array of player transforms to choose from
    public float speed = 1f; // Movement speed
    public float avoidanceRadius = 0.2f; // Radius for avoiding other zombies

    private Transform nearestPlayer;
    public bool isLeft;
    private Animator animator; // Reference to the Animator component
    private bool isPushedAway = false; // Track if the zombie is currently pushed away
    void Start()
    {
        // Get the Animator component attached to this GameObject
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (players != null && players.Length > 0 && !isPushedAway && !animator.GetBool("isDead"))
        {
            // Find the nearest player
            nearestPlayer = FindNearestPlayer();

            if (nearestPlayer != null)
            {
                // Calculate the direction towards the nearest player
                Vector2 direction = (nearestPlayer.position - transform.position).normalized;

                // Avoid other zombies
                Vector2 avoidanceDirection = AvoidOtherZombies();

                // Combine the player direction with avoidance direction
                Vector2 finalDirection = (direction + avoidanceDirection).normalized;

                // Move the zombie toward the target while avoiding others
                transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + finalDirection, speed * Time.deltaTime);

                // Check if the zombie is moving left or right based on the x component of the direction
                isLeft = direction.x < 0;
                
                // Set the animator parameter based on isLeft
                animator.SetBool("isLeft", isLeft);
            }
        }
    }

    private Transform FindNearestPlayer()
    {
        Transform closest = null;
        float minDistance = float.MaxValue;

        foreach (var player in players)
        {
            if (player != null)
            {
                if (player.gameObject.GetComponent<PlayerHealthSystem>().isDead){
                    continue;
                }
                float distance = Vector2.Distance(transform.position, player.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = player;
                }
            }
        }

        return closest;
    }

    private Vector2 AvoidOtherZombies()
    {
        Vector2 avoidanceDirection = Vector2.zero;
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, avoidanceRadius);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Zombie") && hitCollider.transform != transform)
            {
                Vector2 directionAwayFromZombie = (transform.position - hitCollider.transform.position).normalized;
                avoidanceDirection += directionAwayFromZombie;
            }
        }

        return avoidanceDirection.normalized; // Normalize to ensure consistent movement speed
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(PushAwayFromPlayer(other.transform));
        }
    }

    private IEnumerator PushAwayFromPlayer(Transform player)
    {
        if (animator.GetBool("isLeft")){
            animator.SetBool("touchedLeft", true);
        }
        else{
            animator.SetBool("touchedRight", true);
        }
        isPushedAway = true;

        // Calculate push direction away from player
        Vector2 pushDirection = (transform.position - player.position).normalized;

        // Apply push force for 0.1 seconds
        float pushDuration = 0.1f;
        float elapsedTime = 0f;

        while (elapsedTime < pushDuration)
        {
            transform.position += (Vector3)pushDirection * speed * Time.deltaTime;
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Wait for 1 second to allow the animation to play
        yield return new WaitForSeconds(1f);

        // Reset animation state
        if (animator.GetBool("isLeft"))
        {
            animator.SetBool("touchedLeft", false);
        }
        else
        {
            animator.SetBool("touchedRight", false);
        }

        isPushedAway = false; // Allow chasing again after pushing away
    }
}

