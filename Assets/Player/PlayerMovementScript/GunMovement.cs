using UnityEngine;

public class GunMovement : MonoBehaviour
{
    public Transform player; // The player transform to follow
    public float rotationSpeed = 10f; // Speed of rotation
    private Vector2 currentDirection; // Direction of movement or shooting

    void Update()
    {
        // Get the current direction of movement or shooting
        if (player != null)
        {
            // Use the ShootingScript's direction or the movement direction
            var shootingScript = player.GetComponent<ShootingScript>();
            if (shootingScript != null)
            {
                currentDirection = shootingScript.currentDirection;
            }
        }

        // If there's a direction (i.e., player is moving or shooting), rotate the gun
        if (currentDirection != Vector2.zero)
        {
            // Calculate the angle to face the movement direction (or shooting direction)
            float angle = Mathf.Atan2(currentDirection.y, currentDirection.x) * Mathf.Rad2Deg;

            // Smoothly rotate the gun to face the movement or shooting direction
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                Quaternion.Euler(0f, 0f, angle - 90f), // Adjust the angle for 2D orientation
                rotationSpeed * Time.deltaTime
            );
        }
    }
}
