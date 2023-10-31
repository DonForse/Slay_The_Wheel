using UnityEngine;
using UnityPackages.Slay_The_Spire_Map.Scripts.ReorderableList.List;
using UnityPackages.Slay_The_Spire_Map.Scripts.ReorderableList.List.Attributes;

namespace UnityPackages.Slay_The_Spire_Map.Scripts.ReorderableList.Example
{
    public class NestedExample : MonoBehaviour
    {

        [Reorderable]
        public ExampleChildList list;

        [System.Serializable]
        public class ExampleChild
        {

            [Reorderable(singleLine = true)]
            public NestedChildList nested;
        }

        [System.Serializable]
        public class NestedChild
        {

            public float myValue;
        }

        [System.Serializable]
        public class NestedChildCustomDrawer
        {

            public bool myBool;
            public GameObject myGameObject;
        }

        [System.Serializable]
        public class ExampleChildList : ReorderableArray<ExampleChild>
        {
        }

        [System.Serializable]
        public class NestedChildList : ReorderableArray<NestedChildCustomDrawer>
        {
        }
    }
}