using System;
using UnityEditor;
using UnityEngine;
using UnityPackages.Slay_The_Spire_Map.OneLine.OneLine.Editor.Slices;
using UnityPackages.Slay_The_Spire_Map.OneLine.OneLine.Editor.Utils;

namespace UnityPackages.Slay_The_Spire_Map.OneLine.OneLine.Editor{
    internal class HeaderDrawer {

        private GUIStyle tableHeaderStyle;

        public HeaderDrawer() {
            tableHeaderStyle = new GUIStyle(EditorStyles.boldLabel);
            tableHeaderStyle.alignment = TextAnchor.MiddleCenter;
            tableHeaderStyle.normal.textColor = GuiUtil.GrayColor;
        }

        public void Draw(SerializedProperty property, Slices.Slices slices){
            slices.AddBefore(new DrawableImpl(null, rect => DrawHeaderInternal(rect, property.displayName)));
        }

        private void DrawHeaderInternal (Rect rect, string header){
            int fittedWidth = (int) Math.Round(rect.width / 6); //units for one litera
            header = header.Substring(0, Math.Min(header.Length, fittedWidth));

            EditorGUI.LabelField(rect, new GUIContent(header, header), tableHeaderStyle);
        }

    }
}
