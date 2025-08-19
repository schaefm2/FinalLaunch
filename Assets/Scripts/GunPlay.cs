using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPlay : MonoBehaviour
{

    [SerializeField] private Transform spawnLocation;
    [SerializeField] private GameObject projectile;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(projectile, spawnLocation.position, transform.rotation);
        }
    }

    
}
