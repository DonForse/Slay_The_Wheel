﻿using System;
using UnityEngine;
using UnityPackages.Slay_The_Spire_Map.OneLine.OneLine.Attributes;

namespace UnityPackages.Slay_The_Spire_Map.OneLine.OneLine.Example.Details.Scripts {
[CreateAssetMenu(menuName = "OneLine/HighlightExample")]
public class HighlightExample : ScriptableObject {
    [SerializeField, OneLine, Highlight]
    private string rootField;
    [SerializeField, OneLine] 
    private HighlightedFields nestedFields;

    [Serializable]
    public class HighlightedFields {
        [SerializeField, Highlight(0, 1, 0)]
        private string first;
        [SerializeField, Highlight(0, 0, 1)]
        private string second;
        [SerializeField]
        private string third;
        [SerializeField, Highlight(1, 1, 0)]
        private string fourth;
    }
}
}