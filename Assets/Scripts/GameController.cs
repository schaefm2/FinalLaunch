using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    [SerializeField] private GameObject customImage;

    // Start is called before the first frame update
    void Start()
    {
        MeshRenderer myImage = customImage.GetComponent<MeshRenderer>();
        myImage.enabled = false;

    }


    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            MeshRenderer myImage = customImage.GetComponent<MeshRenderer>();
            myImage.enabled = true;
            print("Player Detected: You won!");
            Destroy(gameObject);
        }
    }

}
