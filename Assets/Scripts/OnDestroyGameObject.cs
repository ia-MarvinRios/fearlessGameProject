using UnityEngine;
using UnityEngine.Events;

public class OnDestroyGameObject : MonoBehaviour
{
    public UnityEvent onDestroy;
    public bool doDestroyOnTouch;

    private void OnDestroy()
    {
        onDestroy?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (doDestroyOnTouch && other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Destroy the game object this script is attached to.
    /// </summary>
    public void DestroyGameObject()
    {
        Destroy(gameObject);
    }

    public void DestroyChildrenObjects()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        foreach (RectTransform child in transform)
        {
           Destroy(child.gameObject);
        }
    }
}