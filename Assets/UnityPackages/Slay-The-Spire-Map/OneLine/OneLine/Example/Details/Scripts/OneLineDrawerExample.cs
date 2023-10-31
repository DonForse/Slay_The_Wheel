using System;
using UnityEngine;
using UnityPackages.Slay_The_Spire_Map.OneLine.OneLine.Attributes;

namespace UnityPackages.Slay_The_Spire_Map.OneLine.OneLine.Example.Details.Scripts {
[CreateAssetMenu(menuName = "OneLine/OneLineDrawerExample")]
public class OneLineDrawerExample : ScriptableObject {

    [SerializeField]
    private RootField root;
    [Space]
    [SerializeField, HideLabel]
    private RootField[] rootArray;

    [Serializable]
    public class RootField {
        [SerializeField]
        private string first;
        [SerializeField]
        private Color second;
        [SerializeField]
        private NestedField third;
    }

    [Serializable]
    public class NestedField {
        [SerializeField, Tooltip("This tooltip is very useful")]
        private bool first;
        [SerializeField]
        private Vector2 second;
    }
}
}