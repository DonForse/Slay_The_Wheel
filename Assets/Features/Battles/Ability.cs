using System;
using Features.Battles.Core;

namespace Features.Battles
{
    [Serializable]
    public class Ability
    {
        public AbilityData[] AbilityData;
        public AbilityEnum Type;
    }
}