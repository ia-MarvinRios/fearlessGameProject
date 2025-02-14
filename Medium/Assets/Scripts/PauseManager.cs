using UnityEngine;
using UnityEngine.SceneManagement;
using StarterAssets;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenu;
    private bool isPaused = false;

    public StarterAssetsInputs playerInputs;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    void PauseGame()
    {
        isPaused = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        playerInputs.TogglePauseMenu();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
