using UnityEditor;
using static GameManager;

#if (UNITY_EDITOR)
[CustomEditor(typeof(MissionComplete))]
public class MissionCompleteEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MissionComplete script = (MissionComplete)target;

        if (script.gameManager != null && script.gameManager.Misiones.Count > 0)
        {
            // Crear un dropdown dinámico con las misiones
            string[] nombresMisiones = new string[script.gameManager.Misiones.Count];
            for (int i = 0; i < script.gameManager.Misiones.Count; i++)
            {
                nombresMisiones[i] = script.gameManager.Misiones[i].nombre;
            }

            // Mostrar el dropdown para seleccionar la misión
            script.misionSeleccionadaIndex = EditorGUILayout.Popup("Selecciona la Misión", script.misionSeleccionadaIndex, nombresMisiones);

            // Acceder a la misión seleccionada
            Mision misionSeleccionada = script.gameManager.Misiones[script.misionSeleccionadaIndex];
            //Debug.Log("Misión seleccionada: " + misionSeleccionada.nombre);
        }
    }
}
#endif
