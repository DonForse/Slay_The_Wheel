using System.Collections;
using System.Collections.Generic;
using Features.Battles.Core.Effects;
using Features.Cards;
using Features.Common;

namespace Features.Battles.Managers
{
    public class BattleEffectsManager
    {
        private Battle _battle;
        private List<IOnApplyEffectStrategy> _effects;
        private readonly CoroutineManager _coroutineManager;

        public BattleEffectsManager(Battle battle, CoroutineManager coroutineManager)
        {
            _battle = battle;
            _coroutineManager = coroutineManager;
            _effects = new()
            {
                new BurnOnApplyEffectStrategy(_battle),
                new OilOnApplyEffectStrategy(),
                new BombOnApplyEffectStrategy(_battle)
            };
        }

        public IEnumerator ApplyEffects(BattleEventEnum battleEventEnum)
        {
            var coroutinesToExecute = new List<IEnumerator>();
            foreach (var card in _battle.playerController.Cards)
            {
                foreach (var cardAbility in card.Effects)
                {
                    foreach (var ability in _effects)
                    {
                        if (ability.IsValid(cardAbility, battleEventEnum))
                        {
                            coroutinesToExecute.Add(ability.Execute(cardAbility, card));
                        }
                    }
                }
            }

            yield return _coroutineManager.ExecuteCoroutines(coroutinesToExecute.ToArray());
        }
    }
}