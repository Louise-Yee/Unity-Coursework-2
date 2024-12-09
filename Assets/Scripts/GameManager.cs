using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Animator transitionAnim;
    [SerializeField] GameObject rural;
    [SerializeField] GameObject city;
    [SerializeField] GameObject zombieSpawnNorth;
    [SerializeField] GameObject zombieSpawnSouth;
    [SerializeField] GameObject zombieSpawnWest;
    [SerializeField] GameObject zombieSpawnEast;
    public static int zombiesKilled = 0;
    void Start(){
        city.SetActive(false);
        rural.SetActive(true);
        zombieSpawnNorth.SetActive(false);
        zombieSpawnSouth.SetActive(true);
        zombieSpawnWest.SetActive(true);
        zombieSpawnEast.SetActive(true);
    }
    
    void Update(){
        // for now, press G after all the zombies in the first round are eliminated
        if (Input.GetKey(KeyCode.G) && zombiesKilled==ZombieSpawner.spawnCount){
            NextLevel();
        }
    }
    public void NextLevel(){
        StartCoroutine(LoadLevel());
    }
    IEnumerator LoadLevel(){
        transitionAnim.SetTrigger("End");
        yield return new WaitForSeconds(1);
        rural.SetActive(false);
        city.SetActive(true);
        zombieSpawnNorth.SetActive(true);
        ZombieSpawner.spawnCount = 136;
        ZombieSpawner.temp = 0;
        zombiesKilled = 0;
        transitionAnim.SetTrigger("Start");
    }
}
