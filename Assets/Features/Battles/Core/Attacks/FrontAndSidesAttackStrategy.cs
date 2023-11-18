using System.Collections;
using System.Linq;
using Features.Battles.Wheel;
using Features.Cards;

namespace Features.Battles.Core.Attacks
{
    public class FrontAndSidesAttackStrategy : IAttackStrategy
    {
        public FrontAndSidesAttackStrategy(Battle battle)
        {
            _battle = battle;
        }

        private readonly Battle _battle;
        public bool IsValid(AttackType attackType) => attackType == AttackType.FrontAndSides;

        public IEnumerator Execute(InPlayCard attackerCard, PlayerController defenderPlayerController)
        {
            attackerCard.PlayAct();
            var defenders = defenderPlayerController.GetFrontNeighborsCards(0, 2).ToList();
            foreach (var defender in defenders)
                yield return _battle.ApplyDamage(attackerCard.Attack, defender, attackerCard,defenderPlayerController, null);
            yield break;
        }
    }
}