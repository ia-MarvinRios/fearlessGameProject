using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class ProximityChecker : MonoBehaviour
{
    Gui3D gui3D;

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
                gui3D.IsOnRange = false;
            }
        }
        else
            Debug.LogWarning("No existe gui3D en la escena actual.");
        return;
    }
}
