using System.Collections;
using System.Linq;
using Features.Battles.Wheel;
using Features.Cards.InPlay;

namespace Features.Battles.Core.Abilities
{
    public class RotateLeftOnApplyAbilityStrategy : IOnApplyAbilityStrategy
    {
        public bool IsValid(AbilityEnum abilityEnum)
        {
            return abilityEnum == AbilityEnum.RotateLeft;
        }

        public IEnumerator Execute(Ability ability,InPlayCard executor, PlayerController enemyWheel, PlayerController executorWheel)
        {
            yield return enemyWheel.Rotate(ActDirection.Left, ability.AbilityData.First().Amount);
        }
    }
}