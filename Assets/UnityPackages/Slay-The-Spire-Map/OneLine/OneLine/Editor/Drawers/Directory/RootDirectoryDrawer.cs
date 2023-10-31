using UnityEditor;
using UnityEngine;
using UnityPackages.Slay_The_Spire_Map.OneLine.OneLine.Attributes;
using UnityPackages.Slay_The_Spire_Map.OneLine.OneLine.Editor.Utils;
using UnityPackages.Slay_The_Spire_Map.OneLine.RectEx;

namespace UnityPackages.Slay_The_Spire_Map.OneLine.OneLine.Editor {
    internal class RootDirectoryDrawer : DirectoryDrawer {


        public RootDirectoryDrawer(DrawerProvider getDrawer) : base(getDrawer) {
        }

        public override void AddSlices(SerializedProperty property, Slices.Slices slices){
            getDrawer(property).AddSlices(property, slices);
        }

        public Rect DrawPrefixLabel(Rect rect, SerializedProperty property){
            var hideLabel = property.GetCustomAttribute<HideLabelAttribute>();
            if (hideLabel == null) {
                rect = IndentWithLabel(rect, property, true);
            }
            return rect;
        }

        private static Rect IndentWithLabel(Rect rect, SerializedProperty property, bool drawLabel) {
            var rects = rect.CutFromLeft(EditorGUIUtility.labelWidth, 0);

            if (drawLabel) {
                var labelRect = rects[0]
                                    .CutFromBottom(EditorGUIUtility.singleLineHeight)[1];
                DrawLabel(labelRect, property);
            }
            return rects[1];
        }

        private static void DrawLabel(Rect rect, SerializedProperty property){
            string tooltip =  property.GetCustomAttribute<TooltipAttribute>()
                                .IfPresent(x => x.tooltip)
                                .OrElse(null);

            var label = new GUIContent(property.displayName, tooltip);
            EditorGUI.LabelField(rect, label);
        }

    }
}
