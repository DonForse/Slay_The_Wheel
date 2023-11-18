using System;
using System.Collections.Generic;
using Features.Battles;
using UnityEngine;

namespace Features.Cards
{
    public class HeroRunCard : RunCard
    {
        public int Exp;
        public int Level;
        public HeroRunCard(BaseCardScriptableObject cardScriptableObject) : base(cardScriptableObject)
        {
            
        }
    }
}