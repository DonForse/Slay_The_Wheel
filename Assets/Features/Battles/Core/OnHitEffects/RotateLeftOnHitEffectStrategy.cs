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

        public IEnumerator Execute(PlayerController defenderPlayerController, int count)
        {
            yield return defenderPlayerController.Rotate(ActDirection.Left, count);
        }
    }
}