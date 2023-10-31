using System.Collections.Generic;
using UnityEditor;

namespace UnityPackages.Slay_The_Spire_Map.OneLine.OneLine.Editor.Utils {
    // Crutch: see https://github.com/slavniyteo/one-line/issues/9
    internal class ArraysSizeObserver {

        private Dictionary<string, int> arraysSizes = new Dictionary<string, int>();
        
        public bool IsArraySizeChanged(SerializedProperty property){
            var arrayName = property.propertyPath.Split('.')[0];
            property = property.serializedObject.FindProperty(arrayName);

            return property.IsReallyArray() && 
                   IsRealArraySizeChanged(arrayName, property.arraySize);
        }

        public bool IsRealArraySizeChanged(string arrayName, int currentArraySize){
            if (! arraysSizes.ContainsKey(arrayName)){
                arraysSizes[arrayName] = currentArraySize;
            }
            else if (arraysSizes[arrayName] != currentArraySize){
                arraysSizes[arrayName] = currentArraySize;
                return true;
            }

            return false;
        }
    }
}
