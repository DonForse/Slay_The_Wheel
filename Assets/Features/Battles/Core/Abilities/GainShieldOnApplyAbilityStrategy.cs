using System;
using System.Collections;
using Features.Battles.Wheel;
using Features.Cards;
using Features.Cards.InPlay;

namespace Features.Battles.Core.Abilities
{
    public class GainShieldOnApplyAbilityStrategy : IOnApplyAbilityStrategy
    {
        public bool IsValid(AbilityEnum abilityEnum) => abilityEnum == AbilityEnum.GainShield;

        public IEnumerator Execute(Ability ability, InPlayCard executor, PlayerController enemyWheel,
            PlayerController executorWheel)
        {
            foreach (var data in ability.AbilityData)
            {
                var targets = TargetSystem.GetTargets(data.Target, executor, executorWheel, enemyWheel);
                foreach (var target in targets)
                {
                    target.GetCard().Armor += data.Amount;
                    yield return target.PlayGainShield();
                }
            }
            yield break;
        }
    }
}