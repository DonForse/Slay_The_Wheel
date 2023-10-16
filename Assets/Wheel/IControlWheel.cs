using System;
using System.Collections;

public interface IControlWheel 
{
    event EventHandler TurnRight;
    event EventHandler TurnLeft;
    void Enable();
    void Disable();
}