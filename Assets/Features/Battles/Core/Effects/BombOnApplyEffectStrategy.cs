using System.Collections;
using Features.Battles.Wheel;
using Features.Cards;
using Features.Cards.InPlay;
using UnityEngine;

namespace Features.Battles.Core.Effects
{
    public class BombOnApplyEffectStrategy : IOnApplyEffectStrategy
    {
        private readonly Battle _battle;

        public BombOnApplyEffectStrategy(Battle battle)
        {
            _battle = battle;
        }

        public bool IsValid(Effect effect, BattleEventEnum battleEventEnum)
        {
            return battleEventEnum == BattleEventEnum.TurnStart && effect.Type == EffectEnum.Bomb && effect.Amount > 0;
        }

        public IEnumerator Execute(Effect effect, InPlayCard cardWithTheEffect)
        {
    
                if (effect.Amount == 1)
                {
                    foreach (var enemyCard in _battle.GetEnemyWheel(cardWithTheEffect).Cards)
                    {
                        yield return _battle.ApplyDamage(40, enemyCard, cardWithTheEffect, null);
                    }

                    yield return _battle.ApplyDamage(9999, cardWithTheEffect, null, null);
                }

                cardWithTheEffect.UpdateEffect(EffectEnum.Bomb, -1);
            }
    }
}