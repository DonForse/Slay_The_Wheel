using UnityEditor;
using UnityEngine;
using UnityPackages.Slay_The_Spire_Map.OneLine.OneLine.Editor.Slices;
using UnityPackages.Slay_The_Spire_Map.OneLine.OneLine.Editor.Utils;

namespace UnityPackages.Slay_The_Spire_Map.OneLine.OneLine.Editor {
    internal class TooltipDrawer {

        public void Draw(SerializedProperty property, Slices.Slices slices){
            var attribute = property.GetCustomAttribute<TooltipAttribute>();
            if (attribute == null) return;

            var slice = new DrawableImpl(rect => EditorGUI.LabelField(rect, new GUIContent("", attribute.tooltip)));
            slices.AddAfter(slice);
        }

    }
}
