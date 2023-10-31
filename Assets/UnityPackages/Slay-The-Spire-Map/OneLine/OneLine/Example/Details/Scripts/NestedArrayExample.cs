using System;
using UnityEngine;
using UnityPackages.Slay_The_Spire_Map.OneLine.OneLine.Attributes;

namespace UnityPackages.Slay_The_Spire_Map.OneLine.OneLine.Example.Details.Scripts {
[CreateAssetMenu(menuName = "OneLine/NestedArrayExample")]
public class NestedArrayExample : ScriptableObject {
    [SerializeField, OneLine]
    private NestedArray nestedArray;

    [Serializable]
    public class NestedArray {
        [SerializeField]
        private string[] array;
    }
}
}