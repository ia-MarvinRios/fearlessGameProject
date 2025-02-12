using UnityEditor;
using UnityEngine;

public class IrahetaTools
{
    [MenuItem("Iraheta Tools/Crear GameManager")]
    private static void CreateGameManager()
    {
        // Verifica si ya existe un objeto llamado "GameManager" en la escena
        if (GameObject.Find("GameManager") == null)
        {
            AgregarTag("NPC");
            AgregarTag("EXP");
            AgregarTag("ENEMIGO");

            GameObject gameManager = new GameObject("GameManager");
            gameManager.AddComponent<GameManager>();
            Undo.RegisterCreatedObjectUndo(gameManager, "Crear GameManager");
            Debug.Log("GameObject 'GameManager' creado en la escena.");
        }
        else
        {
            Debug.LogWarning("Ya existe un GameObject llamado 'GameManager' en la escena.");
        }
    }

    [MenuItem("Iraheta Tools/Player/PlayerItemController")]
    private static void AddItemPlayerCOntroller()
    {
        if (Selection.activeGameObject != null && Selection.activeGameObject.CompareTag("Player"))
        {
            GameObject selectedObject = Selection.activeGameObject;
            if (selectedObject.GetComponent<PlayerItemController>() == null)
            {
                selectedObject.AddComponent<PlayerItemController>();
                Debug.Log("PlayerItemController añadido a: " + selectedObject.name);
            }
            else
            {
                Debug.LogWarning("El objeto ya tiene el script  adjunto.");
            }
        }
        else
        {
            Debug.LogWarning("Selecciona el Player en la jerarquía antes de agregar el script.");
        }
    }




    /*
    [MenuItem("Iraheta Tools/ENEMIGO/Convertir en ENEMIGO")]
    private static void CrearEnemigo()
    {
        if (Selection.activeGameObject != null && !Selection.activeGameObject.CompareTag("Player"))
        {

            GameObject selectedObject = Selection.activeGameObject;
            if (selectedObject.GetComponent<EnemyController>() == null)
            {
                selectedObject.tag = "ENEMIGO";
                selectedObject.AddComponent<EnemyController>();
            }

            // Verifica si el objeto ya tiene un SphereCollider
            SphereCollider sphereCollider = selectedObject.GetComponent<SphereCollider>();

            if (sphereCollider == null)
            {
                // Añadir SphereCollider si no existe
                sphereCollider = selectedObject.AddComponent<SphereCollider>();
                sphereCollider.radius = 2f; // Radio de 2 metros
                sphereCollider.isTrigger = true;

                //  Debug.Log($"SphereCollider agregado a {selectedObject.name} con radio 2m y como Trigger.");
            }

        }



    }

    */




    [MenuItem("Iraheta Tools/NPC/Convertir en NPC")]
    private static void CrearNPC()
    {

        if (Selection.activeGameObject != null && !Selection.activeGameObject.CompareTag("Player"))
        {
            GameObject selectedObject = Selection.activeGameObject;

            if (selectedObject.GetComponent<NpcController>() == null)
            {
                selectedObject.tag = "NPC";
                selectedObject.AddComponent<NpcController>();

                // Verifica si el objeto ya tiene un SphereCollider
                SphereCollider sphereCollider = selectedObject.GetComponent<SphereCollider>();

                if (sphereCollider == null)
                {
                    // Añadir SphereCollider si no existe
                    sphereCollider = selectedObject.AddComponent<SphereCollider>();
                    sphereCollider.radius = 2f; // Radio de 2 metros
                    sphereCollider.isTrigger = true;

                    //  Debug.Log($"SphereCollider agregado a {selectedObject.name} con radio 2m y como Trigger.");
                }

            }
            else
            {
                Debug.LogWarning("El objeto ya es NPC");
            }


        }


    }



    [MenuItem("Iraheta Tools/ITEM/Convertir en Exp")]
    private static void CrearEXP()
    {

        if (Selection.activeGameObject != null && !Selection.activeGameObject.CompareTag("Player"))
        {
            GameObject selectedObject = Selection.activeGameObject;

            if (selectedObject.GetComponent<ExpController>() == null)
            {
                selectedObject.tag = "EXP";
                selectedObject.AddComponent<ExpController>();

                // Verifica si el objeto ya tiene un SphereCollider
                SphereCollider sphereCollider = selectedObject.GetComponent<SphereCollider>();

                if (sphereCollider == null)
                {
                    // Añadir SphereCollider si no existe
                    sphereCollider = selectedObject.AddComponent<SphereCollider>();
                    sphereCollider.radius = 1f; // Radio de 0.5 metros
                    sphereCollider.isTrigger = true;

                    
                }

            }
            else
            {
                Debug.LogWarning("El objeto ya es EXP");
            }


        }


    }



    [MenuItem("Iraheta Tools/ITEM/Convertir en Item")]
      private static void ShowTagSelectionWindow()
      {
        if (Selection.activeGameObject != null && !Selection.activeGameObject.CompareTag("Player"))
        {
            TagSelectionWindow window = EditorWindow.GetWindow<TagSelectionWindow>("Seleccionar Tag");
            window.Show();
        }

      }


