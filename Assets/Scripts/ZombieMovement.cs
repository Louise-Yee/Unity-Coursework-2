using UnityEngine;

public class ZombieMovement : MonoBehaviour
{
    public Transform target; // The player or target to follow
    public float speed = 1f; // Movement speed

    void Update()
    {
        if (target != null)
        {
            // Move the zombie toward the target
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

            // Optional: Rotate the zombie to face the target
            Vector2 direction = (target.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 90); // Adjust rotation if needed
        }
    }
}
