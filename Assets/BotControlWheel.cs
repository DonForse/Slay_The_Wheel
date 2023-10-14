using System;
using System.Collections;
using UnityEngine;

public class BotControlWheel : ControlWheel
{
    public override event EventHandler TurnRight;
    public override event EventHandler TurnLeft;
    
    public IEnumerator TurnTowardsDirection(bool right)
    {
        startAngle = rotationAngle;
        return MoveTowardsDirection(right);
    }

    private IEnumerator MoveTowardsDirection(bool right)
    {
        var rotationInput = right ? 1 : -1;
        var anglePerItem = (1.5f * Mathf.PI) / (wheel.Size);

        while (Mathf.Abs(rotationAngle - startAngle) < anglePerItem)
        {
            rotationAngle += rotationInput * rotationSpeed * Time.deltaTime;
            RotateToNewPosition();
            yield return new WaitForEndOfFrame();
        }

        SnapToNearestPosition();

        if ((rotationAngle - startAngle) > 0)
            TurnRight?.Invoke(this, null);
        else
            TurnLeft?.Invoke(this, null);
    }
}