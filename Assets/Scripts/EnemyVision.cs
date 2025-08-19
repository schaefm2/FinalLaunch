using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyVision : MonoBehaviour
{
    private bool sighted;

    private GameObject player;

    public Transform lastSightedLocation { get; private set; }

    // most of this is directly from the professor
    [Header("Sensor")]
    public float sensorRadius = 8f;
    public float fieldOfViewAngle = 120f;
    public float sensorTimeStep = 0.25f;

    private Coroutine checkSightCoroutine;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = other.gameObject;
            checkSightCoroutine = StartCoroutine(CheckSight());
        }
    }

    private IEnumerator CheckSight()
    {
        while (true)
        {
            yield return new WaitForSeconds(sensorTimeStep);

            sighted = false;

            RaycastHit hit;
            Vector3 enemyToPlayer = player.transform.position - transform.position;
            float angleToPlayer = Vector3.Angle(enemyToPlayer, transform.forward);
            Ray ray = new Ray(transform.position + new Vector3(0f, 1f, 0f), enemyToPlayer);

            if (angleToPlayer < fieldOfViewAngle)
            {
                Physics.Raycast(ray, out hit, sensorRadius);
                if (hit.transform.gameObject.CompareTag("Player"))
                {
                    sighted = true;
                    lastSightedLocation = player.transform;
                }
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StopCoroutine(checkSightCoroutine);

        }
    }

    public bool IsEnemySpotted()
    {
        return sighted;
    }
}
