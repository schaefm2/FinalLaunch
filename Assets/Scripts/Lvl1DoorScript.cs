using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Animator animator;
    private bool isPlayerNear = false;
    private bool isDoorOpen = false;
    public GameObject interactPrompt;
    public GameObject doorPanel1;
    public GameObject doorPanel2;
    private GameController1 gameController;
    public bool requiresPower = false;
    public bool requiresArmoryKey = false;
    public bool requiresSecurityKey = false;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("Animator component not found on " + gameObject.name);
        }

        animator.SetBool("character_nearby", isDoorOpen);

        if (interactPrompt != null)
        {
            interactPrompt.SetActive(false);
        }

        gameController = GameController1.instance;

        if (gameController == null)
        {
            Debug.LogError("GameController instance not found.");
        }
    }

    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.F))
        {
            if (requiresPower && (gameController == null || !gameController.powerOn))
            {
                Debug.Log("Cannot operate the door. Power is off.");
                return;
            }

            if (requiresArmoryKey && (gameController == null || !gameController.armoryKey))
            {
                Debug.Log("Cannot open the door. Armory key is required.");
                return;
            }
            
            if (requiresSecurityKey && (gameController == null || !gameController.securityKey))
            {
                Debug.Log("Cannot open the door. Security key is required.");
                return;
            }
            
            isDoorOpen = !isDoorOpen;

            if (!isDoorOpen)
            {
                SetDoorPanelsActive(true);
            }
            
            animator.SetBool("character_nearby", isDoorOpen);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;

            if (interactPrompt != null)
            {
                interactPrompt.SetActive(true);
            }

        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;

            if (interactPrompt != null)
            {
                interactPrompt.SetActive(false);
            }

        }
    }

    public void OnDoorOpened()
    {
        if (isDoorOpen)
        {
            SetDoorPanelsActive(false);
        }
    }

    private void SetDoorPanelsActive(bool isActive)
    {
        if (doorPanel1 != null)
        {
            doorPanel1.SetActive(isActive);
        }

        if (doorPanel2 != null)
        {
            doorPanel2.SetActive(isActive);
        }
    }
}
