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

        public IEnumerator Execute(PlayerController defenderPlayerController, int count)
        {
            yield return defenderPlayerController.Rotate(ActDirection.Right, count);
        }
    }
}