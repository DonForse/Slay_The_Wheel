using System.Collections;
using Features.Battles.Wheel;
using Features.Cards;
using Features.Cards.InPlay;

namespace Features.Battles.Core.Abilities
{
    public class DealDamageOnApplyAbilityStrategy : IOnApplyAbilityStrategy
    {
        private Battle _battle;

        public DealDamageOnApplyAbilityStrategy(Battle battle)
        {
            _battle = battle;
        }

        public bool IsValid(AbilityEnum abilityEnum) => abilityEnum == AbilityEnum.DealAttackDamage;

        public IEnumerator Execute(Ability ability,InPlayCard executor, PlayerController enemyWheel, PlayerController executorWheel)
        {
            foreach (var data in ability.AbilityData)
            {
                var targets = TargetSystem.GetTargets(data.Target, executor, executorWheel, enemyWheel);
                foreach (var target in targets)
                {
                    yield return _battle.ApplyDamage(data.Amount, target, executor, null);
                }
            }
            
            yield break;
        }
    }
}