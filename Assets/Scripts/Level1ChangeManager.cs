using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Level1ChangeManager : MonoBehaviour
{
    private bool playerInRangeOfExit = false;
    private bool hasCure = false;
    //private bool hasArmor = false;
    public GameController1 gameController;
    [SerializeField] private TextMeshPro cureMessage;

    private void Start()
    {
        gameController = FindObjectOfType<GameController1>();
        cureMessage.gameObject.SetActive(false);
    }
    
    private void OnTriggerEnter(Collider other) {
        hasCure = GameController1.instance.cure;
        //hasArmor = playerItemController.instance.hasArmor;
        if (other.CompareTag("Player")) {
            playerInRangeOfExit = true;
            if (!hasCure)
            {
                cureMessage.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            playerInRangeOfExit = false;
            if(cureMessage.gameObject.activeSelf)
            {
                cureMessage.gameObject.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update() {
        if (playerInRangeOfExit && hasCure) {
            Debug.Log("Exit Level Triggered by exit door!");
            Debug.Log("Exit currently not working, requires a build and run which cannot be completed for milestone 2");
            SceneManager.LoadScene("LastLevel");
        }

        if (!playerInRangeOfExit)
        {
            cureMessage.gameObject.SetActive(false);
        }
        //
        // if (playerInRangeOfExit && Input.GetKeyDown(KeyCode.F) && !hasCure)
        // {
        //     Debug.Log("You Left the cure in the Main Lab!");
        // }
        //
        // if (playerInRangeOfExit && Input.GetKeyDown(KeyCode.F) && !hasArmor)
        // {
        //     Debug.Log("You Need Armor For The Next Level!");
        // }
    }
}
