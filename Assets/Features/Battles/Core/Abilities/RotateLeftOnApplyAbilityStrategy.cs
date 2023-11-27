using System.Collections;
using Features.Battles.Wheel;
using Features.Cards;
using Features.Cards.InPlay;

namespace Features.Battles.Core.OnHitEffects
{
    public class RotateLeftOnApplyAbilityStrategy : IOnApplyAbilityStrategy
    {
        public bool IsValid(AbilityEnum abilityEnum)
        {
            return abilityEnum == AbilityEnum.RotateLeft;
        }

        public IEnumerator Execute(InPlayCard executor, int amount, PlayerController defender, PlayerController attacker)
        {
            yield return defender.Rotate(ActDirection.Left, amount);
        }
    }
}