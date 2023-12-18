using System.Collections;
using Features.Battles.Wheel;
using Features.Cards;
using Features.Cards.InPlay;
using UnityEngine;

namespace Features.Battles.Core.Effects
{
    public class BurnOnApplyEffectStrategy : IOnApplyEffectStrategy
    {
        private readonly Battle _battle;

        public BurnOnApplyEffectStrategy(Battle battle)
        {
            _battle = battle;
        }

        public bool IsValid(Effect effect, BattleEventEnum battleEventEnum)
        {
            return battleEventEnum == BattleEventEnum.Spin && effect.Type == EffectEnum.Fire && effect.Amount > 0;
        }

        public IEnumerator Execute(Effect effect, InPlayCard cardWithTheEffect)
        {
            cardWithTheEffect.UpdateEffect(EffectEnum.Fire, -Mathf.CeilToInt(effect.Amount/2f));
            yield return _battle.ApplyDamage(effect.Amount, cardWithTheEffect, null, AbilityEnum.AddBurn);
        }
    }
}