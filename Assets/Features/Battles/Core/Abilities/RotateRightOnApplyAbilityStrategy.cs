using System.Collections;
using Features.Battles.Wheel;
using Features.Cards;

namespace Features.Battles.Core.OnHitEffects
{
    public class RotateRightOnApplyAbilityStrategy : IOnApplyAbilityStrategy
    {
        public bool IsValid(AbilityEnum abilityEnum)
        {
            return abilityEnum == AbilityEnum.RotateRight;
        }

        public IEnumerator Execute(InPlayCard executor, int amount, PlayerController defender, PlayerController attacker)
        {
            yield return defender.Rotate(ActDirection.Right, amount);
        }
    }
}