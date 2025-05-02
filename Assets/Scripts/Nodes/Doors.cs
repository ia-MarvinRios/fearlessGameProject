using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(ProximityChecker))]
public class Doors : MonoBehaviour
{
    private Gui3D gui3D;
    private ProximityChecker proximityChecker;

    private bool isRotating = false;

    [Header("Door Settings")]
    public float rotationDuration = 1f;
    private bool unlocked = false;
    public bool Unlocked { get { return unlocked; } set { unlocked = value; } }

    [Header("Events")]
    public UnityEvent onDoorInteraction;
    public UnityEvent onDoorLocked;

    private void Awake()
    {
        gui3D = FindObjectOfType<Gui3D>();
        proximityChecker = GetComponent<ProximityChecker>();
    }

    public void EInteract()
    {
        if (gui3D.IsTooltipActive && proximityChecker._Other != null)
        {
            onDoorInteraction?.Invoke();
        }
    }

    public void OpenDoor()
    {
        Quaternion currentRot = transform.localRotation;
        Quaternion targetRot = Quaternion.identity;

        if (currentRot.y == 0)
            targetRot = Quaternion.Euler(0, 90, 0);
        else if (currentRot.y == 90)
            targetRot = Quaternion.Euler(0, -90, 0);
        else if (currentRot.y == 180)
            targetRot = Quaternion.Euler(0, -90, 0);
        else if (currentRot.y == 270)
            targetRot = Quaternion.Euler(0, -90, 0);

        if (!isRotating)
        {
            StartCoroutine(RotateDoor(currentRot, targetRot));
        }
        else return;
    }

    public void OpenDoorWithRequirement()
    {
        if (unlocked)
        {
            Quaternion currentRot = transform.localRotation;
            Quaternion targetRot = Quaternion.identity;

            if (currentRot.y == 0)
                targetRot = Quaternion.Euler(0, 90, 0);
            else if (currentRot.y == 90)
                targetRot = Quaternion.Euler(0, -90, 0);
            else if (currentRot.y == 180)
                targetRot = Quaternion.Euler(0, -90, 0);
            else if (currentRot.y == 270)
                targetRot = Quaternion.Euler(0, -90, 0);

            if (!isRotating)
            {
                StartCoroutine(RotateDoor(currentRot, targetRot));
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
