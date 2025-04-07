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
    }

    void Update()
    {
        PauseGame();
    }

    public void PlayGame()
    {
        Debug.Log("Jugar");
    }

    public void PauseGame()
    {
        if (inputScript.PauseInput())
        {
            if (!isPaused)
            {
                isPaused = true;
                Debug.Log("Juego PAUSADO.");
            }
            else if (isPaused)
            {
                isPaused = false;
                Debug.Log("Juego REANUDADO.");
            }
        }
    }
}
