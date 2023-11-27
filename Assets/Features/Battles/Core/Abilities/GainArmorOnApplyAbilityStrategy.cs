using System.Collections;
using Features.Battles.Wheel;
using Features.Cards;
using Features.Cards.InPlay;

namespace Features.Battles.Core.Abilities
{
    public class GainArmorOnApplyAbilityStrategy : IOnApplyAbilityStrategy
    {
        public bool IsValid(AbilityEnum abilityEnum) => abilityEnum == AbilityEnum.GainArmor;

        public IEnumerator Execute(InPlayCard executor, int amount, PlayerController defender,
            PlayerController attacker)
        {
            executor.GetCard().Armor += amount;
            yield return executor.PlayGainArmor();
        }
    }
}