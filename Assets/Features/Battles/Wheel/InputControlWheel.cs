using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Features.Battles.Wheel
{
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
                startAngle = playerController.WheelData.RotationAngle;
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
        
            playerController.WheelData.RotationAngle += rotationInput * rotationSpeed * Time.deltaTime;

            var anglePerItem = (1.5f * Mathf.PI) / (playerController.WheelData.Size);

            if (Mathf.Abs(playerController.WheelData.RotationAngle - startAngle) >= anglePerItem)
            {
                _isRotating = false;

                if ((playerController.WheelData.RotationAngle - startAngle) > 0)
                    StartCoroutine(_rightCallback.Invoke());
                else
                    StartCoroutine(_leftCallback.Invoke());
                SnapToNearestPosition();
            }
        
            RotateToNewPosition();
        }
    
        private void RollbackPosition() => playerController.WheelData.RotationAngle = startAngle;

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
}