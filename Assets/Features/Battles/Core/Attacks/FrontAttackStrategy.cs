using System.Collections;
using Features.Battles.Wheel;
using Features.Cards;
using Features.Cards.InPlay;

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
            yield return attackerCard.PlayAttack();
            var defender = defenderPlayerController.GetFrontCard();

            yield return _battle.ApplyDamage(attackerCard.GetCard().Attack, defender,attackerCard, null);
            yield break;
        }
    }
}