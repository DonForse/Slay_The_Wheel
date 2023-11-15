using System.Collections;
using Features.Battles.Wheel;
using Features.Cards;

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

        public IEnumerator Execute(InPlayCard attackerCard, WheelController defenderWheelController)
        {
            attackerCard.PlayAct();
            foreach (var defender in defenderWheelController.Cards)
                _battle.ApplyDamage(attackerCard.Attack, defender, defenderWheelController, null);
            yield break;
        }
    }
}