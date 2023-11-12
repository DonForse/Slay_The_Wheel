using System.Collections.Generic;
using UnityEngine;

namespace Features.Cards.Heroes
{
    [CreateAssetMenu(fileName = "LevelUps", menuName = "Cards/Heroes/LevelUps")]
    public class LevelUpsScriptableObject : ScriptableObject
    {
        public List<LevelUpInformation> LevelUpInformations;
    }
}