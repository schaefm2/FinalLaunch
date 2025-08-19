using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingTargetsManager : MonoBehaviour
{
    
    Animator anim; // door animator

    // for shooting targets on wall
    [SerializeField] private GameObject[] shootingTargets;
    private int targetsHit = 0;

    [SerializeField] private GameObject[] zombiesToKill;
    private int zombiesKilled = 0;

    private bool doorOpened = false; // if door is opened

    public GameObject ClosedDoor;

    // Start is called before the first frame update
    void Start() {
        ClosedDoor = GameObject.Find("ShootingRangeDoor");
        anim = ClosedDoor.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {   
        if (targetsHit == 10 && zombiesKilled == 3 && !doorOpened) {  // All targets hit
            anim.SetBool("AllTargetsDestroyed", true);
            Debug.Log("Opening Shooting Range Door");
            doorOpened = true;
        }
    }

    public void TargetHit(GameObject target) {
        Debug.Log("Inside of TargetHit Function Call");
        Debug.Log("length of shootingTargets is " + shootingTargets.Length);
        for (int i = 0; i < shootingTargets.Length; i++) {
            Debug.Log("Inside of TargetHit forloop");
            if (shootingTargets[i] == target) {  // correct target matched
                Destroy(target);
                targetsHit++;
                Debug.Log(target.name + " has been activated");
                return;
            }
        }
    }

    public void TutorialZombieKilled(GameObject tutorialZombie) {
        for (int i = 0; i < zombiesToKill.Length; i++) {
            Debug.Log("Inside of TutorialZombieKilled Function Call");
            if (zombiesToKill[i] == tutorialZombie) {  // correct target matched
                Destroy(tutorialZombie);
                zombiesKilled++;
                Debug.Log(tutorialZombie.name + " has been killed");
                return;
            }
        }
    }

}
