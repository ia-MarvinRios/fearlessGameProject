using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Rios_witchLevel");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
