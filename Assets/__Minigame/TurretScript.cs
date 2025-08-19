using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretScript : MonoBehaviour
{
    private GameObject player;
    private TankScript playerScript;
    // gate player has to pass to start shooting at it
    public int gateThreshold;
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

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<TankScript>();
        lastShot = 0;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player.transform);
        lastShot += Time.deltaTime;
        if (playerScript.gatesPassed >= gateThreshold && lastShot >= shootSpeed)
        {
            lastShot = 0;
            // shoot at Player
            GameObject newBullet = Instantiate(bullet, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
            newBullet.GetComponent<EnemyBulletShoot>().setSpeed(bulletSpeed);
        }
    }
}
