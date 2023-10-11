using System;
using System.Collections;
using UnityEngine;

public class BotControlWheel : MonoBehaviour, IControlWheel
{
    [SerializeField] private float rotationSpeed = 5;
    [SerializeField] private Wheel wheel;
    private float rotationAngle;
    private float startAngle;
    private float anglePerItem;
    public event EventHandler TurnRight;
    public event EventHandler TurnLeft;

    public void Enable()
    {
        this.enabled = true;
    }

    public void Disable()
    {
        this.enabled = false;
    }

    public IEnumerator TurnTowardsDirection(bool right)
    {
        startAngle = rotationAngle;
        anglePerItem = (1.5f * Mathf.PI) / (wheel.Size);
        return MoveTowardsDirection(right);
    }

    private IEnumerator MoveTowardsDirection(bool right)
    {
        var rotationInput = right ? 1 : -1;
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

    private void SnapToNearestPosition()
    {
        float anglePerItem = 2 * Mathf.PI / wheel.Size;
        float targetAngle = Mathf.Round(rotationAngle / anglePerItem) * anglePerItem;
        rotationAngle = targetAngle;
        RotateToNewPosition();
    }

    private void RotateToNewPosition()
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