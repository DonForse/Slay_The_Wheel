using System;
using System.Collections;

namespace Features.Battles.Wheel
{
    public interface IControlWheel 
    {
        event EventHandler TurnRight;
        event EventHandler TurnLeft;
        void Enable();
        void Disable();
        void SetTurnRightAction(Func<IEnumerator> callback);
        void SetTurnLeftAction(Func<IEnumerator> callback);
        void SetOnBeforeRotation(Action<TurningOrientation> callback);
    }
}