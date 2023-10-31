using UnityEditor;
using UnityEngine;

namespace UnityPackages.Slay_The_Spire_Map.Scripts.Editor
{
    [CustomEditor(typeof(MapManager))]
    public class MapManagerInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var myScript = (MapManager)target;

            GUILayout.Space(10);

            if (GUILayout.Button("Generate"))
                myScript.GenerateNewMap();
        }
    }
}