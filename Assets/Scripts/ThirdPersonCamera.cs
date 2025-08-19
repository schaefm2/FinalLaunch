using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{

    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public Rigidbody rb;
    public Transform lookAt;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    { 
        /*
        //calulate orientation based off cameramovement
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        //calculate the orientation of the player obj
        Vector3 dirToLookAt = lookAt.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = dirToLookAt.normalized;

        playerObj.forward = dirToLookAt.normalized;
        */

    }
}
