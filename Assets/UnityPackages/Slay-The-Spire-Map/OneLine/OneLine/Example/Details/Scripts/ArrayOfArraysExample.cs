using System;
using UnityEngine;
using UnityPackages.Slay_The_Spire_Map.OneLine.OneLine.Attributes;

namespace UnityPackages.Slay_The_Spire_Map.OneLine.OneLine.Example.Details.Scripts {
[CreateAssetMenu(menuName = "OneLine/ArrayOfArraysExample")]
public class ArrayOfArraysExample : ScriptableObject {
    [SerializeField, OneLine]
    private NestedArray[] nestedArray;

    [Serializable]
    public class NestedArray {
        [SerializeField, Width(50)]
        private string[] array;
    }
}
}