using UnityEngine;

public class DontDestroyOnLoadObject : MonoBehaviour
{
    private static DontDestroyOnLoadObject Instance;

    private void Awake()
    {
        Instance = FindObjectOfType<DontDestroyOnLoadObject>();

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            return;
        }
    }
}
