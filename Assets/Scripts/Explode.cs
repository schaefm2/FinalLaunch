using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

public class Explode : MonoBehaviour
{

    // removed SerializedField for grenadeDelay since not attached to a game object it creates unchangable reference
    private float grenadeDelay = 3.5f; // changed from 1 second to 4, otherwise the grenade courotine and fuse of 3 seconds does not work
    [SerializeField] private float radius = 10.0f;
    [SerializeField] private float force = 1500.0f;
    [SerializeField] private bool explodeOnCollision = false;
    [SerializeField] private GameObject effectsPrefab;
    [SerializeField] private float effectDisplayTime = 3.0f;

    public AudioClip ExplosionSound;

    private float delayTimer;

    private void Awake()
    {
        delayTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        delayTimer += Time.deltaTime;
        // Debug.Log($"delayTimer is incremented to {delayTimer} trying to hit {grenadeDelay}");

        if(delayTimer >= grenadeDelay && !explodeOnCollision)
        {
            Debug.Log($"delayTimer {delayTimer} hit {grenadeDelay} grenade now exploding...");
            DoExplosion();
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (explodeOnCollision)
        {
            DoExplosion();
            // Destroy(gameObject);
        }
    }

    private void DoExplosion()
    {
        HandleEffects();
        HandleDestruction();
    }

    private void HandleEffects()
    {
        if(effectsPrefab != null)
        {
            GameObject effect = Instantiate(effectsPrefab, transform.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(ExplosionSound, transform.position, 30f);
            // Destroy(effect, effectDisplayTime);
        }

    }

    private void HandleDestruction()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider collider in colliders)
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddExplosionForce(force, transform.position, radius);
            }
        }
    }
}
