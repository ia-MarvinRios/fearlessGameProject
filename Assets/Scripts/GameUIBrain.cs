using StarterAssets;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

public class GameUIBrain : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] FirstPersonController _controller;
    [SerializeField] StarterAssetsInputs _input;

    [Header("UI Containers")]
    [Tooltip("Pause menu canvas or gameobject container")]
    [SerializeField] GameObject _pauseMenu;
    [SerializeField] GameObject _console;

    private bool isPaused = false;
    private bool isOptionsMenuOpen = false;
    private bool isConsoleOpen = false;
    private bool abletoOpen = true;
    public bool IsPaused { get { return isPaused; } }

    private void Update()
    {
        ToggleConsole();
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

    public void LoadCredits()
    {
        SceneManager.LoadScene("Credits", LoadSceneMode.Single);
    }
    public void LoadCreditsDelayed(int secs)
    {
        StartCoroutine(LoadCreditsDelayedCo(secs));
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void FreezeGame()
    {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        ToggleCameraState();

        isPaused = true;
    }
    public void FreezeWithDelay(float time)
    {
       StartCoroutine(FreezeCoroutine(time));
    }
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

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
    public void SetPMenOpenAvail(bool state)
    {
        abletoOpen = state;
    }

    public void TogglePauseMenu()
    {
        if (!isOptionsMenuOpen && abletoOpen)
        {
            _pauseMenu.SetActive(!_pauseMenu.activeSelf);
            Cursor.lockState = _pauseMenu.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = _pauseMenu.activeSelf ? true : false;
        } 
        else return;
    }

    public void ToggleConsole()
    {
        if (_console != null && _input.console)
        {
            if (!isConsoleOpen)
            {
                // Activate the console and unlock the cursor
                _console.SetActive(true);
                isConsoleOpen = true;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                // Deactivate the player controller
                _controller.enabled = false;
            }
            else
            {
                // Deactivate the console and lock the cursor
                _console.SetActive(false);
                isConsoleOpen = false;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                // Reactivate the player controller
                _controller.enabled = true;
            }
        }
    }

    private IEnumerator FreezeCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        FreezeGame();
    }
    private IEnumerator LoadCreditsDelayedCo(float delay)
    {
        yield return new WaitForSeconds(delay);
        LoadCredits();
    }
}
