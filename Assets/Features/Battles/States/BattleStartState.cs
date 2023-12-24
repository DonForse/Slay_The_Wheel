// using System.Collections;
// using Features.Common;
//
// namespace Features.Battles.States
// {
//     public class BattleStartState : IBattleState
//     {
//         private readonly Battle _battle;
//         private readonly CoroutineManager _coroutineManager;
//
//         public BattleStartState(Battle battle, CoroutineManager coroutineManager)
//         {
//             _battle = battle;
//             _coroutineManager = coroutineManager;
//         }
//
//         public IEnumerator EnterState()
//         {
//             yield return _coroutineManager.ExecuteCoroutines(
//                 _battle.enemyController.ShowCards(),
//                 _battle.playerController.ShowCards());
//         }
//
//         public IEnumerator ExitState()
//         {
//             yield break;
//         }
//
//         public IEnumerator UpdateState()
//         {
//             yield return ApplyOnBattleStartAbilities();
//         }
//         
//         private IEnumerator ApplyOnBattleStartAbilities()
//         {
//             foreach (var card in _battle.playerController.Cards)
//             {
//                 foreach (var ability in card.GetCard().OnBattleStartAbilities)
//                 {
//                     foreach (var strategy in _battle._applyAbilityStrategies)
//                     {
//                         if (strategy.IsValid(ability.Type))
//                             yield return strategy.Execute(ability, card,
//                                 _battle.enemyController, _battle.playerController);
//                     }
//                 }
//             }
//             
//             foreach (var card in _battle.enemyController.Cards)
//             {
//                 foreach (var ability in card.GetCard().OnBattleStartAbilities)
//                 {
//                     foreach (var strategy in _battle._applyAbilityStrategies)
//                     {
//                         if (strategy.IsValid(ability.Type))
//                             yield return strategy.Execute(ability, card,
//                                 _battle.playerController, _battle.enemyController);
//                     }
//                 }
//             }
//         }
//     }
// }