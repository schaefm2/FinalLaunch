using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChangeManager : MonoBehaviour
{
    private bool playerInRangeOfExit = false;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            playerInRangeOfExit = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            playerInRangeOfExit = false;
        }
    }

    // Update is called once per frame
    void Update() {
        if (playerInRangeOfExit && Input.GetKeyDown(KeyCode.F)) {
            Debug.Log("Exit Level Triggered by exit door!");
            Debug.Log("Exit currently not working, requires a build and run which cannot be completed for milestone 2");
            SceneManager.LoadScene("Level1");
        }
    }
}
