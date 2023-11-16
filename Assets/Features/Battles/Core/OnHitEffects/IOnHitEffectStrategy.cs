using System.Collections;
using Features.Battles.Wheel;

namespace Features.Battles
{
    public interface IOnHitEffectStrategy
    {
        bool IsValid(Ability ability);
        IEnumerator Execute(PlayerController defenderPlayerController, int amount);
    }
}