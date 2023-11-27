using System.Collections;
using Features.Battles.Wheel;
using Features.Cards.InPlay;

namespace Features.Battles.Core.Abilities
{
    public class AddAtkRightOnApplyAbilityStrategy : IOnApplyAbilityStrategy
    {
        public bool IsValid(AbilityEnum abilityEnum) => abilityEnum == AbilityEnum.AddAtkRight;

        public IEnumerator Execute(InPlayCard executor, int amount, PlayerController defender,
            PlayerController attacker)
        {
            var neighbors = executor.OwnerPlayer.GetNeighborsCards(executor,1, 2);
            var leftNeighbor = neighbors[1];
            if (!leftNeighbor.IsDead)
                leftNeighbor.GetCard().Attack += 1;
            yield break;
        }
    }
}