using System;
using Features.Cards;
using UnityEngine;

namespace Features.Maps.Shop.Packs
{
    [Serializable]
    public class CardPackItem
    {
        public BaseCardScriptableObject card;
        [Range(0f, 1f)] public float dropRatePercentage;
    }
}