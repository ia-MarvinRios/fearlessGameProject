using UnityEngine;
using UnityEngine.Events;

public class OnDestroyGameObject : MonoBehaviour
{
    public UnityEvent onDestroy;

    private void OnDestroy()
    {
        onDestroy?.Invoke();
    }
}
