using System.Collections;
using Features.Battles.Wheel;

namespace Features.Battles
{
    public class RotateLeftOnHitEffectStrategy : IOnHitEffectStrategy
    {
        public bool IsValid(Ability ability)
        {
            return ability == Ability.RotateLeft;
        }

        public IEnumerator Execute(WheelController defenderWheelController, int count)
        {
            yield return defenderWheelController.Rotate(ActDirection.Left, count);
        }
    }
}