using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class NpcController : MonoBehaviour
{
    [Tooltip("Utilizar el nombre de la variable entre llaves {}: {nombre_npc}, {nombre_mision}")]
    public string _titulo;
    public string _nombreNPC; 
    public float velocidadEscritura = 0.05f;
    string mensajeFinal;

    // Modified
    public Gui3D gui3D;
    public bool isTyping = false;

    [Space]
    [Tooltip("Utilizar el nombre de la variable entre llaves {}: {nombre_npc}, {nombre_mision}, {nombre_item}, {cantidad_meta}, {cantidad_recolectada}, {cantidad_restante}")]
    [TextArea(3, 5)]
    public string _mensajeInicio;
    [TextArea(3, 5)]
    public string _mensajeAceptada;
    [TextArea(3, 5)]
    public string _mensajeIncopleta;
    [TextArea(3, 5)]
    public string _mensajeCompletada;
    [TextArea(3, 5)]
    public string _mensajeCerrada;


    [HideInInspector] public int _misionId = 0;

    private GameManager gameManager;
    public UnityEvent MisionInicio;
    public UnityEvent MisionAceptada;
    public UnityEvent MisionIncopleta;
    public UnityEvent MisionCompletada;
    public UnityEvent MisionCerrada;


    [Tooltip("On Trigger Enter")]
    public UnityEvent Interactuar;
    [Tooltip("On Trigger Exit")]
    public UnityEvent DejarInteractuar;

    int _restante;
    string _nombreItem; 

    //Modified
    Collider _other = null;



    private void Start()
    {
        gameManager = GameManager.Instance;
        gameObject.tag = "NPC";
        _nombreItem = gameManager.misiones[_misionId].selectedTag;

        

        string _mensajeTituloFinal = _titulo
            .Replace("{nombre_mision}", gameManager.misiones[_misionId].nombre)
            .Replace("{nombre_npc}", _nombreNPC);

        if(gameManager._txtTitulo) gameManager._txtTitulo.text = _mensajeTituloFinal;

        //Debug.Log("NPC: " + gameManager.misiones[_misionId].nombre);
    }


    public void MostrarMision()
    {
        _restante = gameManager.misiones[_misionId].cantidadMeta - gameManager.misiones[_misionId].cantidadRecolectada;

        if (gameManager.misiones[_misionId].cerrada)
        {
            mensajeFinal = _mensajeCerrada
            .Replace("{nombre_mision}", gameManager.misiones[_misionId].nombre)
            .Replace("{nombre_npc}", _nombreNPC)
            .Replace("{nombre_item}", _nombreItem)
            .Replace("{cantidad_meta}", gameManager.misiones[_misionId].cantidadMeta.ToString())
            .Replace("{cantidad_recolectada}", gameManager.misiones[_misionId].cantidadRecolectada.ToString())
            .Replace("{cantidad_restante}", _restante.ToString());

            Debug.Log(mensajeFinal);
            if (gameManager._txtDialogo) StartCoroutine(EscribirTexto(gameManager._txtDialogo, mensajeFinal));
            if (MisionCerrada!=null)
            {
                MisionCerrada.Invoke();
            }
            return;

        }

        if (gameManager.misiones[_misionId].EstaCompleta)
        {
            //Vector3 offset = transform.forward * 1f + Vector3.up * 1f;  // 1m adelante y 1m arriba
            mensajeFinal = _mensajeCompletada
            .Replace("{nombre_mision}", gameManager.misiones[_misionId].nombre)
            .Replace("{nombre_npc}", _nombreNPC)
            .Replace("{nombre_item}", _nombreItem)
            .Replace("{cantidad_meta}", gameManager.misiones[_misionId].cantidadMeta.ToString())
            .Replace("{cantidad_recolectada}", gameManager.misiones[_misionId].cantidadRecolectada.ToString())
            .Replace("{cantidad_restante}", _restante.ToString());

            //Debug.Log(mensajeFinal);
            if (gameManager._txtDialogo) StartCoroutine(EscribirTexto(gameManager._txtDialogo, mensajeFinal));
            gameManager.misiones[_misionId].cerrada = true;
            /*GameObject recompensa = Instantiate(
            gameManager.misiones[_misionId].recompensa,
            transform.position + offset,  // Clona a 1 metro en la dirección frontal
            Quaternion.identity
        );*/
            if (gameManager.uIController.gridContainer) gameManager.uIController.AgregarMisionesAlGrid();

            if (MisionCompletada != null)
            {
                MisionCompletada.Invoke();
            }

            return;
        }


        if (!gameManager.misiones[_misionId].asignada)
        {
            mensajeFinal = _mensajeInicio
            .Replace("{nombre_mision}", gameManager.misiones[_misionId].nombre)
            .Replace("{nombre_npc}", _nombreNPC)
            .Replace("{nombre_item}", _nombreItem)
            .Replace("{cantidad_meta}", gameManager.misiones[_misionId].cantidadMeta.ToString())
            .Replace("{cantidad_recolectada}", gameManager.misiones[_misionId].cantidadRecolectada.ToString())
            .Replace("{cantidad_restante}", _restante.ToString());

            Debug.Log(mensajeFinal);
            if (gameManager._txtDialogo) StartCoroutine(EscribirTexto(gameManager._txtDialogo, mensajeFinal));


            if (MisionInicio!=null)
            {
                MisionInicio.Invoke();
            }

            return;
        }

        if (gameManager.misiones[_misionId].asignada)
        {


            mensajeFinal = _mensajeIncopleta
            .Replace("{nombre_mision}", gameManager.misiones[_misionId].nombre)
            .Replace("{nombre_npc}", _nombreNPC)
            .Replace("{nombre_item}", _nombreItem)
            .Replace("{cantidad_meta}", gameManager.misiones[_misionId].cantidadMeta.ToString())
            .Replace("{cantidad_recolectada}", gameManager.misiones[_misionId].cantidadRecolectada.ToString())
            .Replace("{cantidad_restante}", _restante.ToString());

            Debug.Log(mensajeFinal+" "+ gameManager.misiones[_misionId].nombre);
            if (gameManager._txtDialogo) StartCoroutine(EscribirTexto(gameManager._txtDialogo, mensajeFinal));

            if (MisionIncopleta != null)
            {
                MisionIncopleta.Invoke();
            }

            return;
        }



    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            _other = other;
            FaceTarget(other.transform);
            gui3D.IsOnRange = true;
        }

    }
    public void Einteract()
    {
        if (gui3D.IsTooltipActive && _other != null)
        {
            Interactuar.Invoke();
        }
        else return;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _other = null;
            isTyping = false;
            DejarInteractuar.Invoke();
            StopAllCoroutines();
            gui3D.IsOnRange = false;
        }
    }


    public void AceptarMision()
    {
        
        if (!gameManager.misiones[_misionId].asignada)
        {
            StopAllCoroutines();
            gameManager.misiones[_misionId].asignada = true;
            mensajeFinal = _mensajeAceptada
            .Replace("{nombre_mision}", gameManager.misiones[_misionId].nombre)
            .Replace("{nombre_npc}", _nombreNPC)
            .Replace("{nombre_item}", _nombreItem)
            .Replace("{cantidad_meta}", gameManager.misiones[_misionId].cantidadMeta.ToString())
            .Replace("{cantidad_recolectada}", gameManager.misiones[_misionId].cantidadRecolectada.ToString())
            .Replace("{cantidad_restante}", _restante.ToString());
            MisionAceptada?.Invoke();
            isTyping = false;
            Debug.Log(mensajeFinal + " " + gameManager.misiones[_misionId].nombre);
            if (gameManager._txtDialogo) StartCoroutine(EscribirTexto(gameManager._txtDialogo, mensajeFinal));
            if (gameManager.uIController.gridContainer) gameManager.uIController.AgregarMisionesAlGrid();
        }

        
        

    }



    private IEnumerator EscribirTexto(TMP_Text _txtMsg, string _msg)
    {
        if (!isTyping)
        {
            isTyping = true;

            _txtMsg.text = ""; // Limpia el texto antes de empezar
            foreach (char letra in _msg)
            {
                _txtMsg.text += letra; // Agrega una letra cada iteración
                yield return new WaitForSeconds(velocidadEscritura);
            }

            isTyping = false;
        }
    }



    void FaceTarget(Transform player)
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = lookRotation;
    }


}
