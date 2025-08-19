using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody rb;
    public GameObject explosion;

    private float explosionRadius = 10f; // have to set in Start() too ??
    private float explosionForce = 500f;  // if we use force this could be a knockback value?
    private int damage = 10; // damage to enemies in range (zombie has 10 hp)

    // private float timer = 0f;
    private bool isFuseLit = false;

    private ShootingTargetsManager targetsManager;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();        
    }

    void Start()
    {

        targetsManager = FindObjectOfType<ShootingTargetsManager>();

        // timer = 3f;
        // isFuseLit = true;

        // explosionRadius = 10; // auto sets to 100 if not set to manually in start???? possibly weird float behavior

        float speed = 20f;
        Vector3 direction = new Vector3(transform.forward.x, transform.forward.y + .1f, transform.forward.z);
        rb.velocity = direction * speed;
        StartCoroutine(WaitToExplode(3f));
    }

    // void Update() {
    //     if (isFuseLit) {
    //         timer -= Time.deltaTime;
    //         Debug.Log($"Decreasing timer to {timer}");
    //         if (timer <= 0) {
    //             Debug.Log("Right before ExplodeGrenade in timer setup");
    //             ExplodeGrenade();
    //             isFuseLit = false;
    //             // Destroy(gameObject, 0.5f) // 
    //         }
    //     }
    // }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Grenade thrown at: " + collision.gameObject.name);

        // ExplodeGrenade(); // this should be done in WaitToExplode
        // add hit registration here for damage/shooting range
    }

    private void ExplodeGrenade() {

        Debug.Log($"Start of ExplodeGrade with exploding radius {explosionRadius}");

        // hit detection within explosion radius
        Collider[] hitColliderArray = Physics.OverlapSphere(transform.position, explosionRadius); // sphere around grenade

        Debug.Log($"Grenade hit a total of {hitColliderArray.Length} objects");
        foreach (var hitCollider in hitColliderArray) {

            Debug.Log("Grenade hit: " + hitCollider);

            if (hitCollider.CompareTag("Zombie")) { // zombie hit by grenade

                Debug.Log("$$$$$$$$$$$$$$$$$$Zombie hit: " + hitCollider);
            
                ZombieController zombie = hitCollider.GetComponent<ZombieController>();
                zombie.getShot(damage); // apply damage to zombie

            }

            if (hitCollider.CompareTag("TutorialZombie")) { // hit self with grenade

                Debug.Log("$$$$$$$$$tutorial zombie hit: " + hitCollider);
                        
                ZombieController zombie = hitCollider.GetComponent<ZombieController>();
                zombie.getShot(damage); // apply damage to zombie

                targetsManager.TutorialZombieKilled(hitCollider.gameObject);

            }

            if (hitCollider.CompareTag("Player")) { // hit self with grenade

                Debug.Log("$$$$$$$$$$$$$$$$$$Player hit: " + hitCollider);
            
                // if (playerItemController.instance.hasArmor == true) {
                //     if (playerItemController.instance.playerArmor > damage-1) { // take damage from armor
                //         playerItemController.instance.playerArmor -= damage;
                //         Debug.Log("$$$$$$$$$$$$$$$$$$Player lost armor: " + playerItemController.instance.playerArmor);
                //     }
                // } else { // player takes damage
                    Debug.Log("$$$$$$$$$$$$$$$$$$Player health before grenade: " + playerItemController.instance.playerHealth);
                    playerItemController.instance.playerHealth -= damage;
                    Debug.Log("$$$$$$$$$$$$$$$$$$Player lost health: " + playerItemController.instance.playerHealth);
                // }

            }

        }

    }

    // time flaoted for grenade fuse
    private IEnumerator WaitToExplode(float time)
    {
        Debug.Log("Start of waitToExplode");
        // ExplodeGrenade();
        yield return new WaitForSeconds(time);

        ExplodeGrenade();
        Debug.Log("End of waitToExplode");
        // Destroy(gameObject);
    }
}
