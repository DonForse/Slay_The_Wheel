using System;
using UnityEngine;
using UnityPackages.Slay_The_Spire_Map.OneLine.OneLine.Attributes;

namespace UnityPackages.Slay_The_Spire_Map.OneLine.OneLine.Example.Details.Scripts {
[CreateAssetMenu(menuName = "OneLine/FixedLengthExample")]
public class FixedLengthExample : ScriptableObject {
    [SerializeField, OneLine]
    private ImmutableLengthArray arrayWithImmutableLength;

    [Serializable]
    public class ImmutableLengthArray {
        [SerializeField, ArrayLength(7)]
        private string[] array;
    }
}
}