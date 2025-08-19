using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StartScript : MonoBehaviour
{
    private GameObject player;
    public GameObject[] zombies;
    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        // player has finished minigame
        if (SingletonMiniGame.Instance.finishedMiniGame)
        {
            player.transform.position = new Vector3(9.50925446f, 0.873464584f, 20.8864174f);
            List<string> zombieArray = SingletonMiniGame.Instance.ZombieNamesThatWeHaveKilled;
            // kill zombies that we already killed
            for (int i = 0; i < zombieArray.Count; i++)
            {
                GameObject.Find(zombieArray[i]).SetActive(false);
            }
            GameObject door = GameObject.Find("BigDoor");
            Animator animator = door.GetComponent<Animator>();
            print("setting big door");
            animator.SetBool("character_nearby", true);
            // activate zombies inside key room
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

    // Update is called once per frame
    void Update()
    {
    }
}
