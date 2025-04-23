using StarterAssets;
using UnityEngine;

public class GuiManager : MonoBehaviour
{
    private static GuiManager instance;

    public Canvas mainMenuCanvas;
    public Canvas pauseMenuCanvas;

    public GameObject playerInputs;
    private FirstPersonController inputScript;

    private bool isPaused;

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
        PauseGame();
    }

    public void ResumeGame()
    {
        isPaused = false;
        Debug.Log("Juego REANUDADO.");
        pauseMenuCanvas.enabled = false;
        Time.timeScale = 1f;
    }

    public void PauseGame()
    {
        if (inputScript._pauseInput == true)
        {
            if (!isPaused)
            {
                isPaused = true;
                Debug.Log("Juego PAUSADO.");
                pauseMenuCanvas.enabled = true;
                Time.timeScale = 0f;
            }

            ResumeGame();
        }
    }
}
