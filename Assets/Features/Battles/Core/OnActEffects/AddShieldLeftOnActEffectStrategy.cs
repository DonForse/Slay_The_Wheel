using Features.Battles.Wheel;

namespace Features.Battles.Core.OnActEffects
{
    public class AddShieldLeftOnActEffectStrategy : IOnActEffectStrategy
    {
        public bool IsValid(Ability ability) => ability == Ability.AddShieldLeft;

        public void Execute(PlayerController defenderPlayerController, PlayerController attackerPlayerController)
        {
            var neighbors = attackerPlayerController.GetFrontNeighborsCards(1, 2);
            var leftNeighbor = neighbors[0];
            if (!leftNeighbor.IsDead)
                leftNeighbor.Armor += 5;
        }
    }
}