using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController2 : MonoBehaviour
{
    public KeyCode interactKey = KeyCode.F;
    public GameController1.GameState gameStateToUnlock;

    private bool playerInRange = false;

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            UnlockGameState();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    private void UnlockGameState()
    {
        GameController1 gameController = GameController1.instance;
        if (gameController != null)
        {
            switch (gameStateToUnlock)
            {
                case GameController1.GameState.ArmoryKey:
                    gameController.armoryKey = true;
                    break;
                case GameController1.GameState.SecurityKey:
                    gameController.securityKey = true;
                    break;
                case GameController1.GameState.Cure:
                    gameController.cure = true;
                    break;
                default:
                    Debug.LogWarning("No game state.");
                    break;
            }

            Debug.Log($"{gameStateToUnlock} has been set to true.");
        }
        Destroy(gameObject);
    }
}
