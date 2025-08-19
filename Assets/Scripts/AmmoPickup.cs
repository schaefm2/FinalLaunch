using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public enum AmmoType{Pistol, Rifle, Shotgun, Grenade, Pills}
    [Header("Pickup Settings")]
    public AmmoType ammoType;

    public int totalAmmo = 20;
    
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
    // Update is called once per frame
    void Update()
    {
            
            if (playerInRange && Input.GetKeyDown(KeyCode.F) && ammoStillExists)
            {
                PickupAmmo();
            }

    }


    private void PickupAmmo()
    {
        if (playerItemController.instance != null)
        {
            ammoStillExists = false;
            // int totalAmmo = CountChildPickups();
            //int totalAmmo = 30; // just so we dont have to put 30 cells for rifle ammo but I like countChildPickups for grenades
            switch (ammoType)
            {
                case AmmoType.Pistol:
                    int tempAmmo = (int)(totalAmmo*Random.Range(0.8f, 2f));
                    playerItemController.instance.pistolAmmo += tempAmmo;
                    Debug.Log($"{tempAmmo} Pistol ammo was picked up in switch");
                    Debug.Log($"Player now has {playerItemController.instance.pistolAmmo} pistol ammo");
                    break;
                case AmmoType.Rifle:
                    playerItemController.instance.rifleAmmo += (int)(totalAmmo*Random.Range(0.8f, 2f));
                    Debug.Log("Rifle ammo was picked up in switch");
                    break;
                case AmmoType.Shotgun:
                    playerItemController.instance.shotgunAmmo += (int)(totalAmmo*Random.Range(0.8f, 2f));
                    Debug.Log("shotgun ammo was picked up in switch");
                    break;
                case AmmoType.Grenade:
                    // playerItemController.instance.grenadeCount += totalAmmo;
                    int totalGrenades = CountChildPickups();
                    playerItemController.instance.grenadeCount += totalGrenades; // get however many grenades are in the parent object, so can make different models to match # picked up
                    Debug.Log($"{totalGrenades} Grenades were picked up in switch");
                    break;
                case AmmoType.Pills:   // best type of ammo
                    playerItemController.instance.pillCount += totalAmmo;
                    Debug.Log($"player now holds {playerItemController.instance.pillCount} pills");
                    Destroy(gameObject);
                    break;
            }

            DestroyAllPickups();
        }
        else
        {
            foreach (Transform fuelCell in transform)
            {
                // for each fuel cell child of the ammo pickup object 
                Destroy(fuelCell.gameObject);
            }

            // keeping disabled for now so no soft-lock inside tutorial
            // ammoStillExists = false; // cant pickup ammo a second time
            Debug.Log("Ammo has been picked up!");
        }
    }

    private int CountChildPickups()
    {
        int count = 0;
        foreach (Transform child in transform)
        {
            count++;
        }
        return count ;
    }

    private void DestroyAllPickups()
    {
        foreach (Transform child in transform)
        {
            // for each fuel cell child of the ammo pickup object 
            Destroy(child.gameObject);
        }
    }
}
