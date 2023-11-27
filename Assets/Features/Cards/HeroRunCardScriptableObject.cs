using System;
using System.Collections.Generic;
using Features.Battles;
using UnityEngine;

namespace Features.Cards
{
    public class HeroRunCardScriptableObject : RunCardScriptableObject
    {
        public int Exp;
        public int Level;
        public HeroRunCardScriptableObject(BaseCardScriptableObject cardScriptableObject) : base(cardScriptableObject)
        {
            
        }
    }
}