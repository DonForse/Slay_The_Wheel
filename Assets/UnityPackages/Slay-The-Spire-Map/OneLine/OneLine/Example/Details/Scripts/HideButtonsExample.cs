﻿using System;
using UnityEngine;
using UnityPackages.Slay_The_Spire_Map.OneLine.OneLine.Attributes;

namespace UnityPackages.Slay_The_Spire_Map.OneLine.OneLine.Example.Details.Scripts {
[CreateAssetMenu(menuName = "OneLine/HideButtonsExample")]
public class HideButtonsExample : ScriptableObject {
    [SerializeField, OneLine]
    private ArrayHidesButtons arrayWithElements;
    [SerializeField, OneLine]
    private ArrayHidesButtons zeroLengthArray;

    [Serializable]
    public class ArrayHidesButtons {
        [SerializeField, HideButtons]
        private string[] array;
    }
}
}