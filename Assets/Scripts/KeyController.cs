using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour
{
    private playerItemController playerItems;
    // public bool hasKey = false;


    // Start is called before the first frame update
    void Start()
    {
        playerItems = GameObject.FindGameObjectWithTag("Player").GetComponent<playerItemController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            // hasKey = true;
            playerItems.hasKey = true; // set item in playItems to true (acquired) -- might make more sense to keep key & door local
            Destroy(gameObject);
            print("Player picked up key, walk up to door and hit 'F' to open");
            Debug.Log("Triggered by and set key for: " + other.gameObject.name);
        }
    }
}
