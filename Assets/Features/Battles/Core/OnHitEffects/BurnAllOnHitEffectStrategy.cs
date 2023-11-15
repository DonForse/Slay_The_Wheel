using System.Collections;
using Features.Battles.Wheel;

namespace Features.Battles
{
    public class BurnAllOnHitEffectStrategy : IOnHitEffectStrategy
    {
        public bool IsValid(Ability ability) => ability == Ability.BurnAll;

        public IEnumerator Execute(WheelController defenderWheelController, int count)
        {
            foreach (var card in defenderWheelController.Cards)
            {
                for (int i = 0; i < count; i++)
                    card.AddEffect(Ability.Burn);
            }
            yield break;
        }
    }
}