using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShoot : MonoBehaviour
{
    private float shootSpeed = 0.0f;
    // Start is called before the first frame update
    //void Start()
    //{
    //    shootSpeed = 0.0f;
    //}

    // called by parent tank to set the speed of the bullet
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
        // decrease hp of other 
        if (other.gameObject.tag == "MiniGameEnemy")
        {
            other.GetComponent<RedBall>().health--;
        }
        Destroy(gameObject);
    }
}
