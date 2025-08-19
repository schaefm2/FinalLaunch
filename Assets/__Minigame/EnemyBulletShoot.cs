using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyBulletShoot : MonoBehaviour
{
    private float shootSpeed = 0.0f;
    // Start is called before the first frame update
    //void Start()
    //{
    //    shootSpeed = 0.0f;
    //}

    // called by parent to set the speed of the bullet
    public void setSpeed(float speed)
    {
        shootSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * shootSpeed * Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    { 
        // kill the player
        if (other.gameObject.tag == "Player")
        {
            print("a");
            SceneManager.LoadScene(4);
        }
        print(other.gameObject.tag);
        Destroy(gameObject);
    }
}
