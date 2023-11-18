using System.Collections;
using Features.Battles.Wheel;
using Features.Cards;

namespace Features.Battles.Core.OnHitEffects
{
    public class BurnOnApplyAbilityStrategy : IOnApplyAbilityStrategy
    {
        public bool IsValid(AbilityEnum abilityEnum) => abilityEnum == AbilityEnum.Burn;

        public IEnumerator Execute(InPlayCard executor, int amount, PlayerController defender,
            PlayerController attacker)
        {
            var affectedCard = defender.GetFrontCard();
            affectedCard.UpdateEffect(AbilityEnum.Burn, amount);
            yield break;
        }
    }
}