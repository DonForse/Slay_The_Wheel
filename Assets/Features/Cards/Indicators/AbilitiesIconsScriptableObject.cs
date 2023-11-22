using System.Collections.Generic;
using UnityEngine;

namespace Features.Cards.Indicators
{
    [CreateAssetMenu(fileName = "AbilitiesIcons", menuName = "Icons/Ability")]
    public class AbilitiesIconsScriptableObject : ScriptableObject
    {
        public List<AbilityIcon> abilitiesIcons;
    }
}