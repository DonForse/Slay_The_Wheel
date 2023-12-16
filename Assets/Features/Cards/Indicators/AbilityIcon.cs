using System;
using Features.Battles;
using UnityEngine;

namespace Features.Cards.Indicators
{
    [Serializable]
    public class AbilityIcon
    {
        public AbilityEnum ability;
        public Sprite image;
        public string description;
    }
}