using System;
using System.Collections.Generic;
using UnityEngine;
using UnityPackages.Slay_The_Spire_Map.OneLine.OneLine.Attributes;

namespace UnityPackages.Slay_The_Spire_Map.OneLine.OneLine.Example.Tests.Scripts
{
    [CreateAssetMenu(menuName = "OneLine/ExtendedClassTest")]
    public class ExtendedClassTest : ScriptableObject {

        [SerializeField]
        private NestedClass root;

        [Serializable]
        public abstract class BaseClass {
            [OneLine]
            public List<Vector2> list;
        }

        [Serializable]
        public class NestedClass : BaseClass {
        }
    }
}