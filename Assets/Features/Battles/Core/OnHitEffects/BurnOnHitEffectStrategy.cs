using Features.Battles.Wheel;

namespace Features.Battles
{
    public class BurnOnHitEffectStrategy : IOnHitEffectStrategy
    {
        public bool IsValid(Ability ability) => ability == Ability.Burn;
        public void Execute(WheelController defenderWheelController)
        {
            var affectedCard = defenderWheelController.GetFrontCard();
            affectedCard.AddEffect(Ability.Burn);
        }
    }
}