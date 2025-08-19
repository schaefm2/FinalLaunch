using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TriggerMessage : MonoBehaviour
{

    public TextMeshProUGUI messageText;
    private float messageDisplayTime = 5f;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            StartCoroutine(DisplayMessage("Shoot all 10 targets with the raycasting physics gun to open the door."));
        }
    }

    private IEnumerator DisplayMessage(string message) {
        messageText.text = message;
        messageText.gameObject.SetActive(true);
        yield return new WaitForSeconds(messageDisplayTime);
        messageText.gameObject.SetActive(false);
    }
}
