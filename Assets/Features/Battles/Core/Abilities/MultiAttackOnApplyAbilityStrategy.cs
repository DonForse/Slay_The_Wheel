using System.Collections;
using System.Linq;
using Features.Battles.Wheel;
using Features.Cards;
using Features.Cards.InPlay;

namespace Features.Battles.Core.Abilities
{
    public class MultiAttackOnApplyAbilityStrategy : IOnApplyAbilityStrategy
    {
        private Battle _battle;
        private bool _executingMultiAttack = false;


        public MultiAttackOnApplyAbilityStrategy(Battle battle)
        {
            _battle = battle;
        }

        public bool IsValid(AbilityEnum abilityEnum) => !_executingMultiAttack && abilityEnum == AbilityEnum.MultiAttack;

        public IEnumerator Execute(Ability ability,InPlayCard executor, PlayerController enemyWheel, PlayerController executorWheel)
        {
            foreach (var data in ability.AbilityData)
            {
                var targets = TargetSystem.GetTargets(data.Target, executor, executorWheel, enemyWheel);
                foreach (var target in targets)
                {
                    _executingMultiAttack = true;
                    for (int i = 0; i < data.Amount; i++)
                    {
                        yield return _battle.ApplyFrontCardAttack(executor, enemyWheel);
                    }
                    _executingMultiAttack = false;
                }
            }
        }
    }
}