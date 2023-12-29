using Features.Battles.Core;
using Features.Cards.InPlay;

namespace Features.Battles.Wheel
{
    public interface IPlayerController
    {
        void SpinWheel(WheelRotation wheelRotation);
        void ExecuteAbility(AbilityEnum abilityEnum);
        void SwapCard(InPlayCard card);
    }
}