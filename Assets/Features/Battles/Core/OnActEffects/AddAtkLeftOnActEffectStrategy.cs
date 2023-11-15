using Features.Battles.Wheel;

namespace Features.Battles.Core.OnActEffects
{
    public class AddAtkLeftOnActEffectStrategy : IOnActEffectStrategy
    {
        public bool IsValid(Ability ability) => ability == Ability.AddAtkLeft;

        public void Execute(WheelController defenderWheelController, WheelController attackerWheelController)
        {
            var neighbors = attackerWheelController.GetFrontNeighborsCards(1, 2);
            var leftNeighbor = neighbors[0];
            if (!leftNeighbor.IsDead)
                leftNeighbor.Attack += 1;
        }
    }
}