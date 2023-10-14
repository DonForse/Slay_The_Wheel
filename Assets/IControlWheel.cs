using System;
using System.Collections;

public interface IControlWheel 
{
    event EventHandler TurnRight;
    event EventHandler TurnLeft;
    void Enable();
    void Disable();
    IEnumerator TurnRightWithoutNotifying();
    IEnumerator TurnLeftWithoutNotifying();
}