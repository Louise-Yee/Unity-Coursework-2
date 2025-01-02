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
    [SerializeField] GameObject player1TutorialUI;
    [SerializeField] GameObject player2TutorialUI;

    [SerializeField] GameObject gameCompleted;
    [SerializeField] GameObject gameOver;
    [SerializeField] GameObject backToMainMenu;
    [SerializeField] GameObject speech;
    [SerializeField] TextMeshProUGUI speechText;
    [SerializeField] GameObject mission;
    [SerializeField] TextMeshProUGUI missionText;
    [SerializeField] GameObject terrence;
    [SerializeField] GameObject phillip;
    [SerializeField] GameObject josh;
    [SerializeField] GameObject player2GearPanel;
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject rural;
    [SerializeField] GameObject city;
    [SerializeField] GameObject rooftop;
    [SerializeField] GameObject helicopter;
    [SerializeField] GameObject zombieSpawnNorth;
    [SerializeField] GameObject zombieSpawnSouth;
    [SerializeField] GameObject zombieSpawnWest;
    [SerializeField] GameObject zombieSpawnWest1;
    [SerializeField] GameObject zombieSpawnWest2;
    [SerializeField] GameObject zombieSpawnEast;
    [SerializeField] Text player1killed;
    [SerializeField] Text player2killed;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] terrenceVoice;
    [SerializeField] AudioClip[] phillipVoice;
    [SerializeField] AudioClip[] joshVoice;
    [SerializeField] AudioClip[] altTerrenceVoice;
    [SerializeField] AudioClip[] altPhillipVoice;
    public static int p1ZombieKilled = 0;
    public static int p2ZombieKilled = 0;
    private int terrenceIndex = 0;
    private int phillipIndex = 0;
    private int altTerrenceIndex = 0;
    private int altPhillipIndex = 0;
    private int joshIndex = 0;
    public static bool allGearsCollected = false;
    private Player1Movement p1Movement;
    private Player2Movement p2Movement;
    private Dictionary<int, string> speechDict;
    private Dictionary<int, string> alternate1Dict;
    private Dictionary<int, string> alternate2Dict;
    private int speechKey = 0;
    private int alternate1Key = 0;
    private int alternate2Key = 0;
    private bool inGame = false;
    private bool level1Done = false;
    private bool level2Done = false;
    public static bool player1Dead = false;
    public static bool player2Dead = false;
    private bool isPlaying = false;
    private bool isPaused = false;
    void Start(){
        p1Movement = player1.GetComponent<Player1Movement>();
        p2Movement = player2.GetComponent<Player2Movement>();
        player1.SetActive(false);
        player2.SetActive(false);
        player1UI.SetActive(false);
        player2UI.SetActive(false);
        gameCompleted.SetActive(false);
        gameOver.SetActive(false);
        backToMainMenu.SetActive(false);
        speech.SetActive(false);
        mission.SetActive(false);
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
            { 0, "<b>Terrence:</b>\nHey man, I should be heading home now. It was nice talking to you." },
            { 1, "<b>Phillip:</b>\nYeah dude, just hit me up if we wanna hangout…" },
            { 2, "<b>Phillip:</b>\nSup Josh…" },
            { 3, "<b>Josh:</b>\nDude, have you seen the news? There is a zombie outbreak going on right now!" },
            { 4, "<b>Phillip:</b>\nWhat? How bad is it?" },
            { 5, "<b>Josh:</b>\nIt’s everywhere! I am in the city right now and it’s out of control here! Where are you?" },
            { 6, "<b>Phillip:</b>\nTerrence and I are just outside town; we were both just hanging out in my place." },
            { 7, "<b>Josh:</b>\nWell, get yourselves prepared, because I heard the zombies are rampaging in every direction right now, some of them are probably coming to your place." },
            { 8, "<b>Phillip:</b>\nWait, if the zombies are everywhere right now, where do we go?" },
            { 9, "<b>Josh:</b>\nOk, here is the plan. There is a chopper at the top of the apartment in where I live. I heard there is a safe zone in the North, where the military has things under control. You know where my apartment is right?" },
            { 10, "<b>Phillip:</b>\nYeah, we will be heading where you are." },
            { 11, "<b>Josh:</b>\nCool, come on, before it gets worse!" },
            { 12, "<b>Terrence:</b>\nHow are we gonna get to the city?" },
            { 13, "<b>Phillip:</b>\nHere is a pistol. We are gonna have to fight our way to the city. You know how to use guns right?" },
            { 14, "<b>Terrence:</b>\nYeah, I do, but it’s been a while though." },
            { 15, "<b>Phillip:</b>\nWhatever, let’s get to the city." },
            { 16, "<b>Terrence:</b>\nOh shoot! Here we go! Start shooting!" },
            { 17, "<b>Terrence:</b>\nPhew, I hope that was all the zombies from the city." },
            { 18, "<b>Phillip:</b>\nThat’s not possible, the city has tens of thousands of people and we only killed around 30." },
            { 19, "<b>Terrence:</b>\nFair enough. Come on, let’s get to the city." },
            { 21, "<b>Josh:</b>\nWhere the hell are you guys?" },
            { 22, "<b>Phillip:</b>\nWe are literally just outside the apartment. What about you?" },
            { 23, "<b>Josh:</b>\nWell, I am beside the chopper, and we have a problem. Some parts inside the chopper are rusted and broken since it hasn’t been flown for ages. The engine is dead." },
            { 24, "<b>Phillip:</b>\nI believe I can fix the chopper. I just need a few parts from some vehicles if that’s one way." },
            { 25, "<b>Josh:</b>\nYou just have to get some parts from, say, cars, and apply it to the chopper so that it works again, but even if we are able to fix it, we still need someone who can fly it." },
            { 26, "<b>Terrence:</b>\nI can fly. I was a pilot before I retired a few months ago." },
            { 27, "<b>Josh:</b>\nCool, I guess we are not done after all." },
            { 28, "<b>Phillip:</b>\nOk, we will meet you soon…" },
            { 29, "<b>Josh:</b>\nGuys, we have another problem, these…" },
            { 30, "<b>Terrence:</b>\nWhat the hell just happened?" },
            { 31, "<b>Phillip:</b>\nI don’t know, but hopefully he’s alright. Come on, we gotta get some parts from these cars. But it will take a while for me to get them." },
            { 32, "<b>Terrence:</b>\nWhy don’t we just use grenades, blow up the car and then get the parts? Two should be enough for each." },
            { 33, "<b>Phillip:</b>\nGood idea. We need to blow up all four of these cars. Remember, both of us need to stay alive if we want to survive." },
            { 34, "<b>Terrence:</b>\nQuickly, get the parts and defend yourself!" },
            { 35, "<b>Phillip:</b>\nAlright, I got all the parts that I could use to fix the chopper." },
            { 36, "<b>Terrence:</b>\nCool, let’s go upstairs. I don’t want to see a single blood thirsty zombie again." },
            { 38, "<b>Phillip:</b>\nJosh is not here…" },
            { 39, "<b>Terrence:</b>\nIt looks like he got killed by the zombie hordes." },
            { 40, "<b>Phillip:</b>\nQuick! I am gonna fix the chopper, give me some time." },
            { 41, "<b>Terrence:</b>\nUh Phillip…" },
            { 42, "<b>Terrence:</b>\nFix FASTER! I will hold them off." },
            { 43, "<b>Phillip:</b>\nI will try ASAP!" },
            { 44, "<b>Phillip:</b>\nI fixed it, Terrence. Quick, come get in and start the chopper." },
            { 45, "<b>Terrence:</b>\nOk, let me start the engine." },
            { 46, "<b>Terrence:</b>\nGET IN! LET’S GO!" }
        };
        alternate1Dict = new Dictionary<int, string>
        {
            { 0, "<b>Terrence:</b>\nShoot! Phillip is gone. I just hope that Josh is still alive and he knows how to fix the chopper." },
            { 2, "<b>Terrence:</b>\nJosh, where are you? JOSH? Dang it, I guess he got killed by the zombies." },
            { 3, "<b>Terrence:</b>\nOh no, I am dead..." }
        };
        alternate2Dict = new Dictionary<int, string>
        {
            { 0, "<b>Phillip:</b>\nShoot! Terrence is gone. I just hope Josh knows how to fly the damn chopper." },
            { 2, "<b>Phillip:</b>\nJosh, where are you? I got all the parts, JOSH? Dang it, I guess he’s dead." },
            { 3, "<b>Phillip:</b>\nOh no, I am next..." }
        };
    }

    public void ClickedPlay(){
        transitionAnim.gameObject.SetActive(true);
        mainMenu.SetActive(false);
        rural.SetActive(true);
        player1.SetActive(true);
        player2.SetActive(true);
        p1Movement = player1.GetComponent<Player1Movement>();
        p2Movement = player2.GetComponent<Player2Movement>();
        player1.transform.position = new Vector3(1.1f, 2.87f, 0);
        player2.transform.position = new Vector3(3.1f, 2.87f, 0);
    }

    public void ClickedMainMenu(){
        Time.timeScale = 1f;
        player1UI.SetActive(false);
        player2UI.SetActive(false);
        player1TutorialUI.SetActive(true);
        player2TutorialUI.SetActive(true);
        Color temp;
        Image gameCompletedImages = gameCompleted.GetComponentInChildren<Image>(true);
        temp = gameCompletedImages.color;
        temp.a = 0f;
        gameCompletedImages.color = temp;
        TextMeshProUGUI[] gameCompletedTexts = gameCompleted.GetComponentsInChildren<TextMeshProUGUI>(true);
        foreach (TextMeshProUGUI gCT in gameCompletedTexts){
            temp = gCT.color;
            temp.a = 0f;
            gCT.color = temp;
        }
        Image gameOverImages = gameOver.GetComponentInChildren<Image>(true);
        temp = gameOverImages.color;
        temp.a = 0f;
        gameCompletedImages.color = temp;
        TextMeshProUGUI[] gameOverTexts = gameOver.GetComponentsInChildren<TextMeshProUGUI>(true);
        foreach (TextMeshProUGUI gOT in gameOverTexts){
            temp = gOT.color;
            temp.a = 0f;
            gOT.color = temp;
        }
        gameCompleted.SetActive(false);
        gameOver.SetActive(false);
        speech.SetActive(false);
        mission.SetActive(false);
        rural.SetActive(false);
        city.SetActive(false);
        rooftop.SetActive(false);
        
        player1.GetComponent<PlayerHealthSystem>().Reset();
        player1.GetComponent<ShootingScript>().Reset();
        player1.GetComponent<Player1Movement>().Reset();
        player1.GetComponent<PlayerInventory>().Reset();
        player1.transform.position = new Vector3(1.1f, 2.87f, 0);
        player1.SetActive(false);
        
        player2.GetComponent<PlayerHealthSystem>().Reset();
        player2.GetComponent<ShootingScript>().Reset();
        player2.GetComponent<Player2Movement>().Reset();
        player2.GetComponent<PlayerInventory>().Reset();
        player2.transform.position = new Vector3(3.1f, 2.87f, 0);
        player2.SetActive(false);
        
        DestroyDrops();
        
        mainMenu.SetActive(true);
        transitionAnim.gameObject.SetActive(false);
        zombieSpawnNorth.SetActive(true);
        zombieSpawnSouth.SetActive(true);
        zombieSpawnWest.SetActive(true);
        zombieSpawnWest1.SetActive(true);
        zombieSpawnWest2.SetActive(true);
        zombieSpawnEast.SetActive(true);
        GameObject[] allGameObjects = FindObjectsOfType<GameObject>();
        // Loop through each GameObject and check its name
        foreach (GameObject obj in allGameObjects)
        {
            if (obj.name.StartsWith("ZombieSpawner"))
            {
                obj.GetComponent<ZombieSpawner>().spawnCount=15;
                obj.GetComponent<ZombieSpawner>().temp1=0;
                obj.GetComponent<ZombieSpawner>().temp2=0;
                obj.GetComponent<ZombieSpawner>().spawnInterval=3f;
            }
            else if (obj.tag.Equals("Zombie") || obj.name.Contains("Bullet") || obj.name.Contains("Grenade"))
            {
                Destroy(obj);
            }
        }
        ZombieSpawner.currentIndex = 0;
        ZombieSpawner.minSpawmTime = 2f;
        ZombieSpawner.maxSpawmTime = 4f;
        ZombieSpawner.powerCount = 6;
        zombieSpawnNorth.SetActive(false);
        zombieSpawnSouth.SetActive(false);
        zombieSpawnWest.SetActive(false);
        zombieSpawnWest1.SetActive(false);
        zombieSpawnWest2.SetActive(false);
        zombieSpawnEast.SetActive(false);
        p1ZombieKilled = 0;
        p2ZombieKilled = 0;
        terrenceIndex = 0;
        phillipIndex = 0;
        altTerrenceIndex = 0;
        altPhillipIndex = 0;
        joshIndex = 0;
        allGearsCollected = false;
        speechKey = 0;
        alternate1Key = 0;
        alternate2Key = 0;
        inGame = false;
        level1Done = false;
        level2Done = false;
        player1Dead = false;
        player2Dead = false;
        isPlaying = false;
        isPaused = false;
        backToMainMenu.SetActive(false);
    }

    void GameCompleted(){
        gameCompleted.SetActive(true);
        TextMeshProUGUI[] textComponents = gameCompleted.GetComponentsInChildren<TextMeshProUGUI>();

        for (int i = 0; i < textComponents.Length; i++) {
            if (textComponents[i].name == "p1KillCount"){
                textComponents[i].text = "P1 kill count: "+p1ZombieKilled;
            }
            else if (textComponents[i].name == "p2KillCount"){
                textComponents[i].text = "P2 kill count: "+p2ZombieKilled;
            }
        }
        backToMainMenu.SetActive(true);
    }

    void GameOver(){
        gameOver.SetActive(true);
        backToMainMenu.SetActive(true);
    }
    
    void Update(){
        if (helicopter.GetComponent<StartHeliProgress>().gameCompleted == true){
            GameCompleted();
        }
        else if (player1.GetComponent<PlayerHealthSystem>().currentHealth <= 0 && player2.GetComponent<PlayerHealthSystem>().currentHealth <= 0){
            GameOver();
        }
        else{
            if (!inGame){
                player1UI.SetActive(false);
                player2UI.SetActive(false);
                if (!player1Dead && !player2Dead){
                    if (!p1Movement.isAutoMoving && !p2Movement.isAutoMoving && speechKey >= 0 && speechKey <=19){
                        transitionAnim.gameObject.SetActive(false);
                        if (!isPaused){
                            speech.SetActive(true);
                            speechText.text = speechDict[speechKey];
                        }
                        else{
                            speech.SetActive(false);
                        }
                    }
                    else if (!p1Movement.isAutoMoving && !p2Movement.isAutoMoving && speechKey == 20 && !level1Done){
                        NextLevel2();
                    }
                    else if (!p1Movement.isAutoMoving && !p2Movement.isAutoMoving && speechKey >= 21 && speechKey <= 36 && level1Done){
                        transitionAnim.gameObject.SetActive(false);
                        if (!isPaused){
                            speech.SetActive(true);
                            speechText.text = speechDict[speechKey];
                        }
                        else{
                            speech.SetActive(false);
                        }
                    }
                    else if (!p1Movement.isAutoMoving && !p2Movement.isAutoMoving && speechKey == 37 && !level2Done){
                        NextLevel3();
                    }
                    else if (!p1Movement.isAutoMoving && !p2Movement.isAutoMoving && speechKey >= 38 && speechKey <= 43 && level2Done){
                        transitionAnim.gameObject.SetActive(false);
                        if (!isPaused){
                            speech.SetActive(true);
                            speechText.text = speechDict[speechKey];
                        }
                        else{
                            speech.SetActive(false);
                        }
                    }
                    SwitchCharacter();
                }
                else if (player1Dead && !player2Dead){
                    if (!p2Movement.isAutoMoving && alternate2Key == 0){
                        transitionAnim.gameObject.SetActive(false);
                        if (!isPaused){
                            speech.SetActive(true);
                            phillip.SetActive(true);
                            speechText.text = alternate2Dict[alternate2Key];
                        }
                        else{
                            speech.SetActive(false);
                            phillip.SetActive(false);
                        }
                    }
                    else if (!p2Movement.isAutoMoving && alternate2Key == 1 && !level2Done){
                        NextLevel3();
                    }
                    else if (!p2Movement.isAutoMoving && alternate2Key >= 2 && alternate2Key <= 3 && level2Done){
                        transitionAnim.gameObject.SetActive(false);
                        if (!isPaused){
                            speech.SetActive(true);
                            phillip.SetActive(true);
                            speechText.text = alternate2Dict[alternate2Key];
                        }
                        else{
                            speech.SetActive(false);
                            phillip.SetActive(false);
                        }
                    }
                    SwitchCharacter();
                }
                else if (!player1Dead && player2Dead){
                    if (!p1Movement.isAutoMoving && alternate1Key == 0){
                        transitionAnim.gameObject.SetActive(false);
                        if (!isPaused){
                            speech.SetActive(true);
                            terrence.SetActive(true);
                            speechText.text = alternate1Dict[alternate1Key];
                        }
                        else{
                            speech.SetActive(false);
                            terrence.SetActive(false);
                        }
                    }
                    else if (!p1Movement.isAutoMoving && alternate1Key == 1 && !level2Done){
                        NextLevel3();
                    }
                    else if (!p1Movement.isAutoMoving && alternate1Key >= 2 && alternate1Key <= 3 && level2Done){
                        transitionAnim.gameObject.SetActive(false);
                        if (!isPaused){
                            speech.SetActive(true);
                            terrence.SetActive(true);
                            speechText.text = alternate1Dict[alternate1Key];
                        }
                        else{
                            speech.SetActive(false);
                            terrence.SetActive(false);
                        }
                    }
                    SwitchCharacter();
                }
            }
            else{
                player1UI.SetActive(true);
                player2UI.SetActive(true);
                speech.SetActive(false);
                terrence.SetActive(false);
                phillip.SetActive(false);
                josh.SetActive(false);
                if (!level1Done && !level2Done){
                    player2GearPanel.SetActive(false);
                }
                else if (level1Done && !level2Done){
                    player2GearPanel.SetActive(true);
                }
                int count = 0;
                int sumSpawned = 0;
                int carCount = 0;
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
                    if (obj.name.StartsWith("citycar")){
                        carCount++;
                    }
                }
                if (sumSpawned==30 && count==0 && !level1Done && !level2Done){
                    mission.SetActive(false);
                    inGame = false;
                }
                else if (!player1Dead && !player2Dead){
                    if (sumSpawned==80 && count==0 && level1Done && !level2Done && allGearsCollected){
                        mission.SetActive(false);
                        inGame = false;
                    }
                    else if (level1Done && level2Done && allGearsCollected && helicopter.GetComponent<FixProgress>().completed && !helicopter.GetComponent<StartHeliProgress>().completed && (speechKey==44 || speechKey==45)){
                        if (!isPaused){
                            speech.SetActive(true);
                            speechText.text = speechDict[speechKey];
                        }
                        else{
                            speech.SetActive(false);
                        }
                        SwitchCharacter();
                    }
                    else if (level1Done && level2Done && allGearsCollected && helicopter.GetComponent<FixProgress>().completed && helicopter.GetComponent<StartHeliProgress>().completed && speechKey==46){
                        mission.SetActive(false);
                        if (!isPaused){
                            speech.SetActive(true);
                            speechText.text = speechDict[speechKey];
                        }
                        else{
                            speech.SetActive(false);
                        }
                        SwitchCharacter();
                    }
                    if (level1Done && level2Done){
                        missionText.text = "<b>Mission:</b>\nFix the helicopter. ("+(4-player2.GetComponent<PlayerInventory>().gearCount)+"/4)\nStart the helicopter.";
                    }
                }
                else if (player1Dead && !player2Dead){
                    if (sumSpawned==80 && count==0 && level1Done && !level2Done && allGearsCollected){
                        mission.SetActive(false);
                        inGame = false;
                    }
                }
                else if (!player1Dead && player2Dead){
                    if (sumSpawned==80 && count==0 && level1Done && !level2Done){
                        mission.SetActive(false);
                        inGame = false;
                    }
                }
                if (!level1Done && !level2Done){
                    missionText.text = "<b>Mission:</b>\nKill all 30 zombies. ("+(p1ZombieKilled+p2ZombieKilled)+"/30)";
                }
                else if (level1Done && !level2Done){
                    missionText.text = "<b>Mission:</b>\nKill all 80 zombies. ("+(p1ZombieKilled+p2ZombieKilled-30)+"/80)\nBlow up the 4 cars using grenades to get gears. ("+(4-carCount)+"/4)\nCollect all 4 gears. ("+player2.GetComponent<PlayerInventory>().gearCount+"/4)";
                }
            }
            player1killed.text = "Zombies killed: "+p1ZombieKilled;
            player2killed.text = "Zombies killed: "+p2ZombieKilled;
        }
    }

    public void SwitchCharacter(){
        if (!player1Dead && !player2Dead){
            // Check if speechKey exists in the dictionary
            if (!speechDict.ContainsKey(speechKey)) {
                return; // Exit early if speechKey is missing
            }
            if (speechDict[speechKey].Contains("<b>Terrence:</b>")){
                terrence.SetActive(true);
                phillip.SetActive(false);
                josh.SetActive(false);
                if (!isPlaying && speech.activeSelf){
                    audioSource.clip = terrenceVoice[terrenceIndex];
                    terrenceIndex++;
                    isPlaying = true;
                    audioSource.Play();
                }
            }
            else if (speechDict[speechKey].Contains("<b>Phillip:</b>")){
                terrence.SetActive(false);
                phillip.SetActive(true);
                josh.SetActive(false);
                if (!isPlaying && speech.activeSelf){
                    audioSource.clip = phillipVoice[phillipIndex];
                    phillipIndex++;
                    isPlaying = true;
                    audioSource.Play();
                }
            }
            else if (speechDict[speechKey].Contains("<b>Josh:</b>")){
                terrence.SetActive(false);
                phillip.SetActive(false);
                josh.SetActive(true);
                if (!isPlaying && speech.activeSelf){
                    audioSource.clip = joshVoice[joshIndex];
                    joshIndex++;
                    isPlaying = true;
                    audioSource.Play();
                }
            }
        }
        else if (player1Dead && !player2Dead){
            if (!alternate2Dict.ContainsKey(alternate2Key)) {
                return;
            }
            if (!isPlaying && speech.activeSelf){
                audioSource.clip = altPhillipVoice[altPhillipIndex];
                altPhillipIndex++;
                isPlaying = true;
                audioSource.Play();
            }
        }
        else if (!player1Dead && player2Dead){
            if (!alternate1Dict.ContainsKey(alternate1Key)) {
                return;
            }
            if (!isPlaying && speech.activeSelf){
                audioSource.clip = altTerrenceVoice[altTerrenceIndex];
                altTerrenceIndex++;
                isPlaying = true;
                audioSource.Play();
            }
        }
    }

    public void Skip(){
        if (!player1Dead && !player2Dead){
            if (speechKey==16){
                inGame = true;
                zombieSpawnWest.SetActive(true);
                zombieSpawnEast.SetActive(true);
                mission.SetActive(true);
            }
            else if (speechKey==19){
                p1Movement.isAutoMoving = true;
                p2Movement.isAutoMoving = true;
                speech.SetActive(false);
            }
            else if (speechKey==34){
                inGame = true;
                zombieSpawnNorth.SetActive(true);
                zombieSpawnSouth.SetActive(true);
                mission.SetActive(true);
                GameObject[] allGameObjects = FindObjectsOfType<GameObject>();
                // Loop through each GameObject and check its name
                foreach (GameObject obj in allGameObjects)
                {
                    if (obj.name.StartsWith("ZombieSpawner"))
                    {
                        obj.GetComponent<ZombieSpawner>().spawnCount=20;
                        obj.GetComponent<ZombieSpawner>().temp1=0;
                        obj.GetComponent<ZombieSpawner>().temp2=0;
                    }
                }
            }
            else if (speechKey==36){
                p1Movement.isAutoMoving = true;
                p2Movement.isAutoMoving = true;
                speech.SetActive(false);
            }
            else if (speechKey==43){
                inGame = true;
                zombieSpawnWest1.SetActive(true);
                zombieSpawnWest2.SetActive(true);
                mission.SetActive(true);
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
            isPlaying = false;
            audioSource.Stop();
        }
        else if (player1Dead && !player2Dead){
            if (alternate2Key==0){
                p2Movement.isAutoMoving = true;
                speech.SetActive(false);
                phillip.SetActive(false);
            }
            else if (alternate2Key==3){
                inGame = true;
                zombieSpawnWest1.SetActive(true);
                zombieSpawnWest2.SetActive(true);
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
            alternate2Key++;
            isPlaying = false;
            audioSource.Stop();
        }
        else if (!player1Dead && player2Dead){
            if (alternate1Key==0){
                p1Movement.isAutoMoving = true;
                speech.SetActive(false);
                terrence.SetActive(false);
            }
            else if (alternate1Key==3){
                inGame = true;
                zombieSpawnWest1.SetActive(true);
                zombieSpawnWest2.SetActive(true);
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
                    if (obj.name.Equals("Zombie") || obj.name.Equals("PoweredZombie")){
                        Destroy(obj);
                    }
                }
            }
            alternate1Key++;
            isPlaying = false;
            audioSource.Stop();
        }
    }

    public void NextLevel2(){
        StartCoroutine(LoadLevel2());
    }
    IEnumerator LoadLevel2(){
        transitionAnim.gameObject.SetActive(true);
        player1.SetActive(false);
        player2.SetActive(false);
        player1TutorialUI.SetActive(false);
        player2TutorialUI.SetActive(false);
        transitionAnim.SetTrigger("End");
        level1Done = true;
        yield return new WaitForSeconds(2);
        rural.SetActive(false);
        DestroyDrops();
        city.SetActive(true);
        ZombieSpawner.powerCount = 20;
        ZombieSpawner.minSpawmTime = 4f;
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
        transitionAnim.SetTrigger("End");
        level2Done = true;
        yield return new WaitForSeconds(2);
        city.SetActive(false);
        DestroyDrops();
        rooftop.SetActive(true);
        ZombieSpawner.powerCount = int.MaxValue;
        if (player1Dead || player2Dead){
            ZombieSpawner.minSpawmTime = 0.1f;
            ZombieSpawner.maxSpawmTime = 0.9f;
        }
        else{
            ZombieSpawner.minSpawmTime = 1f;
            ZombieSpawner.maxSpawmTime = 2f;
        }
        transitionAnim.SetTrigger("Start");
        if (!player1Dead && !player2Dead){
            player1.SetActive(true);
            player2.SetActive(true);
            player1.transform.position = new Vector3(-11.46f, -1.51f, 0);
            player2.transform.position = new Vector3(-9.86f, -1.46f, 0);
            p1Movement.isAutoMoving = true;
            p2Movement.isAutoMoving = true;
            speechKey++;
        }
        else if (player1Dead && !player2Dead){
            player2.SetActive(true);
            player2.transform.position = new Vector3(-9.86f, -1.46f, 0);
            p2Movement.isAutoMoving = true;
            alternate2Key++;
        }
        else if (!player1Dead && player2Dead){
            player1.SetActive(true);
            player1.transform.position = new Vector3(-11.46f, -1.51f, 0);
            p1Movement.isAutoMoving = true;
            alternate1Key++;
        }
    }
    public void DestroyDrops(){
        GameObject[] allGameObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allGameObjects)
        {
            if (obj.tag.Equals("GrenadeDrop") || obj.tag.Equals("HealthDrop") || obj.tag.Equals("GearDrop")){
                Destroy(obj);
            }
        }
    }
    public void Pause()
    {
        isPaused = true;
        audioSource.Pause();
        Time.timeScale = 0f;
    }
    public void Resume()
    {
        isPaused = false;
        if (!audioSource.isPlaying && audioSource.time > 0){
            audioSource.Play();
        }
        Time.timeScale = 1f;
    }
}
