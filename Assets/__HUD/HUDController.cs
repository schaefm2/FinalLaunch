using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    private Canvas canvas;
    private TextMeshProUGUI hp;
    private TextMeshProUGUI pill;
    private TextMeshProUGUI pistol;
    private TextMeshProUGUI rifle;
    private TextMeshProUGUI shotgun;
    private TextMeshProUGUI grenade;
    private TextMeshProUGUI overheatText;
    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("HUD").GetComponent<Canvas>();
        hp = canvas.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        pill = canvas.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        pistol = canvas.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        rifle = canvas.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>();
        shotgun = canvas.transform.GetChild(4).gameObject.GetComponent<TextMeshProUGUI>();
        grenade = canvas.transform.GetChild(5).gameObject.GetComponent<TextMeshProUGUI>();
        overheatText = canvas.transform.GetChild(6).gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerItemController.instance != null)
        {
            hp.text = playerItemController.instance.playerHealth.ToString()
                + " / " + playerItemController.instance.playerMaxHealth.ToString();
            pill.text = playerItemController.instance.pillCount.ToString();
            pistol.text = playerItemController.instance.pistolAmmo.ToString();
            rifle.text = playerItemController.instance.rifleAmmo.ToString();
            shotgun.text = playerItemController.instance.shotgunAmmo.ToString();
            grenade.text = playerItemController.instance.grenadeCount.ToString();
            overheatText.text = playerItemController.instance.overheatText;
        }
     
    }
}
