using Features.Battles.Wheel;

namespace Features.Battles.Core
{
    public enum WheelRotation
    {
        Left = 1,
        Right = 2
    }

    public static class WheelRotationExtension
    {
        public static WheelRotation Inverse(this WheelRotation wr)
        {
            return wr == WheelRotation.Left ? WheelRotation.Right : WheelRotation.Left;
        }
    }
}