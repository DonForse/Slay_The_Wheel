using System.Collections;
using Features.Battles.Wheel;
using Features.Cards.InPlay;

namespace Features.Battles.Core.Abilities
{
    public class AddAtkLeftOnApplyAbilityStrategy : IOnApplyAbilityStrategy
    {
        public bool IsValid(AbilityEnum abilityEnum) => abilityEnum == AbilityEnum.AddAtkLeft;

        public IEnumerator Execute(InPlayCard executor, int amount, PlayerController defender,
            PlayerController attacker)
        {
            var neighbors = executor.OwnerPlayer.GetNeighborsCards(executor,1, 2);
            var leftNeighbor = neighbors[0];
            if (!leftNeighbor.IsDead)
                leftNeighbor.Attack += 1;
            yield break;
        }
    }
}