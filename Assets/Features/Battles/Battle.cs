using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Features.Battles.Wheel;
using Features.Cards;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Features.Battles
{
    public class Battle : MonoBehaviour
    {
        [SerializeField] private WheelController playerWheelController;
        [SerializeField] private WheelController enemyWheelController;
        [SerializeField] private BotControlWheel botControlWheel;
        [SerializeField] private BusQueue _busQueue;
        [SerializeField] private TurnMessage turnMessage;
        private int _actions;
        private int turn = 0;
        private List<RunCard> _playerBattleDeck;
        private List<RunCard> _playerDiscardPile;
        private List<RunCard> _enemiesDeck;
        private RunCard _heroCard;
        public event EventHandler<bool> BattleFinished;

        // Start is called before the first frame update
        public IEnumerator Initialize(List<RunCard> deck, List<RunCard> enemies, int playerWheelSize,
            int enemyWheelSize, RunCard heroCard)
        {
            _playerBattleDeck = deck.ToList();
            _playerDiscardPile = new();
            _enemiesDeck = enemies.Skip(enemyWheelSize).ToList();
            _heroCard = heroCard;
            var cards = DrawCards(playerWheelSize - 1, ref _playerBattleDeck, ref _playerDiscardPile);
            cards = cards.Concat(new List<RunCard>() { heroCard }).ToList();
            StartCoroutine(enemyWheelController.InitializeWheel(false, enemyWheelSize, enemies));
            yield return playerWheelController.InitializeWheel(true, playerWheelSize, cards);

            enemyWheelController.LockWheel();
            playerWheelController.UnlockWheel();

            playerWheelController.Acted += OnPlayerActed;
            enemyWheelController.Acted += OnEnemyActed;
            playerWheelController.WheelTurn += OnPlayerWheelMoved;
            enemyWheelController.WheelTurn += OnEnemyWheelMoved;
        }

        private void OnDestroy()
        {
            playerWheelController.Acted -= OnPlayerActed;
            enemyWheelController.Acted -= OnEnemyActed;
            playerWheelController.WheelTurn -= OnPlayerWheelMoved;
            enemyWheelController.WheelTurn -= OnEnemyWheelMoved;
        }

        private List<RunCard> DrawCards(int amountToDraw, ref List<RunCard> deck, ref List<RunCard> discardPile)
        {
            discardPile = discardPile.Where(x => !x.IsDead).ToList();
            if (deck.Count < amountToDraw)
            {
                deck = deck.Concat(discardPile).ToList();
                discardPile.Clear();
            }

            var cards = deck.Take(amountToDraw).ToList();
            deck = deck.Skip(amountToDraw).ToList();
            return cards;
        }

        private void OnEnemyWheelMoved(object sender, InPlayCard e)
        {
            Debug.Log($"<color=yellow>{"OnEnemyWheelMoved"}</color>");
            _busQueue.EnqueueInterruptAction(SpinWheel(enemyWheelController));
        }

        private void OnPlayerWheelMoved(object sender, InPlayCard e)
        {
            Debug.Log($"<color=yellow>{"OnPlayerWheelMoved"}</color>");
            _busQueue.EnqueueInterruptAction(SpinWheel(playerWheelController));
        }

        private void OnEnemyActed(object sender, InPlayCard attacker)
        {
            _actions++;
            _busQueue.EnqueueAction(Act(attacker,
                enemyWheelController,
                playerWheelController));
        }

        private void OnPlayerActed(object sender, InPlayCard attacker)
        {
            _actions++;
            _busQueue.EnqueueAction(Act(attacker,
                playerWheelController,
                enemyWheelController));
        }

        private IEnumerator Act(InPlayCard attacker, WheelController attackerWheelController,
            WheelController defenderWheelController)
        {
            var attackerCard = attacker.GetCard();
            _busQueue.EnqueueAction(ActStartCoroutine(attackerWheelController));
            _busQueue.EnqueueAction(ApplyWheelMovementEffect(attackerWheelController));
            if (attacker.IsDead)
            {
                _busQueue.EnqueueAction(ProcessAttackerDiedInWheel(attacker, attackerWheelController));
                yield break;
            }
            _busQueue.EnqueueAction(ApplyFrontCardEffect(attackerWheelController, defenderWheelController));
            _busQueue.EnqueueAction(ApplyFrontCardAttack(attacker, defenderWheelController));
            _busQueue.EnqueueAction(ApplyAfterHitEffect(attackerCard, defenderWheelController));
            if (_actions == 3)
            {
                _busQueue.EnqueueAction(ChangeTurn());
                yield break;
            }

            _busQueue.EnqueueAction(ActEndCoroutine(attackerWheelController));
        }

        private IEnumerator ActEndCoroutine(WheelController attackerWheelController)
        {
            Debug.Log($"<color=green>{"ACT-END"}</color>");
            attackerWheelController.UnlockWheel();
            yield return null;
        }

        private IEnumerator ActStartCoroutine(WheelController attackerWheelController)
        {
            attackerWheelController.LockWheel();
            yield return null;
        }

        private IEnumerator ProcessAttackerDiedInWheel(InPlayCard attacker, WheelController attackerWheelController)
        {
            if (attackerWheelController.AllUnitsDead())
                yield return EndBattle(attackerWheelController);
            if (_actions == 3)
            {
                _busQueue.EnqueueAction(ChangeTurn());
                yield break; // yield break;
            }
            yield break;
        }

        private IEnumerator SpinWheel(WheelController wheelController)
        {
            yield return ApplyWheelMovementEffect(wheelController);
        }

        private IEnumerator ApplyAfterHitEffect(RunCard attackerCard, WheelController defenderWheelController)
        {
            foreach (var ability in attackerCard.Abilities)
            {
                if (ability == Ability.RotateRight)
                    _busQueue.EnqueueInterruptAction(defenderWheelController.RotateRight());
                if (ability == Ability.RotateLeft)
                    _busQueue.EnqueueInterruptAction(defenderWheelController.RotateLeft());
            }
            yield return null;
        }

        private IEnumerator ApplyFrontCardAttack(InPlayCard attackerCard, WheelController defenderWheelController)
        {
            if (attackerCard.GetCard().AttackType == AttackType.Front)
            {
                var defender = defenderWheelController.GetFrontCard();

                ApplyDamage(attackerCard.Attack, defender, defenderWheelController, null);
            }
            else if (attackerCard.GetCard().AttackType == AttackType.All)
            {
                foreach (var defender in defenderWheelController.Cards)
                    ApplyDamage(attackerCard.Attack, defender, defenderWheelController, null);
            }
            else if (attackerCard.GetCard().AttackType == AttackType.FrontAndSides)
            {
                var defenders = defenderWheelController.GetFrontNeighborsCards(0, 2).ToList();
                foreach (var defender in defenders)
                    ApplyDamage(attackerCard.Attack, defender, defenderWheelController, null);
            }
            attackerCard.PlayAct();
            yield return null;
        }

        private void ApplyDamage(int damage, InPlayCard defender, WheelController defenderWheelController,
            Ability? source)
        {
            var defenderCard = defender.GetCard();
            defender.PlayGetHitAnimation(damage, source);
            defenderCard.Hp -= damage;
            if (defenderCard.Hp > 0)
            {
                return;
            }

            defender.SetDead();

            _busQueue.EnqueueAction(defenderWheelController.PutAliveUnitAtFront(true));
            var deck = defenderWheelController == enemyWheelController ? _enemiesDeck : _playerBattleDeck;
            var discardPile = defenderWheelController == enemyWheelController
                ? new List<RunCard>()
                : _playerDiscardPile;
            if (deck.Count > 0)
            {
                var cards = DrawCards(1, ref deck, ref discardPile);
                if (cards == null || cards.Count == 0)
                    _busQueue.EnqueueAction(EndBattle(defenderWheelController));
                else
                    _busQueue.EnqueueAction(defender.SetCard(cards.First()));
            }

            if (deck.Count == 0 && discardPile.Count == 0 && defenderWheelController.AllUnitsDead())
            {
                _busQueue.EnqueueAction(EndBattle(defenderWheelController));
            }
        }

        private IEnumerator EndBattle(WheelController defenderWheelController)
        {
            _busQueue.Clear();
            yield return new WaitForSeconds(.5f);
            BattleFinished?.Invoke(this, defenderWheelController == enemyWheelController);
        }

        private static IEnumerator ApplyFrontCardEffect(WheelController attackerWheelController,
            WheelController defenderWheelController)
        {
            var attackerCard = attackerWheelController.GetFrontCard().GetCard();
            foreach (var ability in attackerCard.Abilities)
            {
                if (ability == Ability.Burn)
                {
                    var affectedCard = defenderWheelController.GetFrontCard();
                    affectedCard.AddEffect(Ability.Burn);
                }

                if (ability == Ability.BurnAll)
                {
                    foreach (var card in defenderWheelController.Cards)
                    {
                        card.AddEffect(Ability.Burn);
                    }
                }

                if (ability == Ability.AddAtkLeft)
                {
                    var neighbors = attackerWheelController.GetFrontNeighborsCards(1, 2);
                    var leftNeighbor = neighbors[0];
                    if (!leftNeighbor.IsDead)
                        leftNeighbor.Attack += 1;
                }

                if (ability == Ability.AddAtkRight)
                {
                    var neighbors = attackerWheelController.GetFrontNeighborsCards(1, 2);
                    var rightNeighbor = neighbors[1];
                    if (!rightNeighbor.IsDead)
                        rightNeighbor.Attack += 1;
                }

                if (ability == Ability.AddShieldLeft)
                {
                    var neighbors = attackerWheelController.GetFrontNeighborsCards(1, 2);
                    var leftNeighbor = neighbors[0];
                    if (!leftNeighbor.IsDead)
                        leftNeighbor.Shield += 1;
                }

                if (ability == Ability.AddShieldRight)
                {
                    var neighbors = attackerWheelController.GetFrontNeighborsCards(1, 2);
                    var rightNeighbor = neighbors[1];
                    if (!rightNeighbor.IsDead)
                        rightNeighbor.Shield += 1;
                }
            }

            yield return null;
        }

        private IEnumerator ApplyWheelMovementEffect(WheelController wheelController)
        {
            foreach (var card in wheelController.Cards)
            {
                var cardActiveEffects = card.Effects;
                var burns = cardActiveEffects.Count(a => a == Ability.Burn);
                if (burns > 0)
                {
                    card.RemoveEffect(Ability.Burn);
                    ApplyDamage(burns, card, wheelController, Ability.Burn);
                }
            }
            yield break;
        }

        private IEnumerator ChangeTurn()
        {
            Debug.Log($"<color=cyan>{"ACT-END"}</color>");
            turn++;
            _actions = 0;

            if (IsPlayerTurn())
                _busQueue.EnqueueAction(StartPlayerTurn());
            else
                _busQueue.EnqueueAction(PlayBotTurn());
            yield return null;
        }

        private bool IsPlayerTurn() => turn % 2 == 0;

        private IEnumerator PlayBotTurn()
        {
            yield return turnMessage.Show(false);
            playerWheelController.LockWheel();
            enemyWheelController.UnlockWheel();
            StartCoroutine(BotAction());
        }

        private IEnumerator BotAction()
        {
            for (int i = 0; i < 3; i++)
            {
                _busQueue.EnqueueAction(botControlWheel.TurnTowardsDirection(Random.Range(0, 2) == 1));
                yield return new WaitForSeconds(3f);
            }
        }

        private IEnumerator StartPlayerTurn()
        {
            yield return turnMessage.Show(true);
            enemyWheelController.LockWheel();
            playerWheelController.UnlockWheel();
        }

        [UsedImplicitly]
        public void SkipTurn()
        {
            if (IsPlayerTurn())
                ChangeTurn();
        }


        [UsedImplicitly]
        public void Spin()
        {
            if (IsPlayerTurn())
                return;
            // _busQueue.EnqueueAction(ChangeTurn());
        }

        [UsedImplicitly]
        public void Shuffle()
        {
            if (!IsPlayerTurn()) return;
            _actions++;
            _busQueue.EnqueueAction(ShuffleCoroutine());
            if (_actions == 3)
                ChangeTurn();
        }

        private IEnumerator ShuffleCoroutine()
        {
            var slots = playerWheelController.Cards.Count;
            var cards = DrawCards(slots -1, ref _playerBattleDeck, ref _playerDiscardPile);
            var cardsInWheel = playerWheelController.Cards.Select(x => x.GetCard()).ToList();
            cardsInWheel.Remove(_heroCard);
            cards = cards.Concat(new []{_heroCard}).ToList();
            _playerDiscardPile = _playerDiscardPile.Concat(cardsInWheel).ToList();
            for (int i = 0; i < slots; i++)
            {
                yield return playerWheelController.Cards[i].SetCard(cards[i]);
            }
        }
    }
}