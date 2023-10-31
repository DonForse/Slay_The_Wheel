using System;
using System.Collections.Generic;
using UnityEditor;
using UnityPackages.Slay_The_Spire_Map.OneLine.OneLine.Attributes;
using UnityPackages.Slay_The_Spire_Map.OneLine.OneLine.Editor.Utils;

namespace UnityPackages.Slay_The_Spire_Map.OneLine.OneLine.Editor {
    internal class FixedArrayDrawer : ComplexFieldDrawer {

        public FixedArrayDrawer(DrawerProvider getDrawer) : base(getDrawer) {
        }

        protected override IEnumerable<SerializedProperty> GetChildren(SerializedProperty property){
            return property.GetArrayElements();
        }

        public override void AddSlices(SerializedProperty property, Slices.Slices slices){
            ModifyLength(property);
            base.AddSlices(property, slices);
        }

        protected virtual int ModifyLength(SerializedProperty property) {
            var attribute = property.GetCustomAttribute<ArrayLengthAttribute>();
            if (attribute == null) {
                var message = string.Format("Can not find ArrayLengthAttribute at property {0)", property.propertyPath);
                throw new InvalidOperationException(message);
            }
            property.arraySize = attribute.Length;
            return property.arraySize;
        }

    }
}
