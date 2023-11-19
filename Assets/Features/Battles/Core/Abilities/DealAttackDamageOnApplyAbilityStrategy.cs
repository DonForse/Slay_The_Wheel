using System.Collections;
using Features.Battles.Wheel;
using Features.Cards;

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

        public IEnumerator Execute(InPlayCard executor, int value, PlayerController defender, PlayerController attacker)
        {
            var defenderCard = defender.GetFrontCard();
            yield return _battle.ApplyDamage(executor.Attack, defenderCard, executor, defender, null);
            yield break;
        }
    }
}