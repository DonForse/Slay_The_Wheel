using System.Collections;
using Features.Battles.Wheel;
using Features.Cards;

namespace Features.Battles.Core.Abilities
{
    public class AddShieldRightOnApplyAbilityStrategy : IOnApplyAbilityStrategy
    {
        public bool IsValid(AbilityEnum abilityEnum) => abilityEnum == AbilityEnum.AddShieldRight;

        public IEnumerator Execute(InPlayCard executor, int value, PlayerController defender, PlayerController attacker)
        {
            var neighbors = executor.OwnerPlayer.GetFrontNeighborsCards(1, 2);
            var leftNeighbor = neighbors[1];
            if (leftNeighbor.IsDead)
                yield break;
            
            leftNeighbor.Armor += value;
            yield return leftNeighbor.PlayGainArmor();
        }
    }
}