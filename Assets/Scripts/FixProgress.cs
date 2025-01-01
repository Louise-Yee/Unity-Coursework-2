using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FixProgress : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] Scrollbar scrollbar;
    private Collider2D playerNearby;
    float fixTime = 0;
    public bool completed = false;
    // Start is called before the first frame update
    void Start()
    {
        image.gameObject.SetActive(false);
        scrollbar.gameObject.SetActive(false);
    }

    public void Reset(){
        completed = false;
        image.gameObject.SetActive(false);
        scrollbar.gameObject.SetActive(false);
        fixTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerNearby != null){
            if (Input.GetKey(KeyCode.L)){
                StartFixing();
            }
        }
    }

    void StartFixing(){
        PlayerInventory playerNearbyInventory = playerNearby.gameObject.GetComponent<PlayerInventory>();
        if (playerNearbyInventory.gearCount>0){
            if (fixTime <= 10){
                fixTime += Time.deltaTime;
                image.fillAmount = fixTime / 10f;
            }
            else{
                scrollbar.size += 0.25f;
                fixTime = 0;
                image.fillAmount = 0f;
                playerNearbyInventory.gearCount-=1;
                if (playerNearbyInventory.gearCount==0){
                    image.gameObject.SetActive(false);
                    scrollbar.gameObject.SetActive(false);
                    completed = true;
                }
                playerNearbyInventory.UpdateUI();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collider){
        if (collider.name == "Player 2" && !completed){
            playerNearby = collider;
            image.gameObject.SetActive(true);
            scrollbar.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D collider){
        if (collider.name == "Player 2"){
            playerNearby = null;
            image.gameObject.SetActive(false);
            scrollbar.gameObject.SetActive(false);
        }
    }
}
