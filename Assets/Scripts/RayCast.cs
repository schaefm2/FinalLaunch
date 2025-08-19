using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class RayCast : MonoBehaviour
{
    private Ray ray;
    private RaycastHit hit;
    private float rayDistance = 10f;
       // Update is called once per frame
    void Update()
    {
        ray = new Ray(transform.position + new Vector3(0f, 2f, 0f ), transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.red);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.distance < rayDistance)
            {
                // print(hit.collider.gameObject.name);  // spam prints every frame
                
            }
        }
    }
}
