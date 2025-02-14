using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public TMP_Dropdown scenesDropdown;

    void Awake()
    {
        Time.timeScale = 1f;
        Debug.Log("Tiempo reanudado: " + Time.timeScale);
    }

    public void PlayGame()
    {
        int sceneIndex = scenesDropdown.value;
        switch (sceneIndex) {
            case 0:
                SceneManager.LoadScene("rafaVis");
                break;

            case 1:
                SceneManager.LoadScene("Rios_witchLevel");
                break;

            case 2:
                SceneManager.LoadScene("Rios_witchLevel");
                break;

            case 3:
                SceneManager.LoadScene("cementeryLevel");
                break;

            case 4:
                SceneManager.LoadScene("cobertizo");
                break;

            case 5:
                SceneManager.LoadScene("Fachada");
                break;

            default:
                SceneManager.LoadScene("Rios_witchLevel");
                break;

        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
