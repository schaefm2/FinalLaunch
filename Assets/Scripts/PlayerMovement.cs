using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
       [SerializeField]
    [Range(0,20)]
    private float moveSpeed = 5f;
    [SerializeField]
    private float lookSpeed = 10f;
    private Vector2 rotation = new Vector2(0, 0);
    
    [SerializeField]
    private float jumpHeight = 3f;

    private Rigidbody rb;
    private bool touchingGround;

    public bool hasKey = false;

    private Ray ray;
    private RaycastHit hit;
    public float rayDistance = 4f;
    private BoxCollider playerCollider;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {

        // WASD movement controlls
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        transform.Translate(movement * Time.deltaTime * moveSpeed);

        rotation.y += Input.GetAxis("Mouse X") * lookSpeed;
        transform.eulerAngles = rotation;

        if (Input.GetButtonDown("Jump") && touchingGround) {
            rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
        }


        ray = new Ray(transform.position + new Vector3(0f, playerCollider.center.y + 0.5f, 0f), transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.red);

        if (Physics.Raycast(ray, out hit)) {
            if (hit.distance < rayDistance) {
                Debug.Log("Raycast hit something infront");
            }
        }

    }

    private void OnCollisionEnter(Collision collision) {
        touchingGround = true;
    }

    private void OnCollisionExit(Collision collision) {
        touchingGround = false;
    }
}
