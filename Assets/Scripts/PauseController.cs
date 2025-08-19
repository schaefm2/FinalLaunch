using Cinemachine;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    private GameObject pausePanel;
    private GameObject infoPanel;

    private GameObject player;
    private GameObject mainCamera;
    private GameObject crosshair;
    private PlayerInput input;
    private Shooting shootingScript;
    private CinemachineBrain cinemachine;
    private bool isPaused;
    private float pausedTimeScale;
    // Start is called before the first frame update
    void Start()
    {
        crosshair = GameObject.Find("Gun_Crosshair");
        player = GameObject.FindGameObjectWithTag("Player");
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        input = player.GetComponent<PlayerInput>();
        shootingScript = player.GetComponent<Shooting>();

        pausePanel = GameObject.Find("PausePanel");
        pausePanel.SetActive(false);
        infoPanel = GameObject.Find("InfoPanel");
        infoPanel.SetActive(false);

        isPaused = false;
        // disable this to stop cam from following cursor
        cinemachine = mainCamera.GetComponent<CinemachineBrain>();
        Cursor.lockState = CursorLockMode.Locked;
        crosshair.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (infoPanel.activeSelf)
            {
                infoBack();
            }
            else if (!isPaused)
            {
                pause();
            }
            else
            {
                unPause();
            }
        }
    }
    // may need to also disable script to successfully pause
    public void pause()
    {
        input.enabled = false;
        shootingScript.enabled = false;
        pausePanel.SetActive(true);
        isPaused = true;
        pausedTimeScale = Time.timeScale;
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        cinemachine.enabled = false;
        crosshair.SetActive(false);
    }

    public void unPause()
    {
        pausePanel.SetActive(false);
        isPaused = false;
        Time.timeScale = pausedTimeScale;
        Cursor.lockState = CursorLockMode.Locked;
        cinemachine.enabled = true;
        crosshair.SetActive(true);
        input.enabled = true;
        shootingScript.enabled = true;
    }

    public void exit()
    {
        unPause();
        SceneManager.LoadScene(0);
    }

    public void infoScreen()
    {
        infoPanel.SetActive(true);
    }

    // back button pressed on info screen
    public void infoBack()
    {
        infoPanel.SetActive(false);
    }
}
