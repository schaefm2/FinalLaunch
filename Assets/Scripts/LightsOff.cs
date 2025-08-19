using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsOff : MonoBehaviour
{
    
    
    public GameObject lights;

    // Update is called once per frame
    void Update()
    {
        if (GameController1.instance.powerOn)
        {
            Debug.Log("We di it");
            lights.SetActive(true);
            this.enabled = false;   
        }
    }
}
