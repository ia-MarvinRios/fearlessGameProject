using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour
{
    [Header("DOOR SETTINGS")]
    [Space(5)]
    [SerializeField, Range(0, 360)] private float openAngle = 90f;
    [Space(1)]
    [SerializeField] private float openingDuration = 2f;
    [Space(1)]
    [SerializeField] private bool locked = false;
    [Space(1)]
    [SerializeField] private float interactionCooldown = 0.5f;
    [Space(5)]
    [SerializeField] private AudioClip openSound;

    [Header("INTERACTION")]
    [Space(5)]
    [SerializeField] private GameObject tooltip;
    [SerializeField] private GameObject lockedTooltip;
    [Space(5)]
    public UnityEvent onDoorClosed;
    public UnityEvent onDoorOpened;
    
    private bool isRotating = false;
    private bool isOpen = false;
    Quaternion startRot = Quaternion.identity;

    public bool Locked { get { return locked; } set { locked = value; } }

    private void Start()
    {
        startRot = transform.localRotation;
    }

    public void Interact()
    {
        if (locked)
        {
            Debug.Log("Door is locked.");
            onDoorClosed?.Invoke();
            return;
        }
        if (!isRotating)
        {
            // Get the player position
            Vector3 playerPos = Camera.main.transform.position;

            // Start the door opening coroutine
            StartCoroutine(OpenDoor(playerPos));
        }
        else return;
    }

    public void ToggleTooltip()
    {
        if (tooltip != null && lockedTooltip != null)
        {
            tooltip.SetActive(false);
            lockedTooltip.SetActive(false);
        }
    }
    public void ShowTooltip()
    {
        if (tooltip != null && lockedTooltip != null)
        {
            if (locked)
            {
                tooltip.SetActive(false);
                lockedTooltip.SetActive(true);
            }
            else
            {
                tooltip.SetActive(true);
                lockedTooltip.SetActive(false);
            }
        }
        else
            Debug.LogWarning("Tooltip or Locked Tooltip is not assigned in the inspector.");
    }

    IEnumerator OpenDoor(Vector3 playerPos)
    {
        isRotating = true;

        AudioSource.PlayClipAtPoint(openSound, transform.position);

        Quaternion targetRot = startRot;
        Quaternion initRot = transform.localRotation;

        if (!isOpen)
        {
            Vector3 d = (playerPos - transform.position).normalized;
            d.y = 0f;

            float dot = Vector3.Dot(transform.forward, d);
            float angle = dot > 0 ? -openAngle : openAngle;
            
            targetRot = Quaternion.Euler(0f, initRot.eulerAngles.y + angle, 0f);
            Debug.Log(targetRot.eulerAngles);
        }

        float time = 0f;
        while (time < openingDuration)
        {
            time += Time.deltaTime;
            float t = time / openingDuration;

            transform.localRotation = Quaternion.Slerp(initRot, targetRot, t);

            yield return null;
        }

        transform.localRotation = targetRot;
        isOpen = !isOpen;
        onDoorOpened?.Invoke();
        
        yield return new WaitForSeconds(interactionCooldown);

        isRotating = false;
    }

    public void DoorLocked(bool lockedState)
    {
        locked = lockedState;
        Debug.Log($"Lock State of door: {gameObject.name} has changed to: {locked}");
    }
}
