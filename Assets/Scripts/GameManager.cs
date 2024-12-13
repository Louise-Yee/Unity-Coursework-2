using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Animator transitionAnim;
    [SerializeField] GameObject rural;
    [SerializeField] GameObject city;
    [SerializeField] GameObject rooftop;
    [SerializeField] GameObject zombieSpawnNorth;
    [SerializeField] GameObject zombieSpawnSouth;
    [SerializeField] GameObject zombieSpawnWest;
    [SerializeField] GameObject zombieSpawnEast;
    public static int zombiesKilled = 0;
    void Start(){
        city.SetActive(false);
        rural.SetActive(true);
        zombieSpawnNorth.SetActive(false);
        zombieSpawnSouth.SetActive(false);
        zombieSpawnWest.SetActive(true);
        zombieSpawnEast.SetActive(true);
    }
    
    void Update(){
        // for now, press G after all the zombies in the first round are eliminated
        if (Input.GetKey(KeyCode.G) && zombiesKilled==ZombieSpawner.spawnCount){
            NextLevel1();
        }
        // for now, press G after all the zombies in the second round are eliminated
        if (Input.GetKey(KeyCode.H) && zombiesKilled==ZombieSpawner.spawnCount){
            NextLevel2();
        }
    }
    public void NextLevel1(){
        StartCoroutine(LoadLevel1());
    }
    IEnumerator LoadLevel1(){
        transitionAnim.SetTrigger("End");
        yield return new WaitForSeconds(1);
        rural.SetActive(false);
        city.SetActive(true);
        zombieSpawnNorth.SetActive(true);
        zombieSpawnSouth.SetActive(true);
        ZombieSpawner.spawnCount = 160;
        ZombieSpawner.powerCount = 12;
        ZombieSpawner.temp1 = 0;
        ZombieSpawner.temp2 = 0;
        zombiesKilled = 0;
        transitionAnim.SetTrigger("Start");
    }
    public void NextLevel2(){
        StartCoroutine(LoadLevel2());
    }
    IEnumerator LoadLevel2(){
        transitionAnim.SetTrigger("End");
        yield return new WaitForSeconds(1);
        city.SetActive(false);
        rooftop.SetActive(true);
        zombieSpawnEast.SetActive(false);
        ZombieSpawner.spawnCount = 80;
        ZombieSpawner.powerCount = 12;
        ZombieSpawner.temp1 = 0;
        ZombieSpawner.temp2 = 0;
        zombiesKilled = 0;
        transitionAnim.SetTrigger("Start");
    }
}
