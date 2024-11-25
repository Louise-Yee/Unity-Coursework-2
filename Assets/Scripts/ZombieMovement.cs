using UnityEngine;

public class ZombieMovement : MonoBehaviour
{
    public Transform[] players; // Array of player transforms to choose from
    public float speed = 1f; // Movement speed
    public float avoidanceRadius = 0.2f; // Radius for avoiding other zombies

    private Transform nearestPlayer;
    public bool isLeft;
    private Animator animator; // Reference to the Animator component
    void Start()
    {
        // Get the Animator component attached to this GameObject
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (players != null && players.Length > 0)
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
}

