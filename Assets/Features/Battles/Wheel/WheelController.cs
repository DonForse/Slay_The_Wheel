using System;
using System.Collections;
using System.Collections.Generic;
using Features.Battles.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Features.Battles.Wheel
{
    public class WheelController : MonoBehaviour
    {
        [SerializeField] private GameObject itemPrefab;
        [SerializeField] protected float rotationSpeed = 5;
        protected float startAngle;
        [SerializeField] private WheelData wheelData;
        private bool _enabled;
        private bool _isRotating;
        private List<Vector2> _positions = new();
        private List<GameObject> items= new();
        public event EventHandler RotatedRight;
        public event EventHandler RotatedLeft;

        public void Enable() => _enabled = true;
        public void Disable() => _enabled = false;
        public void SetSize(int size)
        {
            wheelData.Size = size;
            CalculatePositions();
            for (int i = 0; i < wheelData.Size; i++)
            {
                var go = Instantiate(itemPrefab, this.transform);
                go.transform.localPosition = _positions[i];
                items.Add(go);
            }

            enabled = true;
        }

        public IEnumerator MoveTowardsDirection(WheelRotation wheelRotation)
        {
            startAngle = wheelData.RotationAngle;
            var rotationInput = wheelRotation == WheelRotation.Right ? 1 : -1;
            var anglePerItem = (1.5f * Mathf.PI) / (wheelData.Size);

            while (Mathf.Abs(wheelData.RotationAngle - startAngle) < anglePerItem)
            {
                wheelData.RotationAngle += rotationInput * rotationSpeed * Time.deltaTime;
                RotateToNewPosition();
                yield return new WaitForEndOfFrame();
            }

            SnapToNearestPosition();

            // if (!executeCallback)
            //     yield break;
            // if ((playerController.WheelData.RotationAngle - startAngle) > 0)
            //     yield return _rightCallback.Invoke();
            // // TurnRight?.Invoke(this, null);
            // else
            //     yield return _leftCallback.Invoke();
            // TurnLeft?.Invoke(this, null);
        }
        
        private void Update()
        {
            if (!_enabled) return;
            if (!_isRotating && Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                    return;
                startAngle = wheelData.RotationAngle;
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
        
            wheelData.RotationAngle += rotationInput * rotationSpeed * Time.deltaTime;

            var anglePerItem = (1.5f * Mathf.PI) / (wheelData.Size);

            if (Mathf.Abs(wheelData.RotationAngle - startAngle) >= anglePerItem)
            {
                _isRotating = false;

                if ((wheelData.RotationAngle - startAngle) > 0)
                    RotatedRight?.Invoke(this,null);
                else
                    RotatedLeft?.Invoke(this,null);
                SnapToNearestPosition();
            }
        
            RotateToNewPosition();
        }
        private void RollbackPosition() => wheelData.RotationAngle = startAngle;

        private void SnapToNearestPosition()
        {
            if (wheelData.Size == 0) return;
            float anglePerItem = 2 * Mathf.PI / wheelData.Size;
            float targetAngle = Mathf.Round(wheelData.RotationAngle / anglePerItem) * anglePerItem;
            wheelData.RotationAngle = targetAngle;
            RotateToNewPosition();
        }

        private void RotateToNewPosition()
        {
            for (var i = 0; i < wheelData.Size; i++)
            {
                var initialTheta = Mathf.Atan2(_positions[i].y, _positions[i].x);
                var newTheta = initialTheta + wheelData.RotationAngle;
                var x = wheelData.Radius * Mathf.Cos(newTheta);
                var y = wheelData.Radius * Mathf.Sin(newTheta);
                var newPosition = new Vector2(x, y);
                items[i].transform.localPosition = newPosition;
            }
        }
        
        private void CalculatePositions()
        {
            var angleOffset = Mathf.PI / 2; // Offset to start at the top of the circle

            for (var i = 0; i < wheelData.Size; i++)
            {
                // Calculate spherical coordinates with angle offset
                var theta = 2 * Mathf.PI * i / wheelData.Size + angleOffset;
                var x = wheelData.Radius * Mathf.Cos(theta);
                var y = wheelData.Radius * Mathf.Sin(theta);

                var position = new Vector2(x, y);
                _positions.Add(position);
            }
        }
    }
}