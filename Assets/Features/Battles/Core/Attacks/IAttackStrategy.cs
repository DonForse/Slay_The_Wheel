using System.Collections;
using Features.Battles.Wheel;
using Features.Cards;
using Features.Cards.InPlay;

namespace Features.Battles.Core.Attacks
{
    public interface IAttackStrategy
    {
        bool IsValid(AttackType attackType);
        IEnumerator Execute(InPlayCard attackerCard, PlayerController defenderPlayerController);
    }
}