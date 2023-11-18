using System.Collections;
using Features.Battles.Wheel;
using Features.Cards;

namespace Features.Battles.Core.OnActEffects
{
    public class AddShieldLeftOnApplyAbilityStrategy : IOnApplyAbilityStrategy
    {
        public bool IsValid(AbilityEnum abilityEnum) => abilityEnum == AbilityEnum.AddShieldLeft;

        public IEnumerator Execute(InPlayCard executor, int value, PlayerController defender, PlayerController attacker)
        {
            var neighbors = executor.OwnerPlayer.GetFrontNeighborsCards(1, 2);
            var leftNeighbor = neighbors[0];
            if (!leftNeighbor.IsDead)
                leftNeighbor.Armor += value;
            yield break;
        }
    }
}