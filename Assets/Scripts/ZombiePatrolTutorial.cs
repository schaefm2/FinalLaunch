using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiePatrolTutorial : MonoBehaviour
{
    public Transform[] points;
    private int destPoint = 0;
    private UnityEngine.AI.NavMeshAgent agent;

    public float AIDetectedionRange = 15f;
    private Transform player;
    // public LayerMask playerLayer;

    private bool isChasing = false;
    private float chaseTimer = 0f;
    private float chaseCooldown = 3f;


    void Start() {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        // Disabling auto-braking allows for continuous movement between points 
        // (ie, the agent doesn't slow down as it approaches a destination point).
        agent.autoBraking = false;

        player = GameObject.FindGameObjectWithTag("Player").transform;

        GotoNextPoint();
    }

    void Update() {

        if (playerDetected()) {
            Debug.Log("Player detected!");
            isChasing = true;
            chaseTimer = 0;
        }
        
        if (isChasing) {
            agent.destination = player.position; // run at the players current position

            chaseTimer += Time.deltaTime;
            
            if (chaseTimer >= chaseCooldown) {
                isChasing = false;
                GotoNextPoint();
            }
        } else {
            // Choose the next destination point when the agent gets
            // close to the current one.
            if (!agent.pathPending && agent.remainingDistance < 0.5f) {
                GotoNextPoint();
            }
        }
    }

    void GotoNextPoint() {
        // Returns if no points have been set up
        if (points.Length == 0) return;
            
        // Set the agent to go to the currently selected destination.
        agent.destination = points[destPoint].position;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (destPoint + 1) % points.Length;
    }

    bool playerDetected() {
        if (Vector3.Distance(transform.position, player.position) < AIDetectedionRange) {
            Vector3 directionToPlayer = player.position - transform.position;
            RaycastHit hit;

            Debug.DrawLine(transform.position, player.position, Color.red);

            if (Physics.Raycast(transform.position, directionToPlayer.normalized, out hit, AIDetectedionRange)) {
                if (hit.transform.CompareTag("Player")) {
                    return true; // player is detected by zombie
                }
            }

        }
        return false;
    }

}
