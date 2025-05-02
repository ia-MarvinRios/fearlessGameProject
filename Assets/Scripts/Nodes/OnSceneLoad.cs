using UnityEngine;
using UnityEngine.Events;

public class OnSceneLoad : MonoBehaviour
{
    public UnityEvent onSceneLoad;
    private void OnEnable()
    {
        onSceneLoad?.Invoke();
    }
}
