using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

public class Item : MonoBehaviour
{
    //[TagSelector]
    //public string _tagMision;  // Seleccionable desde un Dropdown en el Inspector
    public int _cantidad=1; 


}
/*
// =============================
//       SISTEMA DE TAGS
// =============================
public class TagSelectorAttribute : PropertyAttribute { }

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(TagSelectorAttribute))]
public class TagSelectorDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.propertyType == SerializedPropertyType.String)
        {
            string[] tags = InternalEditorUtility.tags; // Obtener Tags de Unity
            int index = Mathf.Max(0, System.Array.IndexOf(tags, property.stringValue));

            index = EditorGUI.Popup(position, label.text, index, tags);

            property.stringValue = tags[index];  // Asignar el Tag seleccionado
            
        }
        else
        {
            EditorGUI.PropertyField(position, property, label);
        }
    }
}
#endif
*/
