using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class ProximityChecker : MonoBehaviour
{
    Gui3D gui3D;
    private Collider _other;
    public Collider _Other { get { return _other; } }

    private void Start()
    {
        gui3D = FindObjectOfType<Gui3D>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (gui3D != null)
        {
            if (other.CompareTag("Player"))
            {
                _other = other;
                gui3D.IsOnRange = true;
            }
        }
        else
            Debug.LogWarning("No existe gui3D en la escena actual.");
        return;
    }
    private void OnTriggerExit(Collider other)
    {
        if (gui3D != null)
        {
            if (other.CompareTag("Player"))
            {
                _other = null;
                gui3D.IsOnRange = false;
            }
        }
        else
            Debug.LogWarning("No existe gui3D en la escena actual.");
        return;
    }
}
