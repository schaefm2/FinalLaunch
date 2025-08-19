using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using UnityEngine.UIElements;

public class TankScript : MonoBehaviour
{
    public float lookSpeed = 10f;
    public float moveSpeed = 5f;
    private Vector2 rotation = new Vector2(0, 0);
    private Vector3 spawnPoint;
    public Collider playerCollider;
    private Rigidbody rigidbody;

    // prefab for the bullet
    public GameObject bullet;
    // where bullet spawns
    public GameObject bulletSpawnPoint;
    // how fast the bullet travels
    public float bulletSpeed;
    // how often were allowed to shoot
    public float shootSpeed;
    // time when we shot last
    private float lastShot;
    // keeps track of how many red gates the tank has passed
    public int gatesPassed;

    private void Start()
    {
        spawnPoint = transform.position;
        Cursor.lockState = CursorLockMode.Locked; //Confine cursor to screen bounds
        Cursor.visible = false;
        rigidbody = GetComponent<Rigidbody>();
        lastShot = Time.deltaTime;
        gatesPassed = 0;
    }
    // Update is called once per frame (in step with frames)
    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // rotate camera using mouse
        rotation.y += Input.GetAxis("Mouse X") * lookSpeed;
        transform.eulerAngles = rotation;

        // clicking hides cursor (focuses in)
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            Cursor.lockState = CursorLockMode.Locked; //Confine cursor to screen bounds
            Cursor.visible = false;
        }
        lastShot += Time.deltaTime;
        // bullet stuff
        if (Input.GetMouseButton(0) && lastShot >= shootSpeed)
        {
            lastShot = 0;
            // spawns bullet inside barrel that will shoot
            GameObject newBullet = Instantiate(bullet, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
            newBullet.GetComponent<BulletShoot>().setSpeed(bulletSpeed);
        }
    }

    // in step with physics engine
    private void FixedUpdate()
    {
        handleMove();
    }

    private void handleMove()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector2 movementInput = new Vector2(moveHorizontal, moveVertical);
        Vector3 movement = movementInput.x * transform.right * moveSpeed + movementInput.y * transform.forward * moveSpeed;
        rigidbody.velocity = new Vector3(movement.x, rigidbody.velocity.y, movement.z);

        rotation.y += Input.GetAxis("Mouse X") * lookSpeed;
        transform.eulerAngles = rotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "MiniGameEnemy")
        {
            SceneManager.LoadScene(4);
        }
    }
}
