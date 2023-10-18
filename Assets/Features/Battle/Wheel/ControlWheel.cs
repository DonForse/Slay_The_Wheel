using System;
using UnityEngine;

namespace Features.Battle.Wheel
{
    public abstract class ControlWheel :MonoBehaviour, IControlWheel
    {
        [SerializeField] protected float rotationSpeed = 5;
        [SerializeField] protected WheelController wheelController;
        protected float startAngle;
        public abstract event EventHandler TurnRight;
        public abstract event EventHandler TurnLeft;

        public virtual void Enable() => this.enabled = true;

        public virtual void Disable() => this.enabled = false;

        internal void SnapToNearestPosition()
        {
            float anglePerItem = 2 * Mathf.PI / wheelController.WheelData.Size;
            float targetAngle = Mathf.Round(wheelController.WheelData.RotationAngle / anglePerItem) * anglePerItem;
            wheelController.WheelData.RotationAngle = targetAngle;
            RotateToNewPosition();
        }

        internal void RotateToNewPosition()
        {
            for (var i = 0; i < wheelController.WheelData.Size; i++)
            {
                var initialTheta = Mathf.Atan2(wheelController.Positions[i].y, wheelController.Positions[i].x);
                var newTheta = initialTheta + wheelController.WheelData.RotationAngle;

                var x = wheelController.WheelData.Radius * Mathf.Cos(newTheta);
                var y = wheelController.WheelData.Radius * Mathf.Sin(newTheta);

                var newPosition = new Vector2(x, y);
                wheelController.Cards[i].transform.localPosition = newPosition;
            }
        }
    }
}