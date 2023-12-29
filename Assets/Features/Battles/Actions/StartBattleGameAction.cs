using System.Collections;
using Features.Battles.Managers;
using Features.Cards;
using Features.Common;

namespace Features.Battles.Actions
{
    public class StartBattleGameAction : IGameAction
    {
        private readonly BattleAbilitiesManager _battleAbilitiesManager;
        private readonly BattleRelicsManager _battleRelicsManager;
        private readonly BattleEffectsManager _battleEffectsManager;

        public StartBattleGameAction(Battle battle, 
            BattleAbilitiesManager battleAbilitiesManager, 
            BattleRelicsManager battleRelicsManager,
            BattleEffectsManager battleEffectsManager)
        {
            _battleAbilitiesManager = battleAbilitiesManager;
            _battleRelicsManager = battleRelicsManager;
            _battleEffectsManager = battleEffectsManager;
        }

        public IEnumerator Start()
        {
            //display ui/ux elements
            yield break;
        }

        public IEnumerator End()
        {
            //
            yield break;
        }

        public IEnumerator ApplyRelics()
        {
            yield return _battleRelicsManager.ApplyRelicEffect(BattleEventEnum.StartBattle);

            //throw new System.NotImplementedException();
        }

        public IEnumerator ApplyAbilities()
        {
            yield return _battleAbilitiesManager.ApplyAbilitiesEffect(BattleEventEnum.StartBattle);
        }

        public IEnumerator ApplyEffects()
        {
            yield return _battleEffectsManager.ApplyEffects(BattleEventEnum.StartBattle);
        }
    }
}