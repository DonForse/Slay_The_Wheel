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

        public IEnumerator Execute(Ability ability,InPlayCard executor, PlayerController defender, PlayerController attacker)
        {
            _executingMultiAttack = true;
            for (int i = 0; i < ability.AbilityData.First().Amount; i++)
            {
                yield return _battle.ApplyFrontCardAttack(executor, defender);
            }
            _executingMultiAttack = false;
        }
    }
}