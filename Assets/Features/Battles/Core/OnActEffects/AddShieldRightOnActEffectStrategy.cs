using Features.Battles.Wheel;

namespace Features.Battles.Core.OnActEffects
{
    public class AddShieldRightOnActEffectStrategy : IOnActEffectStrategy
    {
        public bool IsValid(Ability ability) => ability == Ability.AddShieldRight;

        public void Execute(WheelController defenderWheelController, WheelController attackerWheelController)
        {
            var neighbors = attackerWheelController.GetFrontNeighborsCards(1, 2);
            var rightNeighbor = neighbors[1];
            if (!rightNeighbor.IsDead)
                rightNeighbor.Shield += 1;
        }
    }
}