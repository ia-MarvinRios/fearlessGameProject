using UnityEngine;

public class ScreenRaycast : MonoBehaviour
{
    // -- INSPECTOR --
    [Header("Raycast Configuration")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask interactableLayerMask;
    [SerializeField] private float maxDistance = 3f;

    // Useful Stuff
    private Vector3 screenPoint = new Vector3(Screen.width / 2, Screen.height / 2, 0);
    private RaycastHit hit;

    // Accesibility
    public RaycastHit Hit { get => hit; }
    public delegate void RaycastHitEvent();
    public event RaycastHitEvent OnRaycastHit;

    private void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            Debug.LogWarning("!ScreenRaycast: No camera assigned. Using main camera.");
        }
    }

    private void Update()
    {
        ReCatchCamera();

        DoScreenRaycast();
    }



    /// <summary>
    /// If the reference to the camera gets lost because of the scene switching, this method will get the main camera again.
    /// </summary>
    private void ReCatchCamera()
    {
        // This is only if the reference to the camera gets lost because of the scene switching
        if (mainCamera == null)
        {
            // Gets the main camera in the scene
            mainCamera = Camera.main;
        }
    }

    /// <summary>
    /// Casts a ray from the center of the screen to the interactable layer mask.
    /// </summary>
    private void DoScreenRaycast()
    {
        if (Physics.Raycast(mainCamera.ScreenPointToRay(screenPoint), out RaycastHit a, maxDistance, interactableLayerMask))
        {
            hit = a;
            OnRaycastHit?.Invoke();
        }
        else
        {
            hit = default;
        }
    }

}
