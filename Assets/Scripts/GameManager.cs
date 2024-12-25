using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] Animator transitionAnim;
    [SerializeField] GameObject player1;
    [SerializeField] GameObject player2;
    [SerializeField] GameObject player1UI;
    [SerializeField] GameObject player2UI;
    [SerializeField] GameObject speech;
    [SerializeField] TextMeshProUGUI speechText;
    [SerializeField] GameObject player2GearPanel;
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject rural;
    [SerializeField] GameObject city;
    [SerializeField] GameObject rooftop;
    [SerializeField] GameObject zombieSpawnNorth;
    [SerializeField] GameObject zombieSpawnSouth;
    [SerializeField] GameObject zombieSpawnWest;
    [SerializeField] GameObject zombieSpawnWest1;
    [SerializeField] GameObject zombieSpawnWest2;
    [SerializeField] GameObject zombieSpawnEast;
    [SerializeField] Text player1killed;
    [SerializeField] Text player2killed;
    public static int p1ZombieKilled = 0;
    public static int p2ZombieKilled = 0;
    public static bool allGearsCollected = false;
    private Player1Movement p1Movement;
    private Player2Movement p2Movement;
    private Dictionary<int, string> speechDict;
    private int speechKey = 0;
    private bool inGame = false;
    private bool level1Done = false;
    private bool level2Done = false;
    private bool level3Done = false;
    void Start(){
        p1Movement = player1.GetComponent<Player1Movement>();
        p2Movement = player2.GetComponent<Player2Movement>();
        player1.SetActive(false);
        player2.SetActive(false);
        player1UI.SetActive(false);
        player2UI.SetActive(false);
        speech.SetActive(false);
        mainMenu.SetActive(true);
        rural.SetActive(false);
        city.SetActive(false);
        rooftop.SetActive(false);
        zombieSpawnNorth.SetActive(false);
        zombieSpawnSouth.SetActive(false);
        zombieSpawnWest.SetActive(false);
        zombieSpawnWest1.SetActive(false);
        zombieSpawnWest2.SetActive(false);
        zombieSpawnEast.SetActive(false);
        transitionAnim.gameObject.SetActive(false);
        
        speechDict = new Dictionary<int, string>
        {
            { 0, "Terrence: Hey man, I should be heading home now. It was nice talking to you." },
            { 1, "Phillip: Yeah dude, just hit me up again if we wanna hangout…" },
            { 2, "Phillip: Sup Josh…" },
            { 3, "Josh: Dude, have you seen the news? There is a zombie outbreak going on right now!" },
            { 4, "Phillip: What? How bad is it?" },
            { 5, "Josh: It’s everywhere! I am in the city right now and it’s out of control here! Where are you right now?" },
            { 6, "Phillip: Me and Terrence are just outside town; we were both just hanging out in my place." },
            { 7, "Josh: Well, get yourselves prepared because I heard the zombies are rampaging in every direction right now, some of them are probably coming to your place right now." },
            { 8, "Phillip: Wait, if the zombies are everywhere right now, where do we go?" },
            { 9, "Josh: Ok, here is the plan. There is a chopper at the top of the apartment in where I live. I heard there is a safe zone in the North, where the military has things under control. You know where my apartment is right?" },
            { 10, "Phillip: Yeah, we will be heading to where you are right now." },
            { 11, "Josh: Cool, come before it gets worse! Ciao." },
            { 12, "Terrence: How are we gonna get to the city?" },
            { 13, "Phillip: Here is a pistol. We are gonna have to fight our way to the city. You know how to use guns right?" },
            { 14, "Terrence: Yeah, I do, but it’s been a while though." },
            { 15, "Phillip: Whatever, let’s get to the city." },
            { 16, "Terrence: Oh shoot! Here we go! Start shooting!" },
            { 17, "Terrence: Phew, I hope that was all the zombies from the city." },
            { 18, "Phillip: That’s not possible, the city has tens of thousands of people and we just killed around 60." },
            { 19, "Terrence: Fair enough. Come on, let’s get to the city." },
            { 21, "Josh: Where the hell are you guys?" },
            { 22, "Phillip: We are literally just outside the apartment. What about you?" },
            { 23, "Josh: Well, I am beside the chopper, and we have a problem. Some parts inside the chopper are rusted and broken since it hasn’t been flown for ages. The engine is dead." },
            { 24, "Phillip: I believe I can fix the chopper. I just need a few parts from some vehicles if that’s one way." },
            { 25, "Josh: You just have to get some parts from, say, cars, and apply it to the chopper so that it works again, but even if we are able to fix it, we still need someone who can fly it." },
            { 26, "Terrence: I can fly. I was a pilot before I retired a few months ago." },
            { 27, "Josh: Cool, I guess we are not done after all." },
            { 28, "Phillip: Ok, we will meet you soon…" },
            { 29, "Josh: Guys, we have another problem, these…" },
            { 30, "Terrence: What the hell just happened?" },
            { 31, "Phillip: I don’t know, but hopefully he’s alright. Come on, we gotta get some parts from these cars. But it will take a while for me to get them." },
            { 32, "Terrence: Why don’t we just use grenades, blow up the car and then get the parts?" },
            { 33, "Phillip: Good idea. We need to blow up all four of these cars. Remember, both of us need to stay alive if we want to survive." },
            { 34, "Terrence: Quickly, get the parts, and defend yourself!" },
            { 35, "Phillip: Alright, I got all the parts that I could use to fix the chopper." },
            { 36, "Terrence: Cool, let’s go upstairs. I don’t want to see a single blood thirsty zombie again." },
            { 38, "Phillip: Josh is not here…" },
            { 39, "Terrence: It looks like he got killed by the zombie hordes." },
            { 40, "Phillip: Quick! I am gonna fix the chopper, give me some time." },
            { 41, "Terrence: Uh Phillip…" },
            { 42, "Terrence: Fix FASTER! I will hold them off." },
            { 43, "Phillip: I will try ASAP!" },
            { 44, "Phillip: I fixed it, Terrence. Quick, come get in and start the chopper." },
            { 45, "Terrence: Ok, let me start the engine." },
            { 46, "Terrence: GET IN! LET’S GO!" }
        };
        
    }

    public void ClickedPlay(){
        transitionAnim.gameObject.SetActive(true);
        mainMenu.SetActive(false);
        rural.SetActive(true);
        player1.SetActive(true);
        player2.SetActive(true);
        player1.transform.position = new Vector3(1.1f, 2.87f, 0);
        player2.transform.position = new Vector3(3.1f, 2.87f, 0);
        // p1.AutoMoveToPosition(new Vector2(-1, 0));
        // p2.AutoMoveToPosition(new Vector2(1, 0));

        // transitionAnim.gameObject.SetActive(true);
        // player1.SetActive(true);
        // player2.SetActive(true);
        // player1UI.SetActive(true);
        // player2UI.SetActive(true);
        // player2GearPanel.SetActive(false);
        // mainMenu.SetActive(false);
        // rural.SetActive(true);
        // zombieSpawnNorth.SetActive(false);
        // zombieSpawnSouth.SetActive(false);
        // zombieSpawnWest.SetActive(true);
        // zombieSpawnWest1.SetActive(false);
        // zombieSpawnWest2.SetActive(false);
        // zombieSpawnEast.SetActive(true);
    }
    
    void Update(){
        if (!inGame){
            player1UI.SetActive(false);
            player2UI.SetActive(false);
            if (!p1Movement.isAutoMoving && !p2Movement.isAutoMoving && speechKey >= 0 && speechKey <=19){
                transitionAnim.gameObject.SetActive(false);
                speech.SetActive(true);
                speechText.text = speechDict[speechKey];
            }
            else if (!p1Movement.isAutoMoving && !p2Movement.isAutoMoving && speechKey == 20 && !level1Done){
                speech.SetActive(false);
                NextLevel2();
            }
            else if (!p1Movement.isAutoMoving && !p2Movement.isAutoMoving && speechKey >= 21 && speechKey <= 36 && level1Done){
                transitionAnim.gameObject.SetActive(false);
                speech.SetActive(true);
                speechText.text = speechDict[speechKey];
            }
            else if (!p1Movement.isAutoMoving && !p2Movement.isAutoMoving && speechKey == 37 && !level2Done){
                speech.SetActive(false);
                NextLevel3();
            }
            else if (!p1Movement.isAutoMoving && !p2Movement.isAutoMoving && speechKey >= 38 && speechKey <= 46 && level2Done){
                transitionAnim.gameObject.SetActive(false);
                speech.SetActive(true);
                speechText.text = speechDict[speechKey];
            }
        }
        else{
            player1UI.SetActive(true);
            player2UI.SetActive(true);
            speech.SetActive(false);
            if (level1Done && !level2Done){
                player2GearPanel.SetActive(true);
                zombieSpawnNorth.SetActive(true);
                zombieSpawnSouth.SetActive(true);
            }
            else if (level1Done && level2Done){
                zombieSpawnWest1.SetActive(true);
                zombieSpawnWest2.SetActive(true);
            }
            else{
                player2GearPanel.SetActive(false);
                zombieSpawnNorth.SetActive(false);
                zombieSpawnSouth.SetActive(false);
            }
            // zombieSpawnWest.SetActive(true);
            // zombieSpawnWest1.SetActive(false);
            // zombieSpawnWest2.SetActive(false);
            // zombieSpawnEast.SetActive(true);
        }
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
        if (sumSpawned==60 && count==0){
            speech.SetActive(true);
            inGame = false;
        }
        else if (sumSpawned==160 && count==0 && allGearsCollected){
            speech.SetActive(true);
            inGame = false;
        }
        // // for now, press J after all the zombies in the second round are eliminated
        // if (Input.GetKey(KeyCode.J) && sumSpawned==160 && count==0 && allGearsCollected){
        //     NextLevel3();
        // }
        player1killed.text = "Zombies killed: "+p1ZombieKilled;
        player2killed.text = "Zombies killed: "+p2ZombieKilled;
    }

    public void Skip(){
        if (speechKey==16){
            inGame = true;
        }
        else if (speechKey==19){
            p1Movement.isAutoMoving = true;
            p2Movement.isAutoMoving = true;
        }
        else if (speechKey==33){
            inGame = true;
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
        }
        else if (speechKey==36){
            p1Movement.isAutoMoving = true;
            p2Movement.isAutoMoving = true;
        }
        else if (speechKey==33){
            inGame = true;
            GameObject[] allGameObjects = FindObjectsOfType<GameObject>();
            // Loop through each GameObject and check its name
            foreach (GameObject obj in allGameObjects)
            {
                if (obj.name.StartsWith("ZombieSpawner"))
                {
                    obj.GetComponent<ZombieSpawner>().spawnCount=int.MaxValue;
                    obj.GetComponent<ZombieSpawner>().temp1=0;
                    obj.GetComponent<ZombieSpawner>().temp2=0;
                }
            }
        }
        speechKey++;
    }

    public void NextLevel2(){
        StartCoroutine(LoadLevel2());
    }
    IEnumerator LoadLevel2(){
        transitionAnim.gameObject.SetActive(true);
        player1.SetActive(false);
        player2.SetActive(false);
        // zombieSpawnNorth.SetActive(true);
        // zombieSpawnSouth.SetActive(true);
        // GameObject[] allGameObjects = FindObjectsOfType<GameObject>();
        // // Loop through each GameObject and check its name
        // foreach (GameObject obj in allGameObjects)
        // {
        //     if (obj.name.StartsWith("ZombieSpawner"))
        //     {
        //         obj.GetComponent<ZombieSpawner>().spawnCount=40;
        //         obj.GetComponent<ZombieSpawner>().temp1=0;
        //         obj.GetComponent<ZombieSpawner>().temp2=0;
        //     }
        // }
        transitionAnim.SetTrigger("End");
        level1Done = true;
        yield return new WaitForSeconds(1);
        rural.SetActive(false);
        city.SetActive(true);
        // player2GearPanel.SetActive(true);
        ZombieSpawner.powerCount = 12;
        ZombieSpawner.maxSpawmTime = 4f;
        ZombieSpawner.maxSpawmTime = 7f;
        transitionAnim.SetTrigger("Start");
        player1.SetActive(true);
        player2.SetActive(true);
        player1.transform.position = new Vector3(-11.87f, 0, 0);
        player2.transform.position = new Vector3(-9.87f, 0, 0);
        p1Movement.isAutoMoving = true;
        p2Movement.isAutoMoving = true;
        speechKey++;
    }
    public void NextLevel3(){
        StartCoroutine(LoadLevel3());
    }
    IEnumerator LoadLevel3(){
        transitionAnim.gameObject.SetActive(true);
        player1.SetActive(false);
        player2.SetActive(false);
        zombieSpawnNorth.SetActive(false);
        zombieSpawnSouth.SetActive(false);
        zombieSpawnEast.SetActive(false);
        zombieSpawnWest.SetActive(false);
        // zombieSpawnWest1.SetActive(true);
        // zombieSpawnWest2.SetActive(true);
        // GameObject[] allGameObjects = FindObjectsOfType<GameObject>();
        // // Loop through each GameObject and check its name
        // foreach (GameObject obj in allGameObjects)
        // {
        //     if (obj.name.StartsWith("ZombieSpawner"))
        //     {
        //         obj.GetComponent<ZombieSpawner>().spawnCount=int.MaxValue;
        //         obj.GetComponent<ZombieSpawner>().temp1=0;
        //         obj.GetComponent<ZombieSpawner>().temp2=0;
        //     }
        // }
        transitionAnim.SetTrigger("End");
        level2Done = true;
        yield return new WaitForSeconds(1);
        city.SetActive(false);
        rooftop.SetActive(true);
        ZombieSpawner.powerCount = int.MaxValue;
        ZombieSpawner.maxSpawmTime = 1f;
        ZombieSpawner.maxSpawmTime = 2f;
        transitionAnim.SetTrigger("Start");
        player1.SetActive(true);
        player2.SetActive(true);
        // player1.transform.position = new Vector3(-11.87f, 0, 0);
        // player2.transform.position = new Vector3(-9.87f, 0, 0);
        p1Movement.isAutoMoving = true;
        p2Movement.isAutoMoving = true;
        speechKey++;
    }
}
