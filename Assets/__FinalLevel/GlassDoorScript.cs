using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GlassDoorScript : MonoBehaviour
{
    Animator anim;
    private bool close;
    public GameObject[] zombies;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        close = false;
    }

    // Update is called once per frame
    void Update()
    {   
        // open or close the door
        if (close && Input.GetKeyDown(KeyCode.F))
        {
            anim.SetBool("character_nearby", !anim.GetBool("character_nearby"));
            // activate zombies
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
    private void OnTriggerEnter(Collider other)
    {
        close = true;
    }

    private void OnTriggerExit(Collider other)
    {
        close = false;
    }
}
