using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    // [SerializeField]
    // [Range(0,20)]
    // private float moveSpeed = 5f;
    [SerializeField]
    private float lookSpeed = 10f;
    private Vector2 rotation = new Vector2(0, 0);
    // private Vector3 movement;

    Animator anim;

    public bool hasKey = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        
        float verticalInput = Input.GetAxis("Vertical");
        anim.SetFloat("Speed", verticalInput);

        float horizontalInput = Input.GetAxis("Horizontal");
        anim.SetFloat("Direction", horizontalInput);

        // movement = new Vector3(0, 0, verticalInput) * moveSpeed * Time.deltaTime;

        rotation.y += Input.GetAxis("Mouse X") * lookSpeed;
        transform.eulerAngles = rotation;


        // turning left and right
        if (Input.GetKeyDown(KeyCode.E)) { // right
            anim.SetBool("Turn Right", true);
            Debug.Log("turn right");
        } else if (Input.GetKeyDown(KeyCode.Q)) {  // left
            anim.SetBool("Turn Left", true);
            Debug.Log("turn left");
        // }
        } else {  // not pressed then reset parameteres
            anim.SetBool("Turn Right", false);
            anim.SetBool("Turn Left", false);
        }

        // anim.SetBool("Turn Right", false);
        // anim.SetBool("Turn Left", false);

        // clicking hides cursor (focuses in)
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Confined; //Confine cursor to screen bounds
            Cursor.visible = false;
        }

    }
}
