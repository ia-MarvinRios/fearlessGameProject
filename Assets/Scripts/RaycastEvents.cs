using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

[RequireComponent(typeof(ScreenRaycast))]
public class RaycastEvents : MonoBehaviour
{
    [Header("Raycast Events")]
    [Space(5)]
    [SerializeField] private ScreenRaycast scrRaycast;
    [SerializeField] StarterAssetsInputs input;

    PlayerInput playerInput;
    Door door = null;
    GoblinAI goblin = null;

    private void Awake()
    {
        playerInput = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();
    }

    void Start()
    {
        scrRaycast.OnRaycastHit += CheckHit;
        scrRaycast.OnRaycastMiss += CheckMiss;

        GoblinAI.OnGoblinAttack += GoblinAway;
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

                case "GOBLIN":
                    goblin = scrRaycast.Hit.collider.GetComponent<GoblinAI>();
                    goblin.IsPlayerLooking = true;
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
        // -- GOBLIN --
        else if (goblin != null)
        {
            goblin.IsPlayerLooking = false;
            goblin = null;
        }
        else
            return;
    }

    void GoblinAway()
    {
        if (goblin != null && playerInput.actions["Light"].IsPressed())
        {
            Destroy(goblin.gameObject);
        }
    }

}
