using System;
using UnityEditor;
using UnityEngine;
using UnityPackages.Slay_The_Spire_Map.OneLine.OneLine.Attributes;

#pragma warning disable 0414

namespace UnityPackages.Slay_The_Spire_Map.OneLine.OneLine.Example.Details.Scripts {
[CreateAssetMenu(menuName = "OneLine/CustomPropertyDrawerExample")]
public class CustomPropertyDrawerExample : ScriptableObject {

    [Separator("Use With Children = True")]
    [SerializeField, OneLine]
    private DirectDrawer directDrawer;
    [SerializeField, OneLine]
    private AttributeDrawer attributeDrawer;

    [Separator("Use With Children = False")]
    [SerializeField, OneLine]
    private DirectWithoutChildren directWithoutChildren;
    [SerializeField, OneLine]
    private AttributeWithoutChildren attributeWithoutChildren;

    [Separator("BuiltIn Attributes Drawer")]
    [SerializeField, OneLine]
    private BuiltinDrawer builtInAttributes;

    #region Direct Custom Drawer

    [Serializable]
    public class DirectDrawer {
        [SerializeField]
        private Parent parent;
        [SerializeField]
        private Child child;

        [Serializable]
        public class Parent {
            [SerializeField]
            private string first = "Default drawer";
        }

        [Serializable]
        public class Child : Parent {
            [SerializeField]
            private string second = "Default drawer"; // Shall not be drown
        }
    }

    #endregion

    #region Custom Drawer on the Attribute

    [Serializable]
    public class AttributeDrawer {
        [SerializeField, Parent]
        private AttributeExample parent;
        [SerializeField, Child]
        private AttributeExample child;

        [Serializable]
        public class AttributeExample {
            [SerializeField]
            private string first = "Default drawer";
        }

        public class Parent : PropertyAttribute {

        }

        public class Child : Parent {

        }
    }

    #endregion

    #region Do not drawer children

    [Serializable]
    public class DirectWithoutChildren {
        [SerializeField]
        private Parent parent;
        [SerializeField]
        private Child child;

        [Serializable]
        public class Parent {
            [SerializeField]
            private string first = "Default drawer";
        }

        [Serializable]
        public class Child : Parent {
        }
    }

    [Serializable]
    public class AttributeWithoutChildren {
        [SerializeField, Range(0, 100)]
        private float pureRange;
        [SerializeField, Parent]
        private AttributeExample parent;
        [SerializeField, Child]
        private AttributeExample child;

        [Serializable]
        public class AttributeExample {
            [SerializeField]
            private string first = "Default drawer";
        }

        public class Parent : PropertyAttribute {

        }

        public class Child : Parent {

        }
    }

    #endregion

    #region Range

    [Serializable]
    public class BuiltinDrawer {
        [SerializeField, Range(0, 100)]
        private float first;
        [SerializeField, Multiline]
        private string second;
        [SerializeField, Range(0, 100)]
        private float third;
    }

    #endregion

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(DirectDrawer.Parent), true)]
    [CustomPropertyDrawer(typeof(AttributeDrawer.Parent), true)]
    [CustomPropertyDrawer(typeof(DirectWithoutChildren.Parent), false)]
    [CustomPropertyDrawer(typeof(AttributeWithoutChildren.Parent), false)]
    public class CustomFieldDrawer : PropertyDrawer {
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label) {
            rect = EditorGUI.PrefixLabel(rect, label);

            EditorGUI.LabelField(rect, property.displayName + " is drown");
        }
    }
#endif
}
}
