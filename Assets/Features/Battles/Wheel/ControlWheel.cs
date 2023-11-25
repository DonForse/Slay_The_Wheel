using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Features.Battles.Wheel
{
    public abstract class ControlWheel :MonoBehaviour, IControlWheel
    {
        [SerializeField] protected float rotationSpeed = 5;
        [SerializeField] protected PlayerController playerController;
        [SerializeField] protected ZoomWheel zoomWheel;
        
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
            if (playerController.WheelData.Size == 0) return;
            float anglePerItem = 2 * Mathf.PI / playerController.WheelData.Size;
            float targetAngle = Mathf.Round(playerController.WheelData.RotationAngle / anglePerItem) * anglePerItem;
            playerController.WheelData.RotationAngle = targetAngle;
            RotateToNewPosition();
        }

        internal void RotateToNewPosition()
        {
            for (var i = 0; i < playerController.Cards.Count; i++)
            {
                var initialTheta = Mathf.Atan2(playerController.Positions[i].y, playerController.Positions[i].x);
                var newTheta = initialTheta + playerController.WheelData.RotationAngle;
                var x = playerController.WheelData.Radius * Mathf.Cos(newTheta);
                var y = playerController.WheelData.Radius * Mathf.Sin(newTheta);
                var newPosition = new Vector2(x, y);
                playerController.Cards[i].transform.localPosition = newPosition;
                
                zoomWheel.SetValue(playerController.Cards[i], 1f-Mathf.Clamp01((newPosition - playerController.Positions[0]).magnitude/3f));
            }
        }
    }
}