// using System.Collections;
// using System.Collections.Generic;
// using Features.Battles.Wheel;
// using Features.Cards;
//
// namespace Features.Battles
// {
//     public class DamageSystem
//     {
//         public IEnumerator ApplyDamage(int damage, InPlayCard defender, PlayerController defenderPlayerController,
//             Ability? source)
//         {
//             var defenderCard = defender.GetCard();
//             defender.PlayGetHitAnimation(damage, source);
//             defenderCard.Hp -= damage;
//             if (defenderCard.Hp > 0)
//                 return;
//
//             defender.SetDead();
//
//             if (defenderPlayerController == enemyWheelController)
//             {
//                 if (_enemiesDeck.Count > 0)
//                 {
//                     var runCards = new List<RunCard>();
//                     var cards = DrawCards(1, ref _enemiesDeck, ref runCards);
//                     if (cards == null || cards.Count == 0)
//                         _busQueue.EnqueueInterruptAction(EndBattle(defenderPlayerController));
//                     else
//                         _busQueue.EnqueueInterruptAction(defender.SetCard(cards.First()));
//                 }
//
//                 if (_enemiesDeck.Count == 0 && defenderPlayerController.AllUnitsDead())
//                 {
//                     _busQueue.EnqueueInterruptAction(EndBattle(defenderPlayerController));
//                 }
//             }
//             else
//             {
//                 if (_playerBattleDeck.Count > 0)
//                 {
//                     var cards = DrawCards(1, ref _playerBattleDeck, ref _playerDiscardPile);
//                     if (cards == null || cards.Count == 0)
//                         _busQueue.EnqueueInterruptAction(EndBattle(defenderPlayerController));
//                     else
//                         _busQueue.EnqueueInterruptAction(defender.SetCard(cards.First()));
//                 }
//
//                 if (_playerBattleDeck.Count == 0 && _playerDiscardPile.Count == 0 &&
//                     defenderPlayerController.AllUnitsDead())
//                 {
//                     _busQueue.EnqueueInterruptAction(EndBattle(defenderPlayerController));
//                 }
//             }
//         }
//     }
// }