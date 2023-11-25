using System;
using Features.Cards;
using UnityEngine;

namespace Features.Battles.Wheel
{
    public class ZoomWheel :MonoBehaviour
    {
        [SerializeField] protected PlayerController playerController;

        private void OnEnable()
        {
            playerController.ActiveCardChanged += OnActiveCardChanged;
        }

        private void OnActiveCardChanged(object sender, InPlayCard e)
        {
            foreach (var card in playerController.Cards)
            {
                card.SetAsActive(card == e);
            }
        }

        private void Update()
        {
            Debug.Log(playerController.WheelData.RotationAngle);
            
        }
    }
}