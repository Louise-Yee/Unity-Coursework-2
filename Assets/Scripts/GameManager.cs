using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] Animator transitionAnim;
    [SerializeField] GameObject player1;
    [SerializeField] GameObject player2;
    [SerializeField] GameObject player1UI;
    [SerializeField] GameObject player2UI;
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject rural;
    [SerializeField] GameObject city;
    [SerializeField] GameObject rooftop;
    [SerializeField] GameObject zombieSpawnNorth;
    [SerializeField] GameObject zombieSpawnSouth;
    [SerializeField] GameObject zombieSpawnWest;
    [SerializeField] GameObject zombieSpawnEast;
    [SerializeField] Text player1killed;
    [SerializeField] Text player2killed;
    public static int zombiesKilled = 0;
    public static int p1ZombieKilled = 0;
    public static int p2ZombieKilled = 0;
    void Start(){
        player1.SetActive(false);
        player2.SetActive(false);
        player1UI.SetActive(false);
        player2UI.SetActive(false);
        mainMenu.SetActive(true);
        rural.SetActive(false);
        city.SetActive(false);
        rural.SetActive(false);
        zombieSpawnNorth.SetActive(false);
        zombieSpawnSouth.SetActive(false);
        zombieSpawnWest.SetActive(false);
        zombieSpawnEast.SetActive(false);
        transitionAnim.gameObject.SetActive(false);
    }

    public void ClickedPlay(){
        transitionAnim.gameObject.SetActive(true);
        player1.SetActive(true);
        player2.SetActive(true);
        player1UI.SetActive(true);
        player2UI.SetActive(true);
        mainMenu.SetActive(false);
        rural.SetActive(true);
        zombieSpawnNorth.SetActive(false);
        zombieSpawnSouth.SetActive(false);
        zombieSpawnWest.SetActive(true);
        zombieSpawnEast.SetActive(true);
    }
    
    void Update(){
        // for now, press F after all the zombies in the first round are eliminated
        if (Input.GetKey(KeyCode.F) && zombiesKilled==ZombieSpawner.spawnCount){
            NextLevel1();
        }
        // for now, press J after all the zombies in the second round are eliminated
        if (Input.GetKey(KeyCode.J) && zombiesKilled==ZombieSpawner.spawnCount){
            NextLevel2();
        }
        player1killed.text = "Zombies killed: "+p1ZombieKilled;
        player2killed.text = "Zombies killed: "+p2ZombieKilled;
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
