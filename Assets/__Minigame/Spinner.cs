using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    private GameObject player;
    private TankScript playerScript;
    // gate player has to pass to start shooting at it
    public int gateThreshold;
    // navmesh agent for the spinner
    private NavMeshAgent agent;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<TankScript>();
        agent = GetComponent<NavMeshAgent>();
        // alows spinning + chasing
        agent.updateRotation = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerScript.gatesPassed >= gateThreshold)
        {
            agent.destination = player.transform.position;
            transform.Rotate(0f, 0.5f, 0f);
        }
    }
}
