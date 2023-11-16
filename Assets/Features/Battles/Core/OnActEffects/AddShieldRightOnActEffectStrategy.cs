using Features.Battles.Wheel;

namespace Features.Battles.Core.OnActEffects
{
    public class AddShieldRightOnActEffectStrategy : IOnActEffectStrategy
    {
        public bool IsValid(Ability ability) => ability == Ability.AddShieldRight;

        public void Execute(PlayerController defenderPlayerController, PlayerController attackerPlayerController)
        {
            var neighbors = attackerPlayerController.GetFrontNeighborsCards(1, 2);
            var rightNeighbor = neighbors[1];
            if (!rightNeighbor.IsDead)
                rightNeighbor.Armor += 5;
        }
    }
}