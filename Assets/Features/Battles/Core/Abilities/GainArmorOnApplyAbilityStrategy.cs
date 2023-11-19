using System.Collections;
using Features.Battles.Wheel;
using Features.Cards;

namespace Features.Battles.Core.Abilities
{
    public class GainArmorOnApplyAbilityStrategy : IOnApplyAbilityStrategy
    {
        public bool IsValid(AbilityEnum abilityEnum) => abilityEnum == AbilityEnum.GainArmor;

        public IEnumerator Execute(InPlayCard executor, int amount, PlayerController defender,
            PlayerController attacker)
        {
            executor.Armor += amount;
            yield break;
        }
    }
}