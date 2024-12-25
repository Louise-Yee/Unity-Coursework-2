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
    [SerializeField] GameObject player2GearPanel;
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
        rooftop.SetActive(false);
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
        player2GearPanel.SetActive(false);
        mainMenu.SetActive(false);
        rural.SetActive(true);
        zombieSpawnNorth.SetActive(false);
        zombieSpawnSouth.SetActive(false);
        zombieSpawnWest.SetActive(true);
        zombieSpawnEast.SetActive(true);
    }
    
    void Update(){
        int count = 0;
        int sumSpawned = 0;
        // Find all GameObjects in the scene
        GameObject[] allGameObjects = FindObjectsOfType<GameObject>();

        // Loop through each GameObject and check its name
        foreach (GameObject obj in allGameObjects)
        {
            if (obj.name.StartsWith("ZombieSpawner"))
            {
                sumSpawned+=obj.GetComponent<ZombieSpawner>().temp1;
            }
            if (obj.tag.Equals("Zombie")){
                count++;
            }
        }
        // for now, press F after all the zombies in the first round are eliminated
        if (Input.GetKey(KeyCode.F) && sumSpawned==60 && count==0){
            NextLevel1();
        }
        // for now, press J after all the zombies in the second round are eliminated
        if (Input.GetKey(KeyCode.J) && sumSpawned==160 && count==0){
            NextLevel2();
        }
        player1killed.text = "Zombies killed: "+p1ZombieKilled;
        player2killed.text = "Zombies killed: "+p2ZombieKilled;
    }
    public void NextLevel1(){
        StartCoroutine(LoadLevel1());
    }
    IEnumerator LoadLevel1(){
        zombieSpawnNorth.SetActive(true);
        zombieSpawnSouth.SetActive(true);
        GameObject[] allGameObjects = FindObjectsOfType<GameObject>();
        // Loop through each GameObject and check its name
        foreach (GameObject obj in allGameObjects)
        {
            if (obj.name.StartsWith("ZombieSpawner"))
            {
                obj.GetComponent<ZombieSpawner>().spawnCount=40;
                obj.GetComponent<ZombieSpawner>().temp1=0;
                obj.GetComponent<ZombieSpawner>().temp2=0;
            }
        }
        transitionAnim.SetTrigger("End");
        yield return new WaitForSeconds(1);
        rural.SetActive(false);
        city.SetActive(true);
        player2GearPanel.SetActive(true);
        ZombieSpawner.powerCount = 12;
        ZombieSpawner.maxSpawmTime = 4f;
        ZombieSpawner.maxSpawmTime = 7f;
        transitionAnim.SetTrigger("Start");
    }
    public void NextLevel2(){
        StartCoroutine(LoadLevel2());
    }
    IEnumerator LoadLevel2(){
        zombieSpawnEast.SetActive(false);
        GameObject[] allGameObjects = FindObjectsOfType<GameObject>();
        // Loop through each GameObject and check its name
        foreach (GameObject obj in allGameObjects)
        {
            if (obj.name.StartsWith("ZombieSpawner"))
            {
                obj.GetComponent<ZombieSpawner>().spawnCount=50;
                obj.GetComponent<ZombieSpawner>().temp1=0;
                obj.GetComponent<ZombieSpawner>().temp2=0;
            }
        }
        transitionAnim.SetTrigger("End");
        yield return new WaitForSeconds(1);
        city.SetActive(false);
        rooftop.SetActive(true);
        ZombieSpawner.powerCount = 15;
        ZombieSpawner.maxSpawmTime = 3f;
        ZombieSpawner.maxSpawmTime = 5f;
        transitionAnim.SetTrigger("Start");
    }
}
