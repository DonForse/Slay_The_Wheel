using Features.Battles.Wheel;

namespace Features.Battles.Core.OnActEffects
{
    public class AddAtkRightOnActEffectStrategy : IOnActEffectStrategy
    {
        public bool IsValid(Ability ability) => ability == Ability.AddAtkRight;

        public void Execute(PlayerController defenderPlayerController, PlayerController attackerPlayerController)
        {
            var neighbors = attackerPlayerController.GetFrontNeighborsCards(1, 2);
            var leftNeighbor = neighbors[1];
            if (!leftNeighbor.IsDead)
                leftNeighbor.Attack += 1;
        }
    }
}