using UnityEditor;
using UnityEngine;
using UnityPackages.Slay_The_Spire_Map.OneLine.OneLine.Editor.Slices;
using UnityPackages.Slay_The_Spire_Map.OneLine.OneLine.Editor.Utils;

namespace UnityPackages.Slay_The_Spire_Map.OneLine.OneLine.Editor {
    internal class SpaceDrawer {

        public void Draw(SerializedProperty property, Slices.Slices slices){
            property.GetCustomAttribute<SpaceAttribute>()
                    .IfPresent(x => slices.Add(new SliceImpl(0, x.height, rect => {})));
        }

    }
}
