using System.Collections;
using Features.Battles.Wheel;

namespace Features.Battles
{
    public class BurnOnHitEffectStrategy : IOnHitEffectStrategy
    {
        public bool IsValid(Ability ability) => ability == Ability.Burn;
        public IEnumerator Execute(WheelController defenderWheelController, int count)
        {
            var affectedCard = defenderWheelController.GetFrontCard();
            for (int i = 0; i < count; i++)
                affectedCard.AddEffect(Ability.Burn);
            yield break;
        }
    }
}