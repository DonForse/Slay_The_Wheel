using System;
using UnityEngine;
using UnityPackages.Slay_The_Spire_Map.OneLine.OneLine.Attributes;

namespace UnityPackages.Slay_The_Spire_Map.OneLine.OneLine.Example.Details.Scripts {
[CreateAssetMenu(menuName = "OneLine/HideLabelExample")]
public class HideLabelExample : ScriptableObject {
    [SerializeField, OneLine, HideLabel]
    private ThreeFields thisSelfDocumentedFieldNameWillNotBeShownInTheInspector;

    [Serializable]
    public class ThreeFields {
        [SerializeField]
        private string first;
        [SerializeField]
        private string second;
        [SerializeField]
        private string third;
    }
}
}