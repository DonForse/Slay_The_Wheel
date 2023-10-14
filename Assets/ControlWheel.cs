using System;
using System.Collections;
using UnityEngine;

public abstract class ControlWheel :MonoBehaviour, IControlWheel
{
    [SerializeField] protected float rotationSpeed = 5;
    [SerializeField] protected Wheel wheel;
    protected float rotationAngle;
    protected float startAngle;
    public abstract event EventHandler TurnRight;
    public abstract event EventHandler TurnLeft;

    public virtual void Enable() => this.enabled = true;

    public virtual void Disable() => this.enabled = false;

    public virtual IEnumerator TurnRightWithoutNotifying()
    {
        var anglePerItem = (1.5f * Mathf.PI) / (wheel.Size);
        startAngle = rotationAngle;

        var rotationInput = 1;
        while (Mathf.Abs(rotationAngle - startAngle) < anglePerItem)
        {
            rotationAngle += rotationInput * rotationSpeed * Time.deltaTime;
            RotateToNewPosition();
            yield return new WaitForEndOfFrame();
        }

        SnapToNearestPosition();
    }

    public virtual IEnumerator TurnLeftWithoutNotifying()
    {
        var anglePerItem = (1.5f * Mathf.PI) / (wheel.Size);
        startAngle = rotationAngle;

        var rotationInput = -1;
        while (Mathf.Abs(rotationAngle - startAngle) < anglePerItem)
        {
            rotationAngle += rotationInput * rotationSpeed * Time.deltaTime;
            RotateToNewPosition();
            yield return new WaitForEndOfFrame();
        }

        SnapToNearestPosition();
    }

    internal void SnapToNearestPosition()
    {
        float anglePerItem = 2 * Mathf.PI / wheel.Size;
        float targetAngle = Mathf.Round(rotationAngle / anglePerItem) * anglePerItem;
        rotationAngle = targetAngle;
        RotateToNewPosition();
    }

    internal void RotateToNewPosition()
    {
        for (var i = 0; i < wheel.Size; i++)
        {
            var initialTheta = Mathf.Atan2(wheel.Positions[i].y, wheel.Positions[i].x);
            var newTheta = initialTheta + rotationAngle;

            var x = wheel.Radius * Mathf.Cos(newTheta);
            var y = wheel.Radius * Mathf.Sin(newTheta);

            var newPosition = new Vector2(x, y);
            wheel.Cards[i].transform.localPosition = newPosition;
        }
    }
}