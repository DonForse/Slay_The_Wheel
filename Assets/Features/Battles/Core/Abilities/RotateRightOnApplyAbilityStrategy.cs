using System.Collections;
using System.Linq;
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

        public IEnumerator Execute(Ability ability,InPlayCard executor, PlayerController defender, PlayerController attacker)
        {
            yield return defender.Rotate(ActDirection.Right, ability.AbilityData.First().Amount);
        }
    }
}