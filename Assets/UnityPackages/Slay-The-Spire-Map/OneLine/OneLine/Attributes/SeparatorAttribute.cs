using System;
using UnityEngine;

namespace UnityPackages.Slay_The_Spire_Map.OneLine.OneLine.Attributes {
    ///<summary>
    ///Draws horizontal or vertical separator
    ///</summary>
    [AttributeUsage(validOn: AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
    public class SeparatorAttribute : PropertyAttribute {

        public SeparatorAttribute() {
            Text = "";
            Thickness = 2;
        }

        public SeparatorAttribute(string text) : this() {
            Text = text;
        }

        public SeparatorAttribute(string text, int thickness) : this(text){
            Thickness = thickness;
        }

        public string Text { get; set; }
        public int Thickness { get; set; }

    }
}
