using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    void Awake()
    {
        Time.timeScale = 1f;
        Debug.Log("Tiempo reanudado: " + Time.timeScale);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Rios_witchLevel");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
