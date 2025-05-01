using UnityEngine;

public class ScreenRaycast : MonoBehaviour
{
    [Header("Raycast Configuration")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask interactableLayerMask;
    [SerializeField] private float maxDistance = 3f;
    private Vector3 screenPoint = new Vector3(Screen.width / 2, Screen.height / 2, 0);
    [HideInInspector] public RaycastHit hit;

    Gui3D gui3D;

    private void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            Debug.LogWarning("!ScreenRaycast: No camera assigned. Using main camera.");
        }

        gui3D = transform.GetComponent<Gui3D>();
    }

    private void Update()
    {
        // This is only if the reference to the camera gets lost because of the scene switching
        if (mainCamera == null)
        {
            // Gets the main camera in the scene
            mainCamera = Camera.main;
        }

        if (Physics.Raycast(mainCamera.ScreenPointToRay(screenPoint), out hit, maxDistance, interactableLayerMask))
        {
            CheckHit();
        }
        else
        {
            hit = default;
            gui3D.IsPointing = false;
        }
    }

    private void CheckHit()
    {
        if (hit.collider != null)
        {
            if (hit.transform.CompareTag("NPC") || hit.transform.CompareTag("Door") || hit.transform.CompareTag("Hacha"))
            {
                gui3D.IsPointing = true;
            }
        }
    }
}
