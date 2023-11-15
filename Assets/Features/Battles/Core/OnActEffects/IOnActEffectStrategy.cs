using Features.Battles.Wheel;

namespace Features.Battles
{
    public interface IOnActEffectStrategy
    {
        bool IsValid(Ability ability);
        void Execute(WheelController defenderWheelController, WheelController attackerWheelController);
    }
}