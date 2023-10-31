using UnityEditor;

namespace UnityPackages.Slay_The_Spire_Map.Scripts.ReorderableList.Example.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Example))]
    public class ExampleEditor : UnityEditor.Editor
    {

        private List.Editor.ReorderableList list1;
        private SerializedProperty list2;
        private List.Editor.ReorderableList list3;

        void OnEnable()
        {

            list1 = new List.Editor.ReorderableList(serializedObject.FindProperty("list1"));
            list1.elementNameProperty = "myEnum";

            list2 = serializedObject.FindProperty("list2");

            list3 = new List.Editor.ReorderableList(serializedObject.FindProperty("list3"));
            list3.getElementNameCallback += GetList3ElementName;
        }

        private string GetList3ElementName(SerializedProperty element)
        {

            return element.propertyPath;
        }

        public override void OnInspectorGUI()
        {

            serializedObject.Update();

            //draw the list using GUILayout, you can of course specify your own position and label
            list1.DoLayoutList();

            //Caching the property is recommended
            EditorGUILayout.PropertyField(list2);

            //draw the final list, the element name is supplied through the callback defined above "GetList3ElementName"
            list3.DoLayoutList();

            //Draw without caching property
            EditorGUILayout.PropertyField(serializedObject.FindProperty("list4"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("list5"));

            serializedObject.ApplyModifiedProperties();
        }
    }
}
