using UnityEngine;
using System.Collections;

public class GameController1 : MonoBehaviour
{
    // Public booleans accessible in the Inspector
    public bool powerOn = true;
    public bool armoryKey = false;
    public bool securityKey = false;
    public bool cure = false;

    [SerializeField] private ScreenFader fader;
    [SerializeField] private GameObject image;
    
    

    // Singleton instance (optional but recommended)
    public static GameController1 instance;

    public enum GameState
    {
        PowerOn, ArmoryKey, SecurityKey, Cure
    }
    void Awake()
    {
        // Implementing the Singleton pattern
        if (instance == null)
        {
            instance = this;
        }

    }

    void Start()
    {
        fader.FadeInFromBlack();
        //image.SetActive(false);
    }

    // Additional game management code can be added here
}