using StarterAssets;
using UnityEngine;

[RequireComponent(typeof(ScreenRaycast))]
public class RaycastEvents : MonoBehaviour
{
    [Header("Raycast Events")]
    [Space(5)]
    [SerializeField] private ScreenRaycast scrRaycast;
    [SerializeField] StarterAssetsInputs input;


    void Start()
    {
        scrRaycast.OnRaycastHit += CheckHit;
    }

    private void OnDestroy()
    {
        scrRaycast.OnRaycastHit -= CheckHit;
    }

    private void CheckHit()
    {
        if (scrRaycast.Hit.collider != null && input.interact)
        {
            switch (scrRaycast.Hit.collider.tag)
            {
                case "Door":
                    scrRaycast.Hit.collider.GetComponent<Door>().Interact();
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
}
