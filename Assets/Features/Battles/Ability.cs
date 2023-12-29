using System;
using Features.Battles.Core;
using Features.Cards;

namespace Features.Battles
{
    [Serializable]
    public class Ability
    {
        public AbilityData[] AbilityData;
        public AbilityEnum Type;
        public BattleEventEnum battleEventEnum;
    }
}