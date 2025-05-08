using StarterAssets;
using UnityEngine;

[RequireComponent(typeof(ScreenRaycast))]
public class RaycastEvents : MonoBehaviour
{
    [Header("Raycast Events")]
    [Space(5)]
    [SerializeField] private ScreenRaycast scrRaycast;
    [SerializeField] StarterAssetsInputs input;

    Door door = null;

    void Start()
    {
        scrRaycast.OnRaycastHit += CheckHit;
        scrRaycast.OnRaycastMiss += CheckMiss;
    }

    private void OnDestroy()
    {
        scrRaycast.OnRaycastHit -= CheckHit;
        scrRaycast.OnRaycastMiss -= CheckMiss;
    }

    private void CheckHit()
    {
        if (scrRaycast.Hit.collider != null)
        {
            switch (scrRaycast.Hit.collider.tag)
            {
                // -- DOORS --
                case "Door":
                    door = scrRaycast.Hit.collider.GetComponent<Door>();
                    door.ShowTooltip();

                    if (input.interact) { door.Interact(); }
                    break;

                case "Enemy":
                    Debug.Log("Hit an enemy: " + scrRaycast.Hit.collider.name);
                    break;

                default:
                    Debug.Log("Hit an object: " + scrRaycast.Hit.collider.name);
                    break;
            }
        }
        else
            return;
    }

    private void CheckMiss()
    {
        // -- DOORS --
        if (door != null)
        {
            door.ToggleTooltip();
            door = null;
        }
        else
            return;
    }

}
