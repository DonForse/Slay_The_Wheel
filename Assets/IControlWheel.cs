using System;

public interface IControlWheel 
{
    event EventHandler TurnRight;
    event EventHandler TurnLeft;
    void Enable();
    void Disable();
}