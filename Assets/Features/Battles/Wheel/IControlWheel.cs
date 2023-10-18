using System;

namespace Features.Battles.Wheel
{
    public interface IControlWheel 
    {
        event EventHandler TurnRight;
        event EventHandler TurnLeft;
        void Enable();
        void Disable();
    }
}