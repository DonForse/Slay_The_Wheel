using System;
using UnityEngine;
using UnityPackages.Slay_The_Spire_Map.OneLine.OneLine.Attributes;

namespace UnityPackages.Slay_The_Spire_Map.OneLine.OneLine.Example.Details.Scripts {
[CreateAssetMenu(menuName = "OneLine/SeparatorExample")]
public class SeparatorExample : ScriptableObject {
    [SerializeField, OneLine]
    private TwoFields first;

    [Space]
    [SerializeField, Separator("[ Separator separates ]"), OneLine]
    private TwoFields second;

    [Serializable]
    public class TwoFields {
        [SerializeField]
        private string first;
        [SerializeField]
        private string second;
    }
}
}