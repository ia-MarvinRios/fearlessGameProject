using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string nextScene = "";
    Collider _other;
    Gui3D gui3D;

    private void Start()
    {
        gui3D = FindObjectOfType<Gui3D>();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            _other = other;
        }

    }
    public void Einteract()
    {
        if (gui3D.IsTooltipActive && _other != null && nextScene != "")
        {
            SceneManager.LoadScene(nextScene);
        }
        else return;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _other = null;
        }
    }
}
