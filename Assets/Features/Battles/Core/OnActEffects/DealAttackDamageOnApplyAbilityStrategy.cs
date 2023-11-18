using System.Collections;
using Features.Battles.Wheel;
using Features.Cards;

namespace Features.Battles.Core.OnActEffects
{
    public class DealAttackDamageOnApplyAbilityStrategy : IOnApplyAbilityStrategy
    {
        private Battle _battle;

        public DealAttackDamageOnApplyAbilityStrategy(Battle battle)
        {
            _battle = battle;
        }

        public bool IsValid(AbilityEnum abilityEnum) => abilityEnum == AbilityEnum.DealAttackDamage;

        public IEnumerator Execute(InPlayCard executor, int value, PlayerController defender, PlayerController attacker)
        {
            var defenderCard = defender.GetFrontCard();
            _battle.ApplyDamage(executor.Attack, defenderCard, defender, null);
            yield break;
        }
    }
}