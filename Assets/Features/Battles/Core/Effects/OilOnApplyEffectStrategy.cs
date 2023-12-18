using System.Collections;
using Features.Battles.Wheel;
using Features.Cards;
using Features.Cards.InPlay;
using UnityEngine;

namespace Features.Battles.Core.Effects
{
    public class OilOnApplyEffectStrategy : IOnApplyEffectStrategy
    {
        public bool IsValid(Effect effect, BattleEventEnum battleEventEnum)
        {
            return battleEventEnum == BattleEventEnum.PutAtFront && effect.Type == EffectEnum.Oil && effect.Amount > 0;
        }

        public IEnumerator Execute(Effect effect, InPlayCard cardWithTheEffect)
        {
            cardWithTheEffect.UpdateEffect(EffectEnum.Oil, -1);
            yield return cardWithTheEffect.OwnerPlayer.RepeatRotate(1);
        }
    }
}