using Features.Battles.Wheel;

namespace Features.Battles
{
    public class BurnAllOnHitEffectStrategy : IOnHitEffectStrategy
    {
        public bool IsValid(Ability ability) => ability == Ability.BurnAll;

        public void Execute(WheelController defenderWheelController)
        {
            foreach (var card in defenderWheelController.Cards)
            {
                card.AddEffect(Ability.Burn);
            }
        }
    }
}