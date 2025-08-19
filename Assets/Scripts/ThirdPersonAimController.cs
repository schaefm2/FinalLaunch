using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.Animations.Rigging;

public class ThirdPersonAimController : MonoBehaviour
{
    [SerializeField] private Rig rig;
    [SerializeField] private CinemachineVirtualCamera aimVirtualCam;
    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform debugTransform;

    
    [SerializeField] private TwoBoneIKConstraint RifleLeftHandConstraint;
    [SerializeField] private MultiAimConstraint RifleRightConstraint;
    [SerializeField] private MultiAimConstraint RifleSpineConstraint;

    [SerializeField] private TwoBoneIKConstraint PistolLeftHandConstraint;
    [SerializeField] private MultiAimConstraint PistolRightConstraint;
    [SerializeField] private MultiAimConstraint PistolSpineConstraint;

    private NewThirdPersonController thirdPersonController;

    private StarterAssetsInputs starterAssetsInputs;

    private Animator anim; 
    
    // Start is called before the first frame update
    private void Awake()
    {
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        thirdPersonController = GetComponent<NewThirdPersonController>();
        anim = GetComponent<Animator>();   
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mouseWorldPosition = Vector3.zero;
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            debugTransform.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
        }
        if (starterAssetsInputs.aim)
        {
            aimVirtualCam.gameObject.SetActive(true);
            thirdPersonController.SetSensitivity(aimSensitivity);
            thirdPersonController.SetRotateOnMove(false);

            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);

            anim.SetLayerWeight(2, 1f);
            anim.SetLayerWeight(0, 0f);

            // for using a rifle.
            if (starterAssetsInputs.weapon2 || starterAssetsInputs.weapon3)
            {
                RifleLeftHandConstraint.weight = 1f;
                RifleRightConstraint.weight = 1f;
                RifleSpineConstraint.weight = 1f;
                
            }
            
            //masks for using a pisol
            else
            {
                PistolRightConstraint.weight = 1f;  
                PistolLeftHandConstraint.weight = 1f;
                PistolSpineConstraint.weight = 1f;  
            }
            
            thirdPersonController.SetRotateOnMove(false);

            rig.weight = 1f;

        }
        else
        {
            thirdPersonController.SetSensitivity(normalSensitivity);
            thirdPersonController.SetRotateOnMove(true);
            thirdPersonController.SetRotateOnMove(true);

            aimVirtualCam.gameObject.SetActive(false);

            anim.SetLayerWeight(1, 1f);
            anim.SetLayerWeight(2, 0f);
            anim.SetLayerWeight(3, 0f);


            rig.weight = 0f; 
            
            PistolLeftHandConstraint.weight = 0f;  
            RifleLeftHandConstraint.weight = 0f;
            RifleRightConstraint.weight = 0f;
            PistolRightConstraint.weight = 0f;
            RifleSpineConstraint.weight = 0f;
            PistolSpineConstraint.weight = 0f;
            
        }


    }
}