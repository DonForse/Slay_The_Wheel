using System;
using System.Collections;
using Features.Battles.Core;
using UnityEngine;
using UnityEngine.Serialization;

namespace Features.Battles.Wheel
{
    public class WheelControllerDebug : MonoBehaviour
    {
        [FormerlySerializedAs("enableInput")] public bool toggleInput;
        public bool setSize;
        public bool rotateRight;
        public int rotateAmount;
        public int size;
        private WheelController wheelController;
        private bool _enabled;

        private void Awake()
        {
            wheelController = GetComponent<WheelController>();
        }

        public void SetSize()
        {
            wheelController.SetSize(size);
        }

        private void Update()
        {
            if (setSize)
            {
                setSize = false;
                SetSize();
            }

            if (rotateRight)
            {
                rotateRight = false;

                StartCoroutine(RotateDebug());
            }

            if (toggleInput)
            {
                toggleInput = false;
                ToggleInput();
            }
        }

        private void ToggleInput()
        {
            if (!_enabled)
                wheelController.Enable();

            if (_enabled)
                wheelController.Disable();
            _enabled = !_enabled;
        }

        private IEnumerator RotateDebug()
        {
            int amount = 0;
            while (amount < rotateAmount)
            {
                yield return wheelController.MoveTowardsDirection(WheelRotation.Right);
                amount++;
            }
        }
    }
}