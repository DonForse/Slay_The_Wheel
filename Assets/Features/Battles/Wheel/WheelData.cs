using System;
using UnityEngine;

namespace Features.Battles.Wheel
{
    [Serializable]
    public class WheelData
    {
        [HideInInspector]public float RotationAngle;
        [SerializeField] public float Radius;
        [HideInInspector]public int Size;
    }
}