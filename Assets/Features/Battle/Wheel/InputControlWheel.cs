using System;
using UnityEngine;
using UnityEngine.EventSystems;

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
            if (EventSystem.current.IsPointerOverGameObject())
                return;
            startAngle = wheelController.WheelData.RotationAngle;
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
        
        wheelController.WheelData.RotationAngle += rotationInput * rotationSpeed * Time.deltaTime;

        var anglePerItem = (1.5f * Mathf.PI) / (wheelController.WheelData.Size);

        if (Mathf.Abs(wheelController.WheelData.RotationAngle - startAngle) >= anglePerItem)
        {
            _isRotating = false;
            SnapToNearestPosition();
            
            if ((wheelController.WheelData.RotationAngle - startAngle) > 0)
                TurnRight?.Invoke(this, null);
            else
                TurnLeft?.Invoke(this, null);
        }
        
        RotateToNewPosition();
    }
    
    private void RollbackPosition() => wheelController.WheelData.RotationAngle = startAngle;

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