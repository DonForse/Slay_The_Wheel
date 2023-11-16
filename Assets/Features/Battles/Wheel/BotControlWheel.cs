using System;
using System.Collections;
using UnityEngine;

namespace Features.Battles.Wheel
{
    public class BotControlWheel : ControlWheel
    {
        private WaitForEndOfFrame _waitForEndOfFrame;
        public override event EventHandler TurnRight;
        public override event EventHandler TurnLeft;

        private void Start()
        {
            _waitForEndOfFrame = new WaitForEndOfFrame();
        }

        public IEnumerator TurnTowardsDirection(bool right)
        {
            startAngle = playerController.WheelData.RotationAngle;
            _onBeforeRotation?.Invoke(right? TurningOrientation.TurnRight: TurningOrientation.TurnLeft);
            yield return MoveTowardsDirection(right);
        }

        private IEnumerator MoveTowardsDirection(bool right)
        {
            var rotationInput = right ? 1 : -1;
            var anglePerItem = (1.5f * Mathf.PI) / (playerController.WheelData.Size);

            while (Mathf.Abs(playerController.WheelData.RotationAngle - startAngle) < anglePerItem)
            {
                playerController.WheelData.RotationAngle += rotationInput * rotationSpeed * Time.deltaTime;
                RotateToNewPosition();
                yield return _waitForEndOfFrame;
            }

            SnapToNearestPosition();

            if ((playerController.WheelData.RotationAngle - startAngle) > 0)
                yield return _rightCallback.Invoke();
            else
                yield return _leftCallback.Invoke();
        }
    }
}