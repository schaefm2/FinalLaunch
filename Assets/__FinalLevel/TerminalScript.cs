using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TerminalScript : MonoBehaviour
{
    private GameObject FToHack;
    private TextMeshPro text;
    // Start is called before the first frame update
    void Start()
    {
        //FToHack = transform.Find("FToHack").gameObject;
        FToHack = transform.GetChild(0).gameObject;
        text = FToHack.GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        if (text.enabled && Input.GetKeyDown(KeyCode.F) && !SingletonMiniGame.Instance.finishedMiniGame)
        {
            SceneManager.LoadScene(4);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            text.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            text.enabled = false;
        }
    }
}
