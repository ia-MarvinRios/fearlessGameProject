using UnityEditor;
using UnityEngine;
using UnityEditor.Events;
using static GameManager;
using UnityEngine.Events;
using TMPro;

public class MissionComplete : MonoBehaviour
{
    [HideInInspector]
    public GameManager gameManager;

    [HideInInspector] // Para no mostrar este campo en el Inspector por defecto
    public int misionSeleccionadaIndex = 0;
    public TMP_Text _txtDialogo;
    public TMP_Text _txtTitulo;
    public UnityEvent MisionCompletada;
    public UnityEvent MisionIncompleta;

    private void Start()
    {
        
        
    }




    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player") && GameManager.Instance != null && gameManager.Misiones.Count > 0)
        {

            if (gameManager.misiones[misionSeleccionadaIndex].EstaCompleta)
            {
                if (gameManager.misiones[misionSeleccionadaIndex].cerrada)
                {
                    MisionCompletada.Invoke();
                }
                else
                {
                    Debug.Log("Has completado la mision, pero debes entregarla");
                }
            }

        }

    }







    private void OnGUI()
    {

        if (GameManager.Instance != null && gameManager.Misiones.Count > 0)
        {
            // Crear un dropdown dinámico con las misiones
            string[] nombresMisiones = new string[gameManager.Misiones.Count];
            for (int i = 0; i < gameManager.Misiones.Count; i++)
            {
                nombresMisiones[i] = gameManager.Misiones[i].nombre;
            }

            // Mostrar el dropdown para seleccionar la misión
            misionSeleccionadaIndex = EditorGUILayout.Popup("Selecciona la Misión", misionSeleccionadaIndex, nombresMisiones);

            // Acceder a la misión seleccionada
            Mision misionSeleccionada = gameManager.Misiones[misionSeleccionadaIndex];
            
        }
    }





}
