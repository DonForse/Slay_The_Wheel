using System;

namespace UnityPackages.Slay_The_Spire_Map.OneLine.OneLine.Attributes {
    ///<summary>
    ///Defines weight of marked field in the line.
    ///Available only on SIMPLE NESTED FIELDS. Has no effect on ROOT FIELD.
    ///Fields without [WeightAttribute] has default weight = 1.
    ///Applied to nested arrays defines weight of each element.
    ///</summary>
    [AttributeUsage(validOn: AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
    public class WeightAttribute : Attribute {
        private float weight;

        public WeightAttribute(float weight) {
            this.weight = weight;
        }

        public float Weight { get { return weight; } }
    }
}
