using UnityEngine;
using UnityEngine.Events;

public class MissionObjectiveChecker : MonoBehaviour
{
    public UnityEvent triggerNextMission;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            triggerNextMission?.Invoke();
        }
    }
}
