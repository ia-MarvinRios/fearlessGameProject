using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static GameManager;
using UnityEngine.UI;
using System.Linq;

public class UIController : MonoBehaviour
{
    public TMP_Text _txtTitulo;
    public TMP_Text _txtDialogo;
    public Transform gridContainer; // Contenedor con GridLayoutGroup
    [Tooltip("Para los nombres del prefab use: txtTitulo, txtProgreso y txtEdtado; imgIcon para el icono de la mision")]
    public GameObject itemPrefab; // Prefab del ítem de misión

    [HideInInspector]
    public List<Mision> misiones = new List<Mision>();
    [Space]
    public TMP_Text _txtPuntos;
    public TMP_Text _txtVida;
    public TMP_Text _txtFuerza;
    public TMP_Text _txtMagia;
    public TMP_Text _txtVelocidad;
    [Space]
    public Slider _HealthBar;
    public Slider _MagicBar;

    public void AgregarMisionesAlGrid()
    {
        
        // Limpiar Grid antes de agregar nuevas misiones
        foreach (Transform child in gridContainer)
        {
            Destroy(child.gameObject);
        }

        // Recorrer la lista de misiones y agregar cada una al Grid
        foreach (var mision in misiones)
        {
            //MODIFIED: Si la misión está cerrada, no se asigna
            if (mision.cerrada)
            {
                mision.asignada = false;
            }

            if (mision.asignada)
            {
                GameObject newItem = Instantiate(itemPrefab, gridContainer);

                TMP_Text[] textos = newItem.GetComponentsInChildren<TMP_Text>(true); // 'true' busca en objetos desactivados
                Slider barraProgreso = newItem.GetComponentInChildren<Slider>(true); //Busca el Slider
                Image[] iconosUI = newItem.GetComponentsInChildren<Image>(true); // Busca el Image UI

                TMP_Text titulo = textos.FirstOrDefault(t => t.name == "txtTitulo");
                TMP_Text progreso = textos.FirstOrDefault(t => t.name == "txtProgreso");
                TMP_Text estado = textos.FirstOrDefault(t => t.name == "txtEstado");
                Image iconoUI = iconosUI.FirstOrDefault(t => t.name == "imgIcon");


                if (titulo) titulo.text = mision.nombre;
                if (progreso) progreso.text = mision.cantidadRecolectada + "/" + mision.cantidadMeta;
                if (estado) estado.text = mision.cerrada ? "Completada" : "En progreso";
                if (barraProgreso) barraProgreso.value = (float)mision.cantidadRecolectada / mision.cantidadMeta;
                if (iconoUI && mision._icono != null) iconoUI.sprite = mision._icono;


            }
        }
        
    }


}
