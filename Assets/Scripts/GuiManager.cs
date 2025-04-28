using StarterAssets;
using System.Collections;
using TMPro;
using UnityEngine;

public class GuiManager : MonoBehaviour
{
    private static GuiManager instance;

    [Header("UI Elements")]
    public GameObject inGameCanvas;
    public GameObject pauseMenuCanvas;
    public GameObject lowerPanel;
    public TMP_Text textArea;
    public Transform buttonsLayout;
    public GameObject optionButton;

    [Header("Player Inputs")]
    public GameObject playerInputs;

    private bool isPaused;
    [HideInInspector] internal FirstPersonController inputScript;
    
    TooltipsBehaviour tooltip = null;

    public TMP_Text TextArea { get { return textArea; } set { textArea = value; } }

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

    public void ShowToolTips(RaycastHit hit)
    {
        tooltip = hit.transform.gameObject.GetComponentInChildren<TooltipsBehaviour>();
        tooltip.Instantiate();
    }
    public void HideToolTips()
    {
        if (tooltip != null)
        {
            tooltip.Destroy();
            tooltip = null;
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
}