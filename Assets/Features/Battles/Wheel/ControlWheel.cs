using System;
using System.Collections;
using UnityEngine;

namespace Features.Battles.Wheel
{
    public abstract class ControlWheel :MonoBehaviour, IControlWheel
    {
        [SerializeField] protected float rotationSpeed = 5;
        [SerializeField] protected WheelController wheelController;
        
        protected float startAngle;
        protected Func<IEnumerator> _rightCallback;
        protected Func<IEnumerator> _leftCallback;
        protected Action<TurningOrientation> _onBeforeRotation;
        
        public abstract event EventHandler TurnRight;
        public abstract event EventHandler TurnLeft;
        public virtual void Enable() => this.enabled = true;
        public virtual void Disable() => this.enabled = false;
        
        public void SetTurnRightAction(Func<IEnumerator> callback)
        {
            _rightCallback = callback;
        }

        public void SetTurnLeftAction(Func<IEnumerator> callback)
        {
            _leftCallback = callback;
        }

        public void SetOnBeforeRotation(Action<TurningOrientation> callback)
        {
            _onBeforeRotation = callback;
        }

        internal void SnapToNearestPosition()
        {
            if (wheelController.WheelData.Size == 0) return;
            float anglePerItem = 2 * Mathf.PI / wheelController.WheelData.Size;
            float targetAngle = Mathf.Round(wheelController.WheelData.RotationAngle / anglePerItem) * anglePerItem;
            wheelController.WheelData.RotationAngle = targetAngle;
            RotateToNewPosition();
        }

        internal void RotateToNewPosition()
        {
            for (var i = 0; i < wheelController.Cards.Count; i++)
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