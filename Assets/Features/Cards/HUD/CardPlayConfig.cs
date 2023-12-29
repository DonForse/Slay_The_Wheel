using System;
using UnityEngine;

namespace Features.Cards.HUD
{
    [Serializable]
    public class CardPlayConfig {
        [SerializeField]
        public RectTransform playArea;
        
        [SerializeField]
        public bool destroyOnPlay;

    }
}