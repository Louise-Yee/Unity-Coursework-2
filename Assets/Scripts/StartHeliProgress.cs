using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartHeliProgress : MonoBehaviour
{
    [SerializeField] Image image;
    private Collider2D playerNearby;
    float startTime = 0;
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

    void OnTriggerEnter2D(Collider2D collider){
        if (collider.name == "Player 1" && !completed){
            playerNearby = collider;
            image.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D collider){
        if (collider.name == "Player 1"){
            playerNearby = null;
            image.gameObject.SetActive(false);
        }
    }
}
