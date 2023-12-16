using System.Collections;
using Features.Battles.Wheel;
using Features.Cards.InPlay;

namespace Features.Battles.Core.Abilities
{
    public interface IOnApplyAbilityStrategy
    {
        bool IsValid(AbilityEnum abilityEnum);
        IEnumerator Execute(Ability ability, InPlayCard executor, PlayerController enemyWheel, PlayerController executorWheel);
    }
}