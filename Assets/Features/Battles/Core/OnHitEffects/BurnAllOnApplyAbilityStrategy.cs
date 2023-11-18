using System.Collections;
using Features.Battles.Wheel;
using Features.Cards;

namespace Features.Battles.Core.OnHitEffects
{
    public class BurnAllOnApplyAbilityStrategy : IOnApplyAbilityStrategy
    {
        public bool IsValid(AbilityEnum abilityEnum) => abilityEnum == AbilityEnum.BurnAll;

        public IEnumerator Execute(InPlayCard executor, int amount, PlayerController defender,
            PlayerController attacker)
        {
            foreach (var card in defender.Cards)
            {
                for (int i = 0; i < amount; i++)
                    card.UpdateEffect(AbilityEnum.Burn, 1);
            }

            yield break;
        }
    }
}