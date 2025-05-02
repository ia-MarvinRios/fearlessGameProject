using StarterAssets;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(ProximityChecker))]
public class Doors : MonoBehaviour
{
    private Gui3D gui3D;
    private StarterAssetsInputs inputs;
    private ProximityChecker proximityChecker;

    private bool isRotating = false;

    [Header("Door Settings")]
    public float rotationDuration = 1f;
    private bool unlocked = false;
    public bool Unlocked { get { return unlocked; } set { unlocked = value; } }
    private bool isOpen = false;
    public Quaternion targetRot = Quaternion.identity;
    private Quaternion currentRot = Quaternion.identity;

    [Header("Events")]
    public UnityEvent onDoorInteraction;
    public UnityEvent onDoorLocked;

    private void Awake()
    {
        gui3D = FindObjectOfType<Gui3D>();
        proximityChecker = GetComponent<ProximityChecker>();
        inputs = FindObjectOfType<StarterAssetsInputs>();
    }
    
    private void Start()
    {
        currentRot = transform.localRotation;
    }

    public void Update()
    {
        if (gui3D.IsTooltipActive && proximityChecker._Other != null && inputs.interact)
        {
            onDoorInteraction?.Invoke();
        }
    }

    public void OpenDoor()
    {
        if (!isRotating)
        {
            if (!isOpen)
            {
                StartCoroutine(RotateDoor(currentRot, targetRot));
                isOpen = true;
            }
            else
            {
                StartCoroutine(RotateDoor(targetRot, currentRot));
                isOpen = false;
            }
        }
        else return;
    }

    public void OpenDoorWithRequirement()
    {
        if (unlocked)
        {
            if (!isRotating)
            {
                if (!isOpen)
                {
                    StartCoroutine(RotateDoor(currentRot, targetRot));
                    isOpen = true;
                }
                else
                {
                    StartCoroutine(RotateDoor(targetRot, currentRot));
                    isOpen = false;
                }
            }
            else return;
        }
        else
        {
            onDoorLocked?.Invoke();
            Debug.Log("Esta puerta está cerrada...");
        }
    }

    public void UnlockDoor()
    {
        unlocked = true;
    }
    public void LockDoor()
    {
        unlocked = false;
    }

    private IEnumerator RotateDoor(Quaternion currentRot, Quaternion targetRot)
    {
        isRotating = true;

        float time = 0f;
        while (time < rotationDuration)
        {
            time += Time.deltaTime;
            float t = time / rotationDuration;

            // Rotación suavizada
            transform.localRotation = Quaternion.Slerp(currentRot, targetRot, t);

            yield return null;
        }

        transform.localRotation = targetRot;

        isRotating = false;
    }
}
