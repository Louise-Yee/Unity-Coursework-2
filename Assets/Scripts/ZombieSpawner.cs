using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [SerializeField] private GameObject player1Prefab;
    [SerializeField] private GameObject player2Prefab;
    [SerializeField] private GameObject[] zombiePrefab;
    public static int currentIndex = 0; // Track which prefab to spawn next
    private float spawnTimer = 0f;
    public static float spawnInterval = 3f;

    // Update is called once per frame
    void Update()
    {
        // make sure we have prefabs to spawn
        if (zombiePrefab.Length > 0)
        {
            // Update the spawn timer
            spawnTimer += Time.deltaTime;
            // Check if enough time has passed to spawn a zombie
            if (spawnTimer >= spawnInterval)
            {
                // Spawn the current prefab
                GameObject newZombie = Instantiate(zombiePrefab[currentIndex]);
                newZombie.name = zombiePrefab[currentIndex].name;

                newZombie.transform.position = new Vector2(transform.position.x, transform.position.y);

                // Assign a script or movement logic to make the zombie move toward the player
                ZombieMovement movementScript = newZombie.AddComponent<ZombieMovement>();
                // movementScript.target = playerPrefab.transform;
                movementScript.players = new Transform[]
                {
                    player1Prefab.transform,
                    player2Prefab.transform
                };

                // Reset the spawn timer
                spawnTimer = 0f;
            }
        }
    }
}
