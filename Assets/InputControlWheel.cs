using System;
using System.Collections;
using UnityEngine;

public class InputControlWheel : MonoBehaviour, IControlWheel
{
    [SerializeField] private Wheel wheel;
    public float rotationSpeed;
    private bool isRotating = false;
    private float startAngle;
    private float rotationAngle;
    private bool _wasEnabled;
    private bool _enabled;
    public event EventHandler TurnRight;
    public event EventHandler TurnLeft;
    public void Enable() => _enabled = true;

    public void Disable() => _enabled = false;

    private void Update()
    {
        if (!_enabled) return;
        if (!isRotating && Input.GetMouseButtonDown(0))
        {
            startAngle = rotationAngle;
            isRotating = true;
        }

        if (isRotating && Input.GetMouseButtonUp(0))
        {
            isRotating = false;
            RollbackPosition();
            SnapToNearestPosition();
        }

        if (!isRotating) return;
        
        var rotationInput = Mathf.Clamp(Input.GetAxis("Mouse X"), -1f,1f);
        
        rotationAngle += rotationInput * rotationSpeed * Time.deltaTime;

        var anglePerItem = (1.5f * Mathf.PI) / (wheel.Size);

        if (Mathf.Abs(rotationAngle - startAngle) >= anglePerItem)
        {
            isRotating = false;
            SnapToNearestPosition();
            
            if ((rotationAngle - startAngle) > 0)
                TurnRight?.Invoke(this, null);
            else
                TurnLeft?.Invoke(this, null);
        }
        
        RotateToNewPosition();
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

    private void RollbackPosition() => rotationAngle = startAngle;

    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            isRotating = false;
            if (_wasEnabled)
                _enabled = true;
            _wasEnabled = false;
        }
        else
        {
            if (_enabled)
                _wasEnabled = true;
            _enabled = false;
            SnapToNearestPosition();
        }
    }
}