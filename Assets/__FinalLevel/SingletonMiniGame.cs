using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMiniGame : MonoBehaviour
{
    public static SingletonMiniGame Instance;
    // set to true when we finish minigame
    public Boolean finishedMiniGame = false;
    public Boolean key1 = false;
    public Boolean key2 = false;
    public List<String> ZombieNamesThatWeHaveKilled;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
