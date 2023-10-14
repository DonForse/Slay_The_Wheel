using System;
using UnityEngine;

public class InputControlWheel : ControlWheel, IControlWheel
{
    private bool _isRotating = false;
    private bool _wasEnabled;
    private bool _enabled;
    public override event EventHandler TurnRight;
    public override event EventHandler TurnLeft;
    public override void Enable()
    {
        base.Enable();
        _enabled = true;
    }

    public override void Disable()
    {
        base.Disable();
        _enabled = false;
    }

    private void Update()
    {
        if (!_enabled) return;
        if (!_isRotating && Input.GetMouseButtonDown(0))
        {
            startAngle = rotationAngle;
            _isRotating = true;
        }

        if (_isRotating && Input.GetMouseButtonUp(0))
        {
            _isRotating = false;
            RollbackPosition();
            SnapToNearestPosition();
        }

        if (!_isRotating) return;
        
        var rotationInput = Mathf.Clamp(Input.GetAxis("Mouse X"), -1f,1f);
        
        rotationAngle += rotationInput * rotationSpeed * Time.deltaTime;

        var anglePerItem = (1.5f * Mathf.PI) / (wheel.Size);

        if (Mathf.Abs(rotationAngle - startAngle) >= anglePerItem)
        {
            _isRotating = false;
            SnapToNearestPosition();
            
            if ((rotationAngle - startAngle) > 0)
                TurnRight?.Invoke(this, null);
            else
                TurnLeft?.Invoke(this, null);
        }
        
        RotateToNewPosition();
    }
    
    private void RollbackPosition() => rotationAngle = startAngle;

    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            _isRotating = false;
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