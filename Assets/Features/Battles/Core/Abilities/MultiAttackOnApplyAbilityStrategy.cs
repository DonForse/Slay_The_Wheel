using System.Collections;
using Features.Battles.Wheel;
using Features.Cards;

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

        public IEnumerator Execute(InPlayCard executor, int value, PlayerController defender, PlayerController attacker)
        {
            _executingMultiAttack = true;
            for (int i = 0; i < value; i++)
            {
                
                yield return _battle.ApplyFrontCardAttack(executor, defender);
            }
            _executingMultiAttack = false;
        }
    }
}