using System;
using System.Collections;
using Features.Battles.Wheel;
using Features.Cards.InPlay;

namespace Features.Battles.Core.Abilities
{
    public class GainAttackOnApplyAbilityStrategy : IOnApplyAbilityStrategy
    {
        public bool IsValid(AbilityEnum abilityEnum) => abilityEnum == AbilityEnum.GainAtk;

        public IEnumerator Execute(Ability ability, InPlayCard executor, PlayerController defender,
            PlayerController attacker)
        {
            foreach (var data in ability.AbilityData)
            {
                switch (data.Target)
                {
                    case TargetEnum.Self:
                        executor.GetCard().Attack += data.Amount;
                        break;
                    case TargetEnum.Left:
                        var leftNeighbor = executor.OwnerPlayer.GetNeighborsCards(executor,1, 2)[0];
                        if (!leftNeighbor.IsDead)
                            leftNeighbor.GetCard().Attack += data.Amount;;
                        break;
                    case TargetEnum.Right:
                        var rightNeighbor = executor.OwnerPlayer.GetNeighborsCards(executor,1, 2)[1];
                        if (!rightNeighbor.IsDead)
                            rightNeighbor.GetCard().Attack += data.Amount;;
                        break;
                    case TargetEnum.AllAllies:
                        break;
                    case TargetEnum.Enemy:
                        break;
                    case TargetEnum.AllEnemies:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            yield break;
        }
    }
}