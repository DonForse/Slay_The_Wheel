using System;
using System.Collections.Generic;
using Features.Battles.Wheel;
using Features.Cards.InPlay;

namespace Features.Battles.Core.Abilities
{
    public class TargetSystem
    {
        public static List<InPlayCard> GetTargets(TargetEnum target, InPlayCard executor, PlayerController executorWheel,
            PlayerController enemyWheel)
        {
            var targets = new List<InPlayCard>();
            switch (target)
            {
                case TargetEnum.Self:
                    targets.Add(executor);
                    break;
                case TargetEnum.Left:
                    targets.Add(executorWheel.GetNeighborsCards(executor, 1,2)[0]);
                    break;
                case TargetEnum.Right:
                    targets.Add(executorWheel.GetNeighborsCards(executor, 1,2)[1]);
                    break;
                case TargetEnum.AllAllies:
                    targets.AddRange(executorWheel.Cards);
                    break;
                case TargetEnum.Enemy:
                    targets.Add(enemyWheel.GetFrontCard());
                    break;
                case TargetEnum.AllEnemies:
                    targets.AddRange(enemyWheel.Cards);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(target), target, null);
            }

            return targets;
        }
    }
}