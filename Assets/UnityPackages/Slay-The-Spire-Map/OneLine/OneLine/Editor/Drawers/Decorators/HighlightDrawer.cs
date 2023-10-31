using UnityEditor;
using UnityPackages.Slay_The_Spire_Map.OneLine.OneLine.Attributes;
using UnityPackages.Slay_The_Spire_Map.OneLine.OneLine.Editor.Slices;
using UnityPackages.Slay_The_Spire_Map.OneLine.OneLine.Editor.Utils;
using UnityPackages.Slay_The_Spire_Map.OneLine.RectEx;

namespace UnityPackages.Slay_The_Spire_Map.OneLine.OneLine.Editor {
    internal class HighlightDrawer {

        public void Draw(SerializedProperty property, Slices.Slices slices){
            var attribute = property.GetCustomAttribute<HighlightAttribute>();
            if (attribute == null) return;

            var slice = new DrawableImpl(rect => GuiUtil.DrawRect(rect.Extend(1), attribute.Color));
            slices.AddBefore(slice);
        }

    }
}
