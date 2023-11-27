using System.Collections;
using Features.Battles.Wheel;
using Features.Cards.InPlay;

namespace Features.Battles.Core.Abilities
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