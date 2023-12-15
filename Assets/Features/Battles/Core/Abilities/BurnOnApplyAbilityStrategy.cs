using System;
using System.Collections;
using Features.Battles.Wheel;
using Features.Cards;
using Features.Cards.InPlay;

namespace Features.Battles.Core.Abilities
{
    public class BurnOnApplyAbilityStrategy : IOnApplyAbilityStrategy
    {
        public bool IsValid(AbilityEnum abilityEnum) => abilityEnum == AbilityEnum.Burn;

        public IEnumerator Execute(Ability ability,InPlayCard executor, PlayerController defender,
            PlayerController attacker)
        {
            foreach (var data in ability.AbilityData)
            {
                switch (data.Target)
                {
                    case TargetEnum.Self:
                        break;
                    case TargetEnum.Left:
                        break;
                    case TargetEnum.Right:
                        break;
                    case TargetEnum.AllAllies:
                        foreach (var card in attacker.Cards)
                        {
                            card.UpdateEffect(EffectEnum.Fire, data.Amount);
                        }
                        break;
                    case TargetEnum.Enemy:
                        var affectedCard = defender.GetFrontCard();
                        affectedCard.UpdateEffect(EffectEnum.Fire, data.Amount);
                        break;
                    case TargetEnum.AllEnemies:
                        foreach (var card in defender.Cards)
                        {
                            card.UpdateEffect(EffectEnum.Fire, data.Amount);
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            yield break;
        }
    }
}