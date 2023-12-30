using System;
using Features.Cards.InPlay;
using UnityEngine;

namespace Features.Battles.Wheel
{
    public class WheelSlot : MonoBehaviour
    {
        public InPlayCard Card;
        public int Index;

        public void SetCard(InPlayCard card)
        {
            Card = card;
        }

        public InPlayCard GetCard() 
        {
            return Card;
        }
    }
}