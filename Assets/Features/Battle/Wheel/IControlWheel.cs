using System;

namespace Features.Battle.Wheel
{
    public interface IControlWheel 
    {
        event EventHandler TurnRight;
        event EventHandler TurnLeft;
        void Enable();
        void Disable();
    }
}