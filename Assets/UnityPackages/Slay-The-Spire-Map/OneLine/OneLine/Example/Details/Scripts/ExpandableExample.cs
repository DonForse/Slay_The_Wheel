using System;
using UnityEngine;
using UnityPackages.Slay_The_Spire_Map.OneLine.OneLine.Attributes;

namespace UnityPackages.Slay_The_Spire_Map.OneLine.OneLine.Example.Details.Scripts {
[CreateAssetMenu(menuName = "OneLine/ExpandableExample")]
public class ExpandableExample : ScriptableObject {

    [SerializeField, Expandable]
    private UnityEngine.Object withoutOneLine;

    [SerializeField, OneLine]
    private TwoFields withOneLine;

    [Serializable]
    public class TwoFields {
        [SerializeField, ReadOnlyExpandable]
        private ScriptableObject first;
        [SerializeField, Expandable]
        private UnityEngine.Object second;
    }
}
}