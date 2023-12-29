using System.Collections;
using Features.Battles.Wheel;
using Features.Cards;
using Features.Cards.InPlay;
using UnityEngine;

namespace Features.Battles.Core.Effects
{
    public class VulnerableOnApplyEffectStrategy : IOnApplyEffectStrategy
    {
        public bool IsValid(Effect effect, BattleEventEnum battleEventEnum)
        {
            return battleEventEnum == BattleEventEnum.EndTurn && effect.Type == EffectEnum.Vulnerable && effect.Amount > 0;
        }

        public IEnumerator Execute(Effect effect, InPlayCard cardWithTheEffect)
        {
            cardWithTheEffect.UpdateEffect(EffectEnum.Vulnerable, -1);
            yield break;
        }
    }
}