using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public Light red;

    public Light green;

    public KeyCode interactKey = KeyCode.F;
    public AudioSource audio;
    public GameController1 gameController;
    private bool inRange = false;
    // Start is called before the first frame update
    void Start()
    {
        gameController = FindObjectOfType<GameController1>();
        if (red != null)
        {
            red.enabled = true;
        }

        if (green != null)
        {
            green.enabled = false;
        }

        if (audio != null)
        {
            audio.Stop();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (inRange && Input.GetKeyDown(interactKey))
        {
            ToggleLight();
        } 
    }

    private void ToggleLight()
    {
        if (red != null && green != null)
        {
            bool redOn = red.enabled;
            red.enabled = !redOn;
            green.enabled = redOn;

            if (gameController != null)
            {
                gameController.powerOn = green.enabled;
            }

            if (audio != null)
            {
                if (green.enabled)
                {
                    audio.Play();
                }
                else
                {
                    audio.Stop();
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
        }
    }
}
