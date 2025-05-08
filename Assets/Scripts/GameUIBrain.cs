using StarterAssets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIBrain : MonoBehaviour
{
    // Singleton
    public static GameUIBrain Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }


    FirstPersonController _controller;

    private bool isPaused = false;
    public bool IsPaused { get { return isPaused; } }

    private void Start()
    {
        _controller = FindObjectOfType<FirstPersonController>();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Cementery");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void FreezeGame()
    {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;

        ToggleCameraState();

        isPaused = true;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;

        ToggleCameraState();

        isPaused = false;
    }

    private void ToggleCameraState()
    {
        if (_controller != null)
        {
            _controller._CanLook = !_controller._CanLook;
        }
    }
    public void ToggleGameState()
    {
        if (!isPaused)
        {
            FreezeGame();
        }
        else
        {
            ResumeGame();
        }
    }
}
