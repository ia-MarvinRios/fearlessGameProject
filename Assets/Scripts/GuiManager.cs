using StarterAssets;
using System.Collections;
using UnityEngine;

public class GuiManager : MonoBehaviour
{
    private static GuiManager instance;

    [Header("UI Elements")]
    public GameObject inGameCanvas;
    public GameObject pauseMenuCanvas;
    public GameObject lowerPanel;
    public Transform buttonsLayout;
    public GameObject optionButton;

    [Header("Player Inputs")]
    public GameObject playerInputs;

    [Header("Interactions Mask")]
    public LayerMask interactableLayerMask;

    private bool isPaused;
    [HideInInspector] internal FirstPersonController inputScript;
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

        pauseMenuCanvas.SetActive(false);
        lowerPanel.SetActive(false);
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

            InteractionCaseExecuter(hit);
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
        inGameCanvas.SetActive(true);
        pauseMenuCanvas.SetActive(false);
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
            inGameCanvas.SetActive(false);
            pauseMenuCanvas.SetActive(true);
            Time.timeScale = 0f;
            inputScript._canLook = false;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            ResumeGame();
        }
    }

    private void InteractionCaseExecuter(RaycastHit hit)
    {
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
                hit.transform.Rotate(new Vector3(0, -90, 0));
            interInput = false;
        }
        if (hit.transform.gameObject.CompareTag("NPC") && interInput && hit.transform.GetComponent<NPC>().didDialogueStart == false)
        {
            interInput = lowerPanel.activeInHierarchy;
            StartCoroutine(StartNPCDialogue(hit));
        }
    }

    public IEnumerator StartNPCDialogue(RaycastHit hit)
    {
        hit.transform.GetComponent<NPC>().StartDialogue();
        yield return new WaitForSeconds(1f);
    }
}