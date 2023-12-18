using System.Collections;
using Features.Battles.Wheel;
using Features.Cards.InPlay;

namespace Features.Battles.Core.Abilities
{
    public class AddShieldOnApplyAbilityStrategy : IOnApplyAbilityStrategy
    {
        public bool IsValid(AbilityEnum abilityEnum) => abilityEnum == AbilityEnum.AddShield;

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