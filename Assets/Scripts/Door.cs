using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("DOOR SETTINGS")]
    [Space(5)]
    [SerializeField, Range(0, 360)] private float openAngle = 90f;
    [Space(1)]
    [SerializeField] private float openingDuration = 2f;
    [Space(1)]
    [SerializeField] private bool locked = false;

    private bool isRotating = false;

    public bool Locked { get { return locked; } set { locked = value; } }

    public void Interact()
    {
        if (locked)
        {
            Debug.Log("Door is locked.");
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

    IEnumerator OpenDoor(Vector3 playerPos)
    {
        Vector3 d = (playerPos - transform.position).normalized;
        d.y = 0f;

        float dot = Vector3.Dot(transform.forward, d);
        float angle = dot > 0 ? openAngle : -openAngle;

        Quaternion initRot = transform.localRotation;
        Quaternion targetRot = Quaternion.Euler(0f, angle, 0f) * initRot;

        float time = 0f;
        while (time < openingDuration)
        {
            isRotating = true;

            time += Time.deltaTime;
            float t = time / openingDuration;

            transform.rotation = Quaternion.Slerp(initRot, targetRot, t);

            yield return null;
        }

        transform.localRotation = targetRot;
        isRotating = false;
    }
}
