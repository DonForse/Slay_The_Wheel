using System;

namespace UnityPackages.Slay_The_Spire_Map.OneLine.OneLine.Attributes {
    ///<summary>
    ///Hides prefix label of ROOT FIELD. Has no effect on NESTED FIELDS.
    ///Useful for expanding available space in line.
    ///Applied to root array hides label of elements ("Element 1", "Element 2" etc)
    ///</summary>
    [AttributeUsage(validOn: AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class HideLabelAttribute : Attribute {

    }
}
