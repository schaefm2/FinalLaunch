using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerItemController : MonoBehaviour
{

    public bool hasKey;

    public int pistolAmmo = 0;
    public int rifleAmmo = 0;
    public int shotgunAmmo = 0;
    public int grenadeCount = 0;
    public int pillCount = 0;

    [SerializeField] public int playerHealth = 2;
    [SerializeField] public int playerMaxHealth = 10;

    public bool hasArmor = false;
    public bool tutorial = false;

    public string overheatText = "";

    public static playerItemController instance;
    

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            if (!tutorial)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
        else if (instance != this)
        {
            if (!tutorial)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
            }
        }
    }

}
