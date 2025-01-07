using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartHeliProgress : MonoBehaviour
{
    [SerializeField]
    Image image;

    [SerializeField]
    AudioSource audioSource;

    [SerializeField]
    float decayRate = 0.5f; // How fast the progress decreases per second
    private Collider2D playerNearby;
    private float startTime = 0;
    private int count = 0;
    public bool completed = false;
    public bool gameCompleted = false;

    void Start()
    {
        image.gameObject.SetActive(false);
    }

    public void Reset()
    {
        image.gameObject.SetActive(false);
        startTime = 0;
        count = 0;
        completed = false;
        gameCompleted = false;
        transform.position = new Vector3(7.78f, 3.45f, 0);
    }

    void Update()
    {
        if (playerNearby != null)
        {
            if (
                Input.GetKey(KeyCode.E)
                && transform.gameObject.GetComponent<FixProgress>().completed == true
                && !completed
            )
            {
                StartEngine();
            }
            else if (!completed) // Only decay if not completed
            {
                // Gradually decrease the progress
                startTime = Mathf.Max(0, startTime - (decayRate * Time.deltaTime));
                image.fillAmount = startTime / 10f;
            }
        }

        if (completed && !audioSource.isPlaying)
        {
            PlayAudio();
        }

        if (count == 2)
        {
            gameCompleted = true;
            MoveUp();
        }
    }

    void StartEngine()
    {
        if (startTime <= 10)
        {
            startTime += Time.deltaTime;
            image.fillAmount = startTime / 10f;
        }
        else
        {
            Debug.Log("Engine has been started");
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
        transform.position += new Vector3(0, Time.deltaTime * 3f, 0);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (
            collider.name == "Player 1"
            && !completed
            && transform.GetComponent<FixProgress>().completed
        )
        {
            playerNearby = collider;
            image.gameObject.SetActive(true);
        }
        if ((collider.name == "Player 1" || collider.name == "Player 2") && completed)
        {
            collider.gameObject.SetActive(false);
            count++;
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if ((collider.name == "Player 1" || collider.name == "Player 2") && completed)
        {
            collider.gameObject.SetActive(false);
            count++;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.name == "Player 1")
        {
            playerNearby = null;
            image.gameObject.SetActive(false);
        }
    }
}
