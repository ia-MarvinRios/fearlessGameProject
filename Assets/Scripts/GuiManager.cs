using StarterAssets;
using UnityEngine;

public class GuiManager : MonoBehaviour
{
    private static GuiManager instance;

    public Canvas mainMenuCanvas;
    public Canvas pauseMenuCanvas;
    public GameObject playerInputs;
    public LayerMask interactableLayerMask;

    private bool isPaused;
    private FirstPersonController inputScript;
    Vector3 screenCenter = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f);
    TooltipsBehaviour tooltip = null;

    // Inputs
    [HideInInspector] public bool interInput = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        isPaused = false;
        inputScript = playerInputs.GetComponent<FirstPersonController>();

        pauseMenuCanvas.enabled = false;
    }

    void Update()
    {
        ShowToolTips();
    }

    private void ShowToolTips()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)), out RaycastHit hit, 10f, interactableLayerMask))
        {
            tooltip = hit.transform.gameObject.GetComponentInChildren<TooltipsBehaviour>();
            tooltip.Instantiate();

            if (hit.transform.gameObject.CompareTag("Door") && interInput)
            {
                Vector3 currentRot = hit.transform.rotation.eulerAngles;
                if (currentRot.y == 0)
                    hit.transform.Rotate(new Vector3(0, 90, 0));
                else if (currentRot.y == 90)
                    hit.transform.Rotate(new Vector3(0, -90, 0));
                else if (currentRot.y == 180)
                    hit.transform.Rotate(new Vector3(0, -90, 0));
                else if (currentRot.y == 270)
                    hit.transform.Rotate(new Vector3(0,-90,0));
                interInput = false;
            }
        }
        else
        {
            if (tooltip != null)
            {
                tooltip.Destroy();
                tooltip = null;
            }
        }
    }

    public void ResumeGame()
    {
        isPaused = false;
        Debug.Log("Juego REANUDADO.");
        pauseMenuCanvas.enabled = false;
        Time.timeScale = 1f;
        inputScript._canLook = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void PauseGame()
    {
        if (!isPaused)
        {
            isPaused = true;
            Debug.Log("Juego PAUSADO.");
            pauseMenuCanvas.enabled = true;
            Time.timeScale = 0f;
            inputScript._canLook = false;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            ResumeGame();
        }
    }
}
