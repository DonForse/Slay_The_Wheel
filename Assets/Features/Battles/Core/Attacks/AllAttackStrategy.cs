using System.Collections;
using Features.Battles.Wheel;
using Features.Cards;
using Features.Cards.InPlay;

namespace Features.Battles.Core.Attacks
{
    public class AllAttackStrategy : IAttackStrategy
    {
        private readonly Battle _battle;
        public AllAttackStrategy(Battle battle)
        {
            _battle = battle;
        }
        public bool IsValid(AttackType attackType) => attackType == AttackType.All;

        public IEnumerator Execute(InPlayCard attackerCard, PlayerController defenderPlayerController)
        {
            yield return attackerCard.PlayAttack();
            foreach (var defender in defenderPlayerController.Cards)
                yield return _battle.ApplyDamage(attackerCard.GetCard().Attack, defender,attackerCard, defenderPlayerController, null);
        }
    }
}