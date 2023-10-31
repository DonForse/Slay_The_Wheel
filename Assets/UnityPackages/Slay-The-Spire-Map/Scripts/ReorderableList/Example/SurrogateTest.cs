using UnityEngine;
using UnityPackages.Slay_The_Spire_Map.Scripts.ReorderableList.List;
using UnityPackages.Slay_The_Spire_Map.Scripts.ReorderableList.List.Attributes;

namespace UnityPackages.Slay_The_Spire_Map.Scripts.ReorderableList.Example
{
    public class SurrogateTest : MonoBehaviour
    {

        [SerializeField]
        private MyClass[] objects;

        [SerializeField, Reorderable(surrogateType = typeof(GameObject), surrogateProperty = "gameObject")]
        private MyClassArray myClassArray;

        [System.Serializable]
        public class MyClass
        {

            public string name;
            public GameObject gameObject;
        }

        [System.Serializable]
        public class MyClassArray : ReorderableArray<MyClass>
        {
        }
    }
}