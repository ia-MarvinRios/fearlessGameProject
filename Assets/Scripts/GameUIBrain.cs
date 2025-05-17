using StarterAssets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIBrain : MonoBehaviour
{
    /*
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
    */

    FirstPersonController _controller;
    [SerializeField] GameObject _pauseMenu;

    private bool isPaused = false;
    private bool isOptionsMenuOpen = false;
    public bool IsPaused { get { return isPaused; } }

    private void Start()
    {
        _controller = FindObjectOfType<FirstPersonController>();
    }

    private void OnEnable()
    {
        if (SceneManager.GetActiveScene().name == "Cementery")
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            AmbienceSFX music = GetComponent<AmbienceSFX>();
            if (music != null)
            {
                StartCoroutine(music.MainMenuMusic());
            }
        }
        Time.timeScale = 1f;
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Cementery", LoadSceneMode.Single);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
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
            if (isOptionsMenuOpen) return;
            else ResumeGame();
        }
    }

    public void SetOptionsMenuState(bool state)
    {
        isOptionsMenuOpen = state;
    }

    public void TogglePauseMenu()
    {
        if (!isOptionsMenuOpen) _pauseMenu.SetActive(!_pauseMenu.activeSelf); else return;
    }
}
