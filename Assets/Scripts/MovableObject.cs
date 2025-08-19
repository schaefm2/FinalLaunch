using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : MonoBehaviour
{
    private KeyCode interactKey = KeyCode.F;
    private bool inRange = false;
    private Vector3 endPosition;
    private Vector3 startPosition;
    private bool moving = false;
    public float speed;
    public float distance;

    void Start()
    {
        startPosition = transform.position;
        endPosition = transform.position - new Vector3(0, 0,distance);
    }
    void Update()
    {
            if (inRange && Input.GetKeyDown(interactKey) && !moving)
            {
                BeginMoving();
            }

            if (moving)
            {
                Move();
            }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
        }
    }

    private void BeginMoving()
    {
        moving = true;
        
        Vector3 temp = startPosition;
        startPosition = endPosition;
        endPosition = temp;
    }

    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, endPosition, speed*Time.deltaTime);
        if (Vector3.Distance(transform.position, endPosition) < 0.01f)
        {
            moving = false;
        }
    }
}
