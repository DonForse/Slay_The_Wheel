using System.Collections;
using Features.Cards;
using Features.Cards.InPlay;

namespace Features.Battles.Core.Effects
{
    public interface IOnApplyEffectStrategy
    {
        bool IsValid(Effect effect, BattleEventEnum battleEventEnum);

        IEnumerator Execute(Effect effect, InPlayCard cardWithTheEffect);
    }
}