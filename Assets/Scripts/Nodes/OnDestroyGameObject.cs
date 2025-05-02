using UnityEngine;
using UnityEngine.Events;

public class OnDestroyGameObject : MonoBehaviour
{
    public UnityEvent onDestroyEvent;

    private void OnDestroy()
    {
        onDestroyEvent?.Invoke();
    }
}
