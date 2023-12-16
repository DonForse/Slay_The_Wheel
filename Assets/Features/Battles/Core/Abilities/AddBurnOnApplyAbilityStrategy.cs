using System;
using System.Collections;
using Features.Battles.Wheel;
using Features.Cards;
using Features.Cards.InPlay;

namespace Features.Battles.Core.Abilities
{
    public class AddBurnOnApplyAbilityStrategy : IOnApplyAbilityStrategy
    {
        public bool IsValid(AbilityEnum abilityEnum) => abilityEnum == AbilityEnum.Burn;

        public IEnumerator Execute(Ability ability,InPlayCard executor, PlayerController enemyWheel,
            PlayerController executorWheel)
        {
            foreach (var data in ability.AbilityData)
            {
                var targets = TargetSystem.GetTargets(data.Target, executor, executorWheel, enemyWheel);
                foreach (var target in targets)
                {
                    target.UpdateEffect(EffectEnum.Fire, data.Amount);
                }
            }
            yield break;
        }
    }
}