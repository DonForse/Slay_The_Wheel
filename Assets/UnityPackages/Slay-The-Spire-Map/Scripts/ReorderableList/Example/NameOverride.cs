﻿using UnityEngine;
using UnityPackages.Slay_The_Spire_Map.Scripts.ReorderableList.List;
using UnityPackages.Slay_The_Spire_Map.Scripts.ReorderableList.List.Attributes;

namespace UnityPackages.Slay_The_Spire_Map.Scripts.ReorderableList.Example
{
    public class NameOverride : MonoBehaviour
    {

        public string nameOverride = "Car";
        public string nestedNameOverride = "Car Part";

        [Reorderable(null, "Car", null)]
        public ExampleChildList autoNameList;

        [Reorderable]
        public DynamicExampleChildList dynamicNameList;

        [System.Serializable]
        public class ExampleChild
        {

            [Reorderable(null, "Car Part", null)]
            public StringList nested;
        }

        [System.Serializable]
        public class DynamicExampleChild
        {

            [Reorderable]
            public StringList nested;
        }

        [System.Serializable]
        public class ExampleChildList : ReorderableArray<ExampleChild>
        {
        }

        [System.Serializable]
        public class DynamicExampleChildList : ReorderableArray<DynamicExampleChild>
        {
        }

        [System.Serializable]
        public class StringList : ReorderableArray<string>
        {
        }
    }
}