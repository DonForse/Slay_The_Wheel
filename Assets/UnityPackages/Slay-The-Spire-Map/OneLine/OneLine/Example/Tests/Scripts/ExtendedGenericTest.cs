using System;
using System.Collections.Generic;
using UnityEngine;
using UnityPackages.Slay_The_Spire_Map.OneLine.OneLine.Attributes;

namespace UnityPackages.Slay_The_Spire_Map.OneLine.OneLine.Example.Tests.Scripts
{
    [CreateAssetMenu(menuName = "OneLine/ExtendedGenericTest")]
    public class ExtendedGenericTest : ScriptableObject {

        [SerializeField]
        private ChildClass root;

        [Serializable]
        public abstract class BaseClass<T> {
            [OneLine]
            public List<T> list;
        }

        [Serializable]
        public class ChildClass : BaseClass<Vector2> {
        }
    }
}