using System.Collections;
using Features.Battles.Wheel;
using Features.Cards;

namespace Features.Battles.Core.Abilities
{
    public class GainAttackOnApplyAbilityStrategy : IOnApplyAbilityStrategy
    {
        public bool IsValid(AbilityEnum abilityEnum) => abilityEnum == AbilityEnum.GainAtk;

        public IEnumerator Execute(InPlayCard executor, int amount, PlayerController defender,
            PlayerController attacker)
        {
            executor.Attack += amount;
            yield break;
        }
    }
}