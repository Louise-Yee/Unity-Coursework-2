using UnityEngine;

public class ZombieMovement : MonoBehaviour
{
    public Transform[] players; // Array of player transforms to choose from
    public float speed = 1f; // Movement speed

    private Transform nearestPlayer;

    void Update()
    {
        if (players != null && players.Length > 0)
        {
            // Find the nearest player
            nearestPlayer = FindNearestPlayer();

            if (nearestPlayer != null)
            {
                // Move the zombie toward the nearest player
                transform.position = Vector2.MoveTowards(transform.position, nearestPlayer.position, speed * Time.deltaTime);

                // Optional: Rotate the zombie to face the target
                Vector2 direction = (nearestPlayer.position - transform.position).normalized;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle - 90); // Adjust rotation if needed
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
}

