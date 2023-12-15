using System.Collections;
using Features.Battles.Wheel;
using Features.Cards;
using Features.Cards.InPlay;

namespace Features.Battles.Core.Abilities
{
    public class DealAttackDamageOnApplyAbilityStrategy : IOnApplyAbilityStrategy
    {
        private Battle _battle;

        public DealAttackDamageOnApplyAbilityStrategy(Battle battle)
        {
            _battle = battle;
        }

        public bool IsValid(AbilityEnum abilityEnum) => abilityEnum == AbilityEnum.DealAttackDamage;

        public IEnumerator Execute(Ability ability,InPlayCard executor, PlayerController defender, PlayerController attacker)
        {
            var defenderCard = defender.GetFrontCard();
            yield return executor.PlayRangedAttack(defenderCard);
            yield return _battle.ApplyDamage(executor.GetCard().Attack, defenderCard, executor, defender, null);
            yield break;
        }
    }
}