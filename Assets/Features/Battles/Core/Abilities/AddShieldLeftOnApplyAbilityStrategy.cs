using System.Collections;
using Features.Battles.Wheel;
using Features.Cards;
using Features.Cards.InPlay;

namespace Features.Battles.Core.Abilities
{
    public class AddShieldLeftOnApplyAbilityStrategy : IOnApplyAbilityStrategy
    {
        public bool IsValid(AbilityEnum abilityEnum) => abilityEnum == AbilityEnum.AddShieldLeft;

        public IEnumerator Execute(InPlayCard executor, int value, PlayerController defender, PlayerController attacker)
        {
            var neighbors = executor.OwnerPlayer.GetNeighborsCards(executor, 1,2);
            var leftNeighbor = neighbors[0];
            if (leftNeighbor.IsDead) yield break;
                
            leftNeighbor.Armor += value;
            yield return leftNeighbor.PlayGainArmor();
        }
    }
}