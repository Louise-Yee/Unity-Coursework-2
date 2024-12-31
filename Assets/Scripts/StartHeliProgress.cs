using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartHeliProgress : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] AudioSource audioSource;
    private Collider2D playerNearby;
    private float startTime = 0;
    private int count = 0;
    public bool completed = false;
    // Start is called before the first frame update
    void Start()
    {
        image.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerNearby != null){
            if (Input.GetKey(KeyCode.E) && transform.gameObject.GetComponent<FixProgress>().completed == true && !completed){
                StartEngine();
            }
            else{
                startTime = 0;
            }
        }
        if (completed && !audioSource.isPlaying){
            // Play audio when completed
            PlayAudio();
        }
        if (count == 2){
            MoveUp();
        }
    }

    void StartEngine(){
        if (startTime <= 10){
            startTime += Time.deltaTime;
            image.fillAmount = startTime / 10f;
        }
        else{
            image.gameObject.SetActive(false);
            completed = true;
        }
    }

    void PlayAudio()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

    void MoveUp()
    {
        // Move the GameObject up slowly
        transform.position += new Vector3(0, Time.deltaTime * 3f, 0); // Adjust speed as necessary
    }

    void OnTriggerEnter2D(Collider2D collider){
        if (collider.name == "Player 1" && !completed){
            playerNearby = collider;
            image.gameObject.SetActive(true);
        }
        if ((collider.name == "Player 1" || collider.name == "Player 2") && completed){
            collider.gameObject.SetActive(false);
            count++;
        }
    }

    void OnTriggerExit2D(Collider2D collider){
        if (collider.name == "Player 1"){
            playerNearby = null;
            image.gameObject.SetActive(false);
        }
    }
}
