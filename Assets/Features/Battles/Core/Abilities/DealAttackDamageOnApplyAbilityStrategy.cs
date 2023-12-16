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

        public IEnumerator Execute(Ability ability,InPlayCard executor, PlayerController enemyWheel, PlayerController executorWheel)
        {
            foreach (var data in ability.AbilityData)
            {
                var targets = TargetSystem.GetTargets(data.Target, executor, executorWheel, enemyWheel);
                foreach (var target in targets)
                {
                    yield return executor.PlayRangedAttack(target);
                    yield return _battle.ApplyDamage(executor.GetCard().Attack, target, executor, enemyWheel, null);
                }
            }
            
            yield break;
        }
    }
}