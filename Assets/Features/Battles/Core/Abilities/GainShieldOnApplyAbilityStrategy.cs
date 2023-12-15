using System;
using System.Collections;
using Features.Battles.Wheel;
using Features.Cards;
using Features.Cards.InPlay;

namespace Features.Battles.Core.Abilities
{
    public class GainShieldOnApplyAbilityStrategy : IOnApplyAbilityStrategy
    {
        public bool IsValid(AbilityEnum abilityEnum) => abilityEnum == AbilityEnum.GainShield;

        public IEnumerator Execute(Ability ability, InPlayCard executor, PlayerController defender,
            PlayerController attacker)
        {
            foreach (var data in ability.AbilityData)
            {
                switch (data.Target)
                {
                    case TargetEnum.Self:
                        if (executor.IsDead)
                            yield break;
                        executor.GetCard().Armor += data.Amount;
                        yield return executor.PlayGainShield();
                        break;
                    case TargetEnum.Left:
                        var leftNeighbor = executor.OwnerPlayer.GetNeighborsCards(executor, 1,2)[0];
                        if (leftNeighbor.IsDead)
                            yield break;
                
                        leftNeighbor.GetCard().Armor += data.Amount;
                        yield return leftNeighbor.PlayGainShield();
                        break;
                    case TargetEnum.Right:
                        var rightNeighbor = executor.OwnerPlayer.GetNeighborsCards(executor,1, 2)[1];
                        if (rightNeighbor.IsDead)
                            yield break;
            
                        rightNeighbor.GetCard().Armor += data.Amount;
                        yield return rightNeighbor.PlayGainShield();
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

            

        }
    }
}