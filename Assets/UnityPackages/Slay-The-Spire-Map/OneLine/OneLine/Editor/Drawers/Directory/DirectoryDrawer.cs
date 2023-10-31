using System.Collections.Generic;
using UnityEditor;
using UnityPackages.Slay_The_Spire_Map.OneLine.OneLine.Editor.Utils;

namespace UnityPackages.Slay_The_Spire_Map.OneLine.OneLine.Editor {
    internal class DirectoryDrawer : ComplexFieldDrawer {

        public DirectoryDrawer(DrawerProvider getDrawer) : base(getDrawer) {
        }

        protected override IEnumerable<SerializedProperty> GetChildren(SerializedProperty property){
            return property.GetChildren();
        }

        public override void AddSlices(SerializedProperty property, Slices.Slices slices){
            highlight.Draw(property, slices);
            base.AddSlices(property, slices);
        }

    }
}
