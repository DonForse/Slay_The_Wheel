using System.Collections.Generic;
using UnityEngine;

namespace Features.Cards.Indicators
{
    [CreateAssetMenu(fileName = "EffectsIcons", menuName = "Icons/Effects")]
    public class EffectsIconsScriptableObject : ScriptableObject
    {
        public List<EffectIcon> effectIcons;
    }
}