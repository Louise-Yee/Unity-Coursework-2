using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [SerializeField] private GameObject player1Prefab;
    [SerializeField] private GameObject player2Prefab;
    [SerializeField] private GameObject[] zombiePrefab;
    public static int currentIndex = 0; // Track which prefab to spawn next
    private float spawnTimer = 0f;
    public float spawnInterval = 3f;
    public static float minSpawmTime = 2f;
    public static float maxSpawmTime = 4f;
    public int spawnCount = 15; // The number of zombies that should spawn
    public static int powerCount = 6; // The number of zombies that contains power
    public int temp1 = 0;
    public int temp2 = 0;
    private System.Random random = new System.Random(); // Random number generator
    private int westSpawnCounter = 0; // Counter for spawns in West areas
    private bool isCoolingDown = false; // Flag to check if spawner is in cooldown

    // Update is called once per frame
    void Update()
    {
        // make sure we have prefabs to spawn
        if (zombiePrefab.Length > 0 && temp1 < spawnCount)
        {
            // Update the spawn timer
            spawnTimer += Time.deltaTime;
            // Check if enough time has passed to spawn a zombie
            if (spawnTimer >= spawnInterval && !isCoolingDown)
            {
                // Spawn the current prefab
                GameObject newZombie = Instantiate(zombiePrefab[currentIndex]);
                newZombie.name = zombiePrefab[currentIndex].name;
                if (transform.name.Contains("North") || transform.name.Contains("South")){
                    // Change x value by a random number between -2 and 2
                    float randomXChange = Random.Range(-2f, 2f);
                    newZombie.transform.position = new Vector2(transform.position.x + randomXChange, transform.position.y);
                }
                else if (transform.name.Contains("West (1)") || transform.name.Contains("West (2)")){
                    newZombie.transform.position = new Vector2(transform.position.x, transform.position.y);
                    if (!GameManager.player1Dead && !GameManager.player2Dead){
                        westSpawnCounter++;

                        // Check if we've spawned 3 zombies in the West area
                        if (westSpawnCounter >= 3)
                        {
                            isCoolingDown = true; // Start cooldown period
                            Invoke("EndCooldown", 4f); // End cooldown after 4 seconds
                            westSpawnCounter = 0; // Reset counter for next batch
                        }
                    }
                }
                else{
                    // Change x value by a random number between -2 and 2
                    float randomYChange = Random.Range(-2f, 2f);
                    newZombie.transform.position = new Vector2(transform.position.x, transform.position.y + randomYChange);  
                }

                // Assign a script or movement logic to make the zombie move toward the player
                ZombieMovement movementScript = newZombie.AddComponent<ZombieMovement>();
                // movementScript.target = playerPrefab.transform;
                movementScript.players = new Transform[]
                {
                    player1Prefab.transform,
                    player2Prefab.transform
                };

                // Randomly paint some zombies red
                if (random.Next(0, 5) == 1 && temp2 < powerCount)
                {
                    SpriteRenderer spriteRenderer = newZombie.GetComponent<SpriteRenderer>();
                    if (spriteRenderer != null)
                    {
                        spriteRenderer.color = Color.red; // Paint the zombie red
                    }
                    newZombie.name = "PoweredZombie";
                    newZombie.GetComponent<zombieHealth>().maxHealth = 2;
                    temp2+=1;
                }

                // Reset the spawn timer
                spawnTimer = 0f;
                //tracks the number of zombies spawning
                temp1+=1;
                spawnInterval = Random.Range(minSpawmTime, maxSpawmTime);
            }
        }
    }

    private void EndCooldown()
    {
        isCoolingDown = false; // End cooldown period, allowing spawning again
    }
}
