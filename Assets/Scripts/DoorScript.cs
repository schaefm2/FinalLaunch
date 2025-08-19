using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{

    Animator anim;

    public playerItemController target;
    public bool doorOpenBool = false;

    // public Collider triggerArea;
    private bool PlayerInTriggerArea = false;

    // Start is called before the first frame update
    void Start() {
        anim = GetComponent<Animator>();
        GameObject Player = GameObject.FindWithTag("Player");
        if (Player != null) {
            target = Player.GetComponent<playerItemController>();
        }
    }

    // Update is called once per frame
    void Update() {   
        if (target.hasKey && PlayerInTriggerArea && Input.GetKeyDown(KeyCode.F)) {
            Debug.Log("Door Triggered to open after pressing F!");
            openDoor();
        }
    }

    void openDoor() {
        if (doorOpenBool == false) {
            doorOpenBool = true; // so it only can open once
            Debug.Log("Door opened");
            anim.SetBool("KeyInLock", true);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            PlayerInTriggerArea = true;
            Debug.Log("Player detected in trigger area...");
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            PlayerInTriggerArea = false;
            Debug.Log("Player detected leaving the trigger area...");
        }
    }

}
