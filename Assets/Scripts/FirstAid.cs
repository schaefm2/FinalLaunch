using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class FirstAid : MonoBehaviour
{
    private bool healing;

    public GameObject pillBottle;

    private StarterAssetsInputs _inputs;
    private Animator _anim;

    private void Start()
    {
        healing = false;

        _inputs = GetComponent<StarterAssetsInputs>();
        _anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_inputs.heal && playerItemController.instance.pillCount > 0)
        {
            pillBottle.SetActive(true);

            if (!healing)
            {
                Debug.Log("Coroutine started");
                StartCoroutine(Heal());
                healing = true;
            }
        }
        else
        {
            pillBottle.SetActive(false);
        }
    }

    private IEnumerator Heal()
    {
        float healingDuration = 4f;
        float timeHealed = 0f;
        _anim.SetBool("Heal", true);
        while (timeHealed < healingDuration)
        {
            if (!_inputs.heal || playerItemController.instance.pillCount <= 0)
            {
                Debug.Log("Healing interrupted.");
                healing = false;
                _anim.SetBool("Heal", false);
                yield break;
            }

            timeHealed += Time.deltaTime;  
            yield return null;  
        }

        
        if (timeHealed >= healingDuration)
        {
            Debug.Log($"Player health increase from {playerItemController.instance.playerHealth}");
            playerItemController.instance.playerHealth += 6;

            
            if (playerItemController.instance.playerHealth > playerItemController.instance.playerMaxHealth)
            {
                playerItemController.instance.playerHealth = playerItemController.instance.playerMaxHealth;
            }

            Debug.Log($"Player health increased to {playerItemController.instance.playerHealth}");

            playerItemController.instance.pillCount--;  // Use up one pill
            Debug.Log($"Player used a pill! Remaining pills: {playerItemController.instance.pillCount}");
        }
        _anim.SetBool("Heal", false);

        healing = false;  
    }
}
