using System;
using System.Collections;
using System.Collections.Generic;
using Features.Battles.Core;
using Features.Cards.InPlay;
using UnityEngine;
using UnityEngine.Serialization;

namespace Features.Battles.Wheel
{
    public class WheelControllerDebug : MonoBehaviour
    {
        [FormerlySerializedAs("enableInput")] public bool toggleInput;
        [FormerlySerializedAs("setSize")] public bool init;
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
            wheelController.Initialize(size, new List<InPlayCardScriptableObject>());
        }

        private void Update()
        {
            if (init)
            {
                init = false;
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
                wheelController.UnlockPlayerInput();

            if (_enabled)
                wheelController.LockPlayerInput();
            _enabled = !_enabled;
        }

        private IEnumerator RotateDebug()
        {
            int amount = 0;
            while (amount < rotateAmount)
            {
                yield return wheelController.TurnTowardsDirection(WheelRotation.Right);
                amount++;
            }
        }
    }
}