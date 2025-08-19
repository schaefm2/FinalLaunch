using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupAmmo : MonoBehaviour
{

    private bool playerInRange = false;
    private bool ammoStillExists = true;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            playerInRange = false;
        }
    }

    void Update() {
        // in range, ammo hasnt been picked up yet, and hits interact key (F)
        if (playerInRange && ammoStillExists && Input.GetKeyDown(KeyCode.F)) {
            foreach (Transform fuelCell in transform) { // for each fuel cell child of the ammo pickup object 
                Destroy(fuelCell.gameObject);
            }
            // keeping disabled for now so no soft-lock inside tutorial
            // ammoStillExists = false; // cant pickup ammo a second time
            Debug.Log("Ammo has been picked up!");
        }
    }

}
