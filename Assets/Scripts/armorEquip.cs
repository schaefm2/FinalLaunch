using UnityEngine;
using System.Collections;

public class ArmorEquip : MonoBehaviour
{
    [SerializeField] private ScreenFader screenFader; // Reference to the ScreenFader
    [SerializeField] private GameObject swatObject; // Swat model parent
    [SerializeField] private GameObject vanguardObject; // Vanguard model parent
    [SerializeField] private Transform swatArmature; // Swat's Armature
    [SerializeField] private Transform vanguardArmature; // Vanguard's Armature
    [SerializeField] private GameObject interactableObject; // The interactable object to deactivate
    [SerializeField] private DoorController armoryDoor;
    [SerializeField] private Transform otherArmor;
    [SerializeField] private GameObject faderImage;
    [SerializeField] private GameObject currentMenu;
    private bool isPlayerInTrigger = false; // Track if the player is in the trigger zone

    void Start()
    {
        //faderImage.SetActive(false);

    }
    private void OnTriggerEnter(Collider other)
    {
        faderImage.SetActive(true);
        // Check if the player enters the trigger zone
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = true;
            Debug.Log("Player entered the armor equip trigger zone.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        faderImage.SetActive(false);
        // Check if the player exits the trigger zone
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
            Debug.Log("Player exited the armor equip trigger zone.");
        }
    }

    private void Update()
    {
        // Check if the player is in the trigger zone and presses "E"
        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.F))
        {
            EquipArmor();
        }
    }

    public void EquipArmor()
    {
        // Start fade to black, swap models, and fade back
        screenFader.FadeToBlackAndBack(() =>
        {
            // Record Swat's armature position and orientation
            Vector3 swatPosition = swatArmature.position;
            Quaternion swatRotation = swatArmature.rotation;

            // Apply Swat's position and rotation to Vanguard's armature
            vanguardArmature.position = swatPosition;
            vanguardArmature.rotation = swatRotation;

            // Swap models
            swatObject.SetActive(false);
            vanguardObject.SetActive(true);

            if (interactableObject != null)
            {
                Destroy(interactableObject);
                Debug.Log("Interactable object deactivated.");
            }

            armoryDoor.requiresArmoryKey = false;
            armoryDoor.requiresPower = false;

			playerItemController.instance.playerMaxHealth = 20;
			playerItemController.instance.playerHealth = playerItemController.instance.playerMaxHealth;

            Debug.Log("Swat's position and orientation applied to Vanguard.");
            playerItemController.instance.hasArmor = true;
            Destroy(otherArmor.gameObject);
            
        });
        MenuSwap();
        Transform newPlayer = vanguardArmature.transform;
        ZombieController[] zombies = FindObjectsOfType<ZombieController>();

        foreach (ZombieController zombie in zombies)
        {
            zombie.UpdatePlayerReference(newPlayer);
        }
    }
    
    private void MenuSwap()
    {
        Debug.Log("Menuswap");
        currentMenu.SetActive(false);
        Debug.Log("Turning off Menu at non-coroutine");
        currentMenu.SetActive(true);
        Debug.Log("Turning on Menu at non-coroutine");
    }
    

}
