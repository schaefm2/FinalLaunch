using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    private GameObject wall;
    // Start is called before the first frame update
    void Start()
    {
        wall = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && wall.active)
        {
            wall.SetActive(false);
            other.gameObject.GetComponent<TankScript>().gatesPassed++;
        }
    }
}
