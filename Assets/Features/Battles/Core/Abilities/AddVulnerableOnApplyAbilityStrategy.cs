using System.Collections;
using Features.Battles.Wheel;
using Features.Cards.InPlay;

namespace Features.Battles.Core.Abilities
{
    public class AddVulnerableOnApplyAbilityStrategy : IOnApplyAbilityStrategy
    {
        public bool IsValid(AbilityEnum abilityEnum) => abilityEnum == AbilityEnum.AddVulnerable;

        public IEnumerator Execute(Ability ability,InPlayCard executor, PlayerController enemyWheel,
            PlayerController executorWheel)
        {
            foreach (var data in ability.AbilityData)
            {
                var targets = TargetSystem.GetTargets(data.Target, executor, executorWheel, enemyWheel);
                foreach (var target in targets)
                {
                    target.UpdateEffect(EffectEnum.Vulnerable, data.Amount);
                }
            }
            yield break;
        }
    }
}