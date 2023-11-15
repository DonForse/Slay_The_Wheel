using System;
using System.Collections;
using UnityEngine;

namespace Features.Battles.Wheel
{
    public class AutomaticControlWheel : ControlWheel
    {
        public override event EventHandler TurnRight;
        public override event EventHandler TurnLeft;

        public IEnumerator TurnTowardsDirection(ActDirection actDirection, bool executeCallback)
        {
            startAngle = wheelController.WheelData.RotationAngle;
            yield return MoveTowardsDirection(actDirection, executeCallback);
        }

        private IEnumerator MoveTowardsDirection(ActDirection actDirection, bool executeCallback)
        {
            var rotationInput = actDirection == ActDirection.Right ? 1 : -1;
            var anglePerItem = (1.5f * Mathf.PI) / (wheelController.WheelData.Size);

            while (Mathf.Abs(wheelController.WheelData.RotationAngle - startAngle) < anglePerItem)
            {
                wheelController.WheelData.RotationAngle += rotationInput * rotationSpeed * Time.deltaTime;
                RotateToNewPosition();
                yield return new WaitForEndOfFrame();
            }

            SnapToNearestPosition();

            if (!executeCallback)
                yield break;
            if ((wheelController.WheelData.RotationAngle - startAngle) > 0)
                yield return _rightCallback.Invoke();
                // TurnRight?.Invoke(this, null);
            else
                yield return _leftCallback.Invoke();
                // TurnLeft?.Invoke(this, null);
        }

        public void Test(ActDirection right)
        {
            StartCoroutine(TurnTowardsDirection(right, true));
        }
    }
}