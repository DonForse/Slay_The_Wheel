using System;
using UnityEngine;
using UnityPackages.Slay_The_Spire_Map.OneLine.OneLine.Attributes;

namespace UnityPackages.Slay_The_Spire_Map.OneLine.OneLine.Example.Details.Scripts {
[CreateAssetMenu(menuName = "OneLine/WidthExample")]
public class WidthExample : ScriptableObject {
    [SerializeField, OneLine]
    private RootField root;

    [Serializable]
    public class RootField {
        [SerializeField, Width(70)]
        private string first;
        [SerializeField] // by default width = 0
        private string second;
        [SerializeField, Weight(2), Width(25)]
        private string third;
        [SerializeField, Width(10000000)]
        private NestedField forth;
    }

    [Serializable]
    public class NestedField {
        [SerializeField]
        private string first;
    }
}
}