        public static void AgregarTag(string nuevoTag)
    {
        // Obtiene el asset de Tags
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty tagsProp = tagManager.FindProperty("tags");

        // Verifica si el Tag ya existe
        for (int i = 0; i < tagsProp.arraySize; i++)
        {
            if (tagsProp.GetArrayElementAtIndex(i).stringValue == nuevoTag)
            {
                Debug.Log($"El Tag '{nuevoTag}' ya existe.");
                return;
            }
        }

        // Agrega el nuevo Tag
        tagsProp.InsertArrayElementAtIndex(tagsProp.arraySize);
        tagsProp.GetArrayElementAtIndex(tagsProp.arraySize - 1).stringValue = nuevoTag;



        // Guarda cambios
        tagManager.ApplyModifiedProperties();
        Debug.Log($"Tag '{nuevoTag}' agregado exitosamente.");
    }



}

public class TagSelectionWindow : EditorWindow
{
    private int selectedTagIndex = 0;
    private string[] tags;

    private void OnEnable()
    {
        tags = UnityEditorInternal.InternalEditorUtility.tags;
//        GameManager gameManager = (GameManager)target;

    }

    private void OnGUI()
    {
        GUILayout.Label("Asignar TAG:", EditorStyles.boldLabel);
        selectedTagIndex = EditorGUILayout.Popup("Tag:", selectedTagIndex, tags);

        if (GUILayout.Button("Convertir en ITEM"))
        {
            convertirItem();
        }
    }


    private void convertirItem()
    {
        string selectedTag = tags[selectedTagIndex];
        GameObject selectedObject = Selection.activeGameObject;
        selectedObject.tag = selectedTag;
        if (selectedObject.GetComponent<Item>() == null)
        {
            selectedObject.AddComponent<Item>();
        }

        // Verifica si el objeto ya tiene un SphereCollider
        SphereCollider sphereCollider = selectedObject.GetComponent<SphereCollider>();

        if (sphereCollider == null)
        {
            // Añadir SphereCollider si no existe
            sphereCollider = selectedObject.AddComponent<SphereCollider>();
            sphereCollider.radius = 1f; // Radio de 0.5 metros
            sphereCollider.isTrigger = true;


        }
        else
        {
            if (sphereCollider.radius < 1)
            {
                sphereCollider.radius = 1f;

            }

            if (!sphereCollider.isTrigger)
            {
                sphereCollider.isTrigger = true;
            }

        }

        Debug.Log($"Se ha convertido en ITEM con tag '{selectedTag}'.");
    }

    private void OpenDoors()
    {

        if (tags.Length == 0)
        {
            Debug.LogWarning("No hay tags disponibles.");
            return;
        }

        string selectedTag = tags[selectedTagIndex];
        GameObject[] doors = GameObject.FindGameObjectsWithTag(selectedTag);

        foreach (GameObject door in doors)
        {

            door.SetActive(true);
        }

        Debug.Log($"Puertas con el tag '{selectedTag}' abiertas.");
        Close();
    }
    



}

public class ConvertirEnemigoWindow : EditorWindow
{
    private GameObject recompensa;
    private GameObject selectedObject;

    [MenuItem("Iraheta Tools/ENEMIGO/Convertir en ENEMIGO")]
    private static void CrearEnemigo()
    {
        if (Selection.activeGameObject != null && !Selection.activeGameObject.CompareTag("Player"))
        {
            GameObject selectedObject = Selection.activeGameObject;

            if (selectedObject.GetComponent<EnemyController>() == null)
            {
                selectedObject.tag = "ENEMIGO";
                selectedObject.AddComponent<EnemyController>();
            }

            // Verifica si el objeto ya tiene un SphereCollider
            SphereCollider sphereCollider = selectedObject.GetComponent<SphereCollider>();

            if (sphereCollider == null)
            {
                // Añadir SphereCollider si no existe
                sphereCollider = selectedObject.AddComponent<SphereCollider>();
                sphereCollider.radius = 2f; // Radio de 2 metros
                sphereCollider.isTrigger = true;
            }

            // Abrir la ventana para asignar la recompensa
            ConvertirEnemigoWindow window = GetWindow<ConvertirEnemigoWindow>("Asignar Recompensa");
            window.selectedObject = selectedObject;
            window.Show();
        }
    }

    private void OnGUI()
    {
        GUILayout.Label("Asignar Recompensa al Enemigo", EditorStyles.boldLabel);

        // Selección de GameObject
        recompensa = (GameObject)EditorGUILayout.ObjectField("Recompensa", recompensa, typeof(GameObject), true);

        // Botón para asignar la recompensa al EnemyController
        if (GUILayout.Button("Asignar Recompensa"))
        {
            if (selectedObject != null)
            {
                EnemyController enemyController = selectedObject.GetComponent<EnemyController>();
                if (enemyController != null)
                {
                    enemyController._recompensa = recompensa;
                    Debug.Log($"Recompensa asignada a {selectedObject.name}: {recompensa.name}");
                    Close(); // Cerrar la ventana
                }
                else
                {
                    Debug.LogError("El objeto seleccionado no tiene un EnemyController.");
                }
            }
        }
    }
}