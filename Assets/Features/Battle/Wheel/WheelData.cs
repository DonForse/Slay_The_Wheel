using System;
using UnityEngine;

namespace Features.Battle.Wheel
{
    [Serializable]
    public class WheelData
    {
        [HideInInspector]public float RotationAngle;
        [SerializeField]public float Radius;
        [HideInInspector]public int Size;
    }
}