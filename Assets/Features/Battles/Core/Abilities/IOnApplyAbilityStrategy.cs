using System.Collections;
using Features.Battles.Wheel;
using Features.Cards;

namespace Features.Battles
{
    public interface IOnApplyAbilityStrategy
    {
        bool IsValid(AbilityEnum abilityEnum);
        IEnumerator Execute(InPlayCard executor, int amount, PlayerController defender, PlayerController attacker);
    }
}