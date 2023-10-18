using System;
using UnityEngine;

[Serializable]
public class WheelData
{
    [HideInInspector]public float RotationAngle;
    [SerializeField]public float Radius;
    [HideInInspector]public int Size;
}