using System;
using System.Collections;
using Features.Battles.Wheel;
using Features.Cards.InPlay;

namespace Features.Battles.Core.Abilities
{
    public class AddOilOnApplyAbilityStrategy : IOnApplyAbilityStrategy
    {
        public bool IsValid(AbilityEnum abilityEnum) => abilityEnum == AbilityEnum.AddOil;

        public IEnumerator Execute(Ability ability, InPlayCard executor, PlayerController enemyWheel, PlayerController executorWheel)
        {
            foreach (var data in ability.AbilityData)
            {
                var targets = TargetSystem.GetTargets(data.Target, executor, executorWheel, enemyWheel);
                foreach (var target in targets)
                {
                    target.UpdateEffect(EffectEnum.Oil, data.Amount);
                }
            }
            yield break;
        }
    }
}