using System.Collections;
using Features.Battles.Wheel;

namespace Features.Battles
{
    public class RotateRightOnHitEffectStrategy : IOnHitEffectStrategy
    {
        public bool IsValid(Ability ability)
        {
            return ability == Ability.RotateRight;
        }

        public IEnumerator Execute(WheelController defenderWheelController, int count)
        {
            yield return defenderWheelController.Rotate(ActDirection.Right, count);
        }
    }
}