using UnityEngine;
using UnityEditor;
using System.Collections.Generic;




public class GameManager : MonoBehaviour
{
    public static GameManager Instance;  // Singleton para fácil acceso

    [System.Serializable]
    public class Mision
    {
        public string nombre;
        public string selectedTag;
        public int cantidadMeta;
        public int cantidadRecolectada;
        public bool asignada = false;
        public bool cerrada = false;
        public NpcController npcController;
        public GameObject recompensa; 

        public bool EstaCompleta => cantidadRecolectada >= cantidadMeta;
    }

    [Header("Misiones")]
    public List<Mision> misiones = new List<Mision>();
    

    public List<Mision> Misiones => misiones;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        for (int i = 0; i < misiones.Count; i++)
        {
            misiones[i].npcController._misionId = i;


        }
    }




    public void RecolectarItem(string tagMision, int cantidad)
    {
        for (int i = 0; i < misiones.Count; i++)
        {
            if (misiones[i].selectedTag == tagMision)
            {
                if (!misiones[i].asignada) return;

                misiones[i].cantidadRecolectada += cantidad;
                Debug.Log($"Misión '{misiones[i].nombre}': {misiones[i].cantidadRecolectada}/{misiones[i].cantidadMeta}");

                if (misiones[i].EstaCompleta)
                {
                    CompletarMision(i);
                }
                return;
            }
        }

        Debug.LogWarning($"No se encontró ninguna misión con el tag '{tagMision}'.");
    }

    private void CompletarMision(int index)
    {
        Debug.Log($"Misión '{misiones[index].nombre}' completada.");
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    private SerializedProperty misionesProp;

    private void OnEnable()
    {
        misionesProp = serializedObject.FindProperty("misiones");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("Gestión de Misiones", EditorStyles.boldLabel);

        if (GUILayout.Button("Añadir Nueva Misión"))
        {
            misionesProp.arraySize++;
        }

        for (int i = 0; i < misionesProp.arraySize; i++)
        {
            SerializedProperty misionProp = misionesProp.GetArrayElementAtIndex(i);
            SerializedProperty nombreProp = misionProp.FindPropertyRelative("nombre");
            SerializedProperty selectedTagProp = misionProp.FindPropertyRelative("selectedTag");
            SerializedProperty cantidadMetaProp = misionProp.FindPropertyRelative("cantidadMeta");
            SerializedProperty cantidadRecolectadaProp = misionProp.FindPropertyRelative("cantidadRecolectada");
            SerializedProperty npcControllerProp = misionProp.FindPropertyRelative("npcController");
            SerializedProperty recompensaProp = misionProp.FindPropertyRelative("recompensa");

            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField($"Misión {i + 1}", EditorStyles.boldLabel);
            nombreProp.stringValue = EditorGUILayout.TextField("Nombre", nombreProp.stringValue);

            // Lista desplegable de Tags
            string[] tags = UnityEditorInternal.InternalEditorUtility.tags;
            int selectedIndex = System.Array.IndexOf(tags, selectedTagProp.stringValue);
            selectedIndex = EditorGUILayout.Popup("Tag", selectedIndex, tags);
            if (selectedIndex >= 0 && selectedIndex < tags.Length)
            {
                selectedTagProp.stringValue = tags[selectedIndex];
            }

            cantidadMetaProp.intValue = EditorGUILayout.IntField("Cantidad Meta", cantidadMetaProp.intValue);
            cantidadRecolectadaProp.intValue = EditorGUILayout.IntField("Cantidad Recolectada", cantidadRecolectadaProp.intValue);
            npcControllerProp.objectReferenceValue = EditorGUILayout.ObjectField("NPC Controller", npcControllerProp.objectReferenceValue, typeof(NpcController), true);
            recompensaProp.objectReferenceValue = EditorGUILayout.ObjectField("recompensa", recompensaProp.objectReferenceValue, typeof(GameObject), true);



            if (GUILayout.Button("Eliminar Misión", GUILayout.MaxWidth(150)))
            {
                misionesProp.DeleteArrayElementAtIndex(i);
            }

            EditorGUILayout.EndVertical();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
