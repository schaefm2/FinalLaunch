using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DoorOpener : MonoBehaviour
{
    private Animator anim;
    public GameObject[] zombies;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (SingletonMiniGame.Instance.key1 && SingletonMiniGame.Instance.key2)
        {
            SingletonMiniGame.Instance.key1 = false;
            anim.SetBool("character_nearby", true);
            for (int i = 0; i < zombies.Length; i++)
            {
                GameObject zombie = zombies[i];
                if (zombie != null)
                {
                    zombie.GetComponent<Animator>().applyRootMotion = true;
                    zombie.GetComponent<NavMeshAgent>().enabled = true;
                    zombie.GetComponent<ZombieController>().enabled = true;
                }
            }
        }
    }
}
