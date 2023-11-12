using System;
using System.Collections.Generic;

namespace Features.Cards.Heroes
{
    [Serializable]
    public class LevelUpInformation
    {
        public int ExpToLevel;
        public List<LevelUpUpgrade> LevelUpUpgrades;
    }
}