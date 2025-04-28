using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System.Collections.Generic;


[System.Serializable]
public class InputActionEvent
{
    [HideInInspector]
    public string actionName;
    public UnityEvent<InputAction.CallbackContext> onPerformed = new UnityEvent<InputAction.CallbackContext>();
    //private bool puedeEjecutarAcciones = true; // Variable que controla la ejecución
}
[RequireComponent(typeof(PlayerInput))]
public class PlayerActionsController : MonoBehaviour
{
    private PlayerInput playerInput;

    [SerializeField]
    private List<InputActionEvent> actionEvents = new List<InputActionEvent>();

    private Dictionary<string, UnityEvent<InputAction.CallbackContext>> actionEventDictionary;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        if (playerInput == null)
        {
            Debug.LogError("No se encontró PlayerInput en el GameObject.");
            return;
        }

        actionEventDictionary = new Dictionary<string, UnityEvent<InputAction.CallbackContext>>();

        // Crear el diccionario de eventos
        foreach (var actionEvent in actionEvents)
        {
            actionEventDictionary[actionEvent.actionName] = actionEvent.onPerformed;
        }

        // Suscribirse a cada acción automáticamente
        foreach (var action in playerInput.actions)
        {
            if (actionEventDictionary.ContainsKey(action.name))
            {
                action.performed += ctx => actionEventDictionary[action.name]?.Invoke(ctx);
            }
        }
    }



    /// <summary>
    /// Se ejecuta en modo diseño cuando se modifica algo en el Inspector
    /// </summary>
    private void OnValidate()
    {
        playerInput = GetComponent<PlayerInput>();

        if (playerInput == null || playerInput.actions == null)
            return;

        // Obtener todas las acciones y agregarlas si no existen en la lista
        foreach (var action in playerInput.actions)
        {
            if (!actionEvents.Exists(a => a.actionName == action.name))
            {
                actionEvents.Add(new InputActionEvent { actionName = action.name });
            }
        }

        // Eliminar acciones que ya no existen en el InputActionAsset
        actionEvents.RemoveAll(a => playerInput.actions[a.actionName] == null);
    }
}
