using System.Collections;
using Features.Battles.Wheel;
using Features.Cards;

namespace Features.Battles.Core.Attacks
{
    public class FrontAttackStrategy : IAttackStrategy
    {
        private readonly Battle _battle;
        public FrontAttackStrategy(Battle battle)
        {
            _battle = battle;
        }
        public bool IsValid(AttackType attackType) => attackType == AttackType.Front;

        public IEnumerator Execute(InPlayCard attackerCard, PlayerController defenderPlayerController)
        {
            attackerCard.PlayAct();
            var defender = defenderPlayerController.GetFrontCard();

            _battle.ApplyDamage(attackerCard.Attack, defender, defenderPlayerController, null);
            yield break;
        }
    }
}