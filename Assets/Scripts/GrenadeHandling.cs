using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UIElements;

public class GrenadeHandling : MonoBehaviour
{
    private StarterAssetsInputs _inputs;
    private Animator animator;

    private float coolDown = 1.8f;
    private float startTime = 0f;

    public GameObject grenade;
    public Transform spawn;
    public Transform grenadeTarget;//just our aiming sphere
    private playerItemController playerItemController;


    // Start is called before the first frame update
    void Start()
    {
        _inputs = GetComponent<StarterAssetsInputs>();
        animator = GetComponent<Animator>();
        playerItemController = FindObjectOfType<playerItemController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_inputs.grenade && !_inputs.aim && Time.time - startTime > 1.8f && playerItemController.instance.grenadeCount>0)
        {
            startTime = Time.time;
            animator.SetBool("ThrowGrenade", true);
            transform.forward = Vector3.Lerp(transform.forward, grenadeTarget.position - transform.position, Time.deltaTime * 20f);
            StartCoroutine(WaitThenThrow(.5f));
            _inputs.grenade = false;
            playerItemController.instance.grenadeCount--; // decrement grenadeCount on usage
        }
        else
        {
            animator.SetBool("ThrowGrenade", false);

        }
    }

    private IEnumerator WaitThenThrow(float time)
    {
        yield return new WaitForSeconds(time);
        Quaternion q = Quaternion.LookRotation(grenadeTarget.position - spawn.position);
        Instantiate(grenade, spawn.position, q);
    }
}
