using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class ZombieController : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask obstacleLayer;
    public LayerMask glassLayer;

    public Transform[] points;
    private int index;

    public float sightRange = 14f;
    public float runRange = 5f;
    public float attackRange = 1f;
    public float runSpeed = 4f;
    public float walkSpeed = 1f;
    public float raycastHeightOffset = 1f;
    public float playerHeightOffset = 1.0f;
    private float zHealth = 10f;
    private float zAttack = 4f;

    public bool calledDeath;

    private bool inSightRange, inRunRange;
    private Vector3 lastKnownPosition;
    private bool playerDetected = false;

    private Animator animator;

    private void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player").transform;
        UpdatePlayerReference(GameObject.FindGameObjectWithTag("Player").transform);
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        agent.speed = walkSpeed;
        index = 0;
        calledDeath = false;
        GotoNextPoint();
    }

    private void Update()
    {
        Vector3 startPosition = transform.position + Vector3.up * raycastHeightOffset;
        Vector3 targetPosition = player.position + Vector3.up * playerHeightOffset;
        bool inSightRange = RaycastCheck(startPosition, targetPosition, sightRange, Color.green);
        bool inRunRange = RaycastCheck(startPosition, targetPosition, runRange, Color.blue);
        bool inAttackRange = RaycastCheck(startPosition, targetPosition, attackRange, Color.red);

        if (zHealth <= 0)
        {
            if (!calledDeath)
            {
                agent.isStopped = true;
                calledDeath = true;
                animator.SetTrigger("Death");
                StartCoroutine(KillZombie());
                if (SingletonMiniGame.Instance != null)
                {
                    // makes sure to not readd zombies when we return from minigame
                    SingletonMiniGame.Instance.ZombieNamesThatWeHaveKilled.Add(name);
                }
            }

            return;
        }

        if (inSightRange || inRunRange)
        {
            lastKnownPosition = player.position;
            playerDetected = true;
            if (inRunRange && inAttackRange)
            {
                AttackPlayer();
            }

            if (inRunRange)
            {
                ChasePlayerFast();
            }
            else
            {
                ChasePlayer();
            }
        }
        else if (playerDetected)
        {
            agent.SetDestination(lastKnownPosition);
            animator.SetTrigger("Walk");

            if (!agent.pathPending && agent.remainingDistance < 1f)
            {
                playerDetected = false;
                Patrol();
            }
        }
        else
        {
            Patrol();
        }


    }

    private void Patrol()
    {
        agent.speed = walkSpeed;
        animator.SetTrigger("Walk");
        
        if (points.Length == 0)
        {
            //Wander();
            return;
        }

        agent.SetDestination(points[index].position);
        if (!agent.pathPending && agent.remainingDistance < 1f)
        {
            GotoNextPoint();
        }
    }

    private void ChasePlayer()
    {
        if (agent.speed != walkSpeed)
        {
            agent.speed = walkSpeed;
            animator.SetTrigger("Walk");
        }

        agent.SetDestination(player.position);
    }

    private void ChasePlayerFast()
    {
        if (agent.speed != runSpeed)
        {
            agent.speed = runSpeed;
            animator.SetTrigger("Run");
        }

        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        //player.health - zDamage;
    }

    // private void OnCollisionEnter(Collider collision)
    // {
    //     // checking for bullet collision with zombie, decrease zHealth by bulletdamage
    // }

    // private void OnCollisionEnter(Collision collision)
    // {
    //     if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
    //     {
    //         Wander();
    //     }
    // }


    IEnumerator KillZombie()
    {
        yield return new WaitForSeconds(6f);

        // wait for 3 seconds and then despawn zombie
        Destroy(gameObject);

        //set animator to death animation of "FallForward" or "FallBackward"
        //make zombie inactive/fallthrough floor/destroy

    }

    private bool RaycastCheck(Vector3 startPosition, Vector3 targetPosition, float range, Color debugColor)
    {
        Vector3 directionToPlayer = (targetPosition - startPosition).normalized;

        if (Physics.Raycast(startPosition, directionToPlayer, out RaycastHit hit, range, ~glassLayer))
        {
            if (hit.collider.CompareTag("Player"))
            {
                Debug.DrawRay(startPosition, directionToPlayer * range, debugColor);
                return true;
            }
        }

        Debug.DrawRay(startPosition, directionToPlayer * range, Color.red);
        return false;
    }

    // private void AttackPlayer()
    // {
    //     animator.SetTrigger("Attack");
    // }

    private void OnDrawGizmosSelected()
    {
        Vector3 startPosition = transform.position + Vector3.up * raycastHeightOffset;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, runRange);
        /*
        if (patrolPointA != null && patrolPointB != null)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(patrolPointA.position, patrolPointB.position);
        }
        */
    }

    void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (points.Length == 0) return;

        // Set the agent to go to the currently selected destination.
        agent.destination = points[index].position;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        index = (index + 1) % points.Length;
    }

    // private void Wander()
    // {
    //     Vector3 randomDir = Random.insideUnitSphere*3f;
    //     randomDir.y = 0;
    //     randomDir += transform.position;
    //     
    //     NavMeshHit navHit;
    //     if (NavMesh.SamplePosition(randomDir, out navHit, 5f, NavMesh.AllAreas))
    //     {
    //         agent.SetDestination(navHit.position);
    //     }
    // }

    public void getShot(float damage)
    {
        print("we go shot!");
        zHealth -= damage;
    }

    public void UpdatePlayerReference(Transform newPlayer)
    {
        player = newPlayer;
    }

}
