using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public int whichKey;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (whichKey == 1)
            {
                SingletonMiniGame.Instance.key1 = true;
            }
            else
            {
                SingletonMiniGame.Instance.key2 = true;
            }
            Destroy(gameObject);
        }
    }
}
