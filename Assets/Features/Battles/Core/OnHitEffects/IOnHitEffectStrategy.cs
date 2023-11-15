using Features.Battles.Wheel;

namespace Features.Battles
{
    public interface IOnHitEffectStrategy
    {
        bool IsValid(Ability ability);
        void Execute(WheelController defenderWheelController);
    }
}