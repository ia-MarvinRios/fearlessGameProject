using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public TMP_Dropdown scenesDropdown;
    private int sceneIndex;

    void Awake()
    {
        Time.timeScale = 1f;
        Debug.Log("Tiempo reanudado: " + Time.timeScale);
    }
    void Start()
    {
        int selectedIndex = scenesDropdown.value;
        string selectedText = scenesDropdown.options[selectedIndex].text;

        Debug.Log("Índice: " + selectedIndex + " Texto: " + selectedText);
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
