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
        [SerializeField] private ActionsView actionsView;
        [SerializeField] private SpellView[] spellViews;

        private int _actions;
        private int turn = 0;
        private List<RunCard> _playerBattleDeck;
        private List<RunCard> _playerDiscardPile;
        private List<RunCard> _enemiesDeck;
        private RunCard _heroCard;
        private bool acting;
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
            cards = new List<RunCard>() { heroCard }.Concat(cards).ToList();
            StartCoroutine(enemyWheelController.InitializeWheel(false, enemyWheelSize, enemies));
            yield return playerWheelController.InitializeWheel(true, playerWheelSize, cards);

            enemyWheelController.LockWheel();
            playerWheelController.UnlockWheel();

            playerWheelController.Acted += OnPlayerActed;
            enemyWheelController.Acted += OnEnemyActed;
            playerWheelController.SetWheelMovedCallback(OnPlayerWheelMoved);
            enemyWheelController.SetWheelMovedCallback(OnEnemyWheelMoved);

            // playerWheelController.WheelTurn += OnPlayerWheelMoved;
            // enemyWheelController.WheelTurn += OnEnemyWheelMoved;
            SetActions(3);
        }

        private void OnDestroy()
        {
            playerWheelController.Acted -= OnPlayerActed;
            enemyWheelController.Acted -= OnEnemyActed;
        }

        private IEnumerator OnEnemyWheelMoved()
        {
            Debug.Log($"<color=yellow>{"OnEnemyWheelMoved"}</color>");
            yield return SpinWheel(enemyWheelController);
        }

        private IEnumerator OnPlayerWheelMoved()
        {
            Debug.Log($"<color=yellow>{"OnPlayerWheelMoved"}</color>");
            yield return SpinWheel(playerWheelController);
        }

        private void OnEnemyActed(object sender, InPlayCard attacker)
        {
            var attackerCard = attacker.GetCard();
            var actCost = attackerCard.ActCost;

            if (_actions  > 0 && actCost> _actions )
            {
                _busQueue.EnqueueAction(RevertAction(enemyWheelController));
                return;
            }
            _busQueue.EnqueueAction(Act(attacker,
                enemyWheelController,
                playerWheelController));
        }

        private void OnPlayerActed(object sender, InPlayCard attacker)
        {
            var attackerCard = attacker.GetCard();
            var actCost = attackerCard.ActCost;
            
            if (_actions  > 0 && actCost> _actions )
            {
                _busQueue.EnqueueAction(RevertAction(playerWheelController));
                return;
            }

            _busQueue.EnqueueAction(Act(attacker,
                playerWheelController,
                enemyWheelController));
        }

        private IEnumerator RevertAction(WheelController wheelController)
        {
            _busQueue.EnqueueAction(ActStartCoroutine(wheelController));
            _busQueue.EnqueueAction(RevertWheelPosition(wheelController));
            _busQueue.EnqueueAction(ActEndCoroutine(wheelController));
            yield break;
            // throw new NotImplementedException();
        }

        private IEnumerator ProcessAct(InPlayCard attacker, WheelController attackerWheelController,
            WheelController defenderWheelController, RunCard attackerCard)
        {
            if (attacker.IsDead)
            {
                _busQueue.EnqueueAction(attackerWheelController.PutAliveUnitAtFront(true));
                _busQueue.EnqueueAction(ProcessAttackerDiedInWheel(attacker, attackerWheelController));
                yield break;
            }

            _busQueue.EnqueueAction(ApplyFrontCardEffect(attackerWheelController, defenderWheelController));
            _busQueue.EnqueueAction(ApplyFrontCardAttack(attacker, defenderWheelController));
            _busQueue.EnqueueAction(ApplyAfterHitEffect(attackerCard, defenderWheelController));
            if (CompletedActions())
            {
                _busQueue.EnqueueAction(ChangeTurn());
                yield break;
            }

            _busQueue.EnqueueAction(ActEndCoroutine(attackerWheelController));
        }

        private IEnumerator ActEndCoroutine(WheelController attackerWheelController)
        {
            acting = false;
            Debug.Log($"<color=green>{"ACT-END"}</color>");
            attackerWheelController.UnlockWheel();
            yield return null;
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

        private IEnumerator ActStartCoroutine(WheelController attackerWheelController)
        {
            acting = true;
            attackerWheelController.LockWheel();
            yield return null;
        }

        private IEnumerator ProcessAttackerDiedInWheel(InPlayCard attacker, WheelController attackerWheelController)
        {
            if (attackerWheelController.AllUnitsDead())
                yield return EndBattle(attackerWheelController);
            if (CompletedActions())
            {
                _busQueue.EnqueueAction(ActEndCoroutine(attackerWheelController));
                _busQueue.EnqueueAction(ChangeTurn());
                yield break; // yield break;
            }
            yield break;
        }

        private IEnumerator SpinWheel(WheelController wheelController)
        {
            yield return ApplyWheelMovementEffect(wheelController);
            // yield return wheelController.PutAliveUnitAtFront(true);
        }

        private IEnumerator ApplyAfterHitEffect(RunCard attackerCard, WheelController defenderWheelController)
        {
            foreach (var group in attackerCard.Abilities.GroupBy(x => x))
            {
                if (group.Key == Ability.RotateRight)
                    yield return defenderWheelController.RotateRight(group.Count());
                if (group.Key == Ability.RotateLeft)
                    yield return defenderWheelController.RotateLeft(group.Count());
            }

            yield return null;
        }

        private IEnumerator Act(InPlayCard attacker, WheelController attackerWheelController,
            WheelController defenderWheelController)
        {
            var attackerCard = attacker.GetCard();
            SetActions(_actions - (attackerCard.IsDead ? 1 : attackerCard.ActCost));

            _busQueue.EnqueueAction(ActStartCoroutine(attackerWheelController));
            if (attackerCard.IsDead)
            {
                _busQueue.EnqueueAction(attackerWheelController.RepeatActMove());
                _busQueue.EnqueueAction(ActEndCoroutine(attackerWheelController));
                yield break; 
            }
            _busQueue.EnqueueAction(ApplyWheelMovementEffect(attackerWheelController));
            _busQueue.EnqueueAction(
                ProcessAct(attacker, attackerWheelController, defenderWheelController, attackerCard));
            yield break;
        }

        private IEnumerator ApplyFrontCardAttack(InPlayCard attackerCard, WheelController defenderWheelController)
        {
            attackerCard.PlayAct();

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

            yield return defenderWheelController.PutAliveUnitAtFront(true);
        }

        private void ApplyDamage(int damage, InPlayCard defender, WheelController defenderWheelController,
            Ability? source)
        {
            var defenderCard = defender.GetCard();
            defender.PlayGetHitAnimation(damage, source);
            defenderCard.Hp -= damage;
            if (defenderCard.Hp > 0)
                return;

            defender.SetDead();

            if (defenderWheelController == enemyWheelController)
            {
                if (_enemiesDeck.Count > 0)
                {
                    var runCards = new List<RunCard>();
                    var cards = DrawCards(1, ref _enemiesDeck, ref runCards);
                    if (cards == null || cards.Count == 0)
                        _busQueue.EnqueueInterruptAction(EndBattle(defenderWheelController));
                    else
                        _busQueue.EnqueueInterruptAction(defender.SetCard(cards.First()));
                }

                if (_enemiesDeck.Count == 0 && defenderWheelController.AllUnitsDead())
                {
                    _busQueue.EnqueueInterruptAction(EndBattle(defenderWheelController));
                }
            }
            else
            {
                if (_playerBattleDeck.Count > 0)
                {
                    var cards = DrawCards(1, ref _playerBattleDeck, ref _playerDiscardPile);
                    if (cards == null || cards.Count == 0)
                        _busQueue.EnqueueInterruptAction(EndBattle(defenderWheelController));
                    else
                        _busQueue.EnqueueInterruptAction(defender.SetCard(cards.First()));
                }

                if (_playerBattleDeck.Count == 0 && _playerDiscardPile.Count == 0 &&
                    defenderWheelController.AllUnitsDead())
                {
                    _busQueue.EnqueueInterruptAction(EndBattle(defenderWheelController));
                }
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

        private IEnumerator RevertWheelPosition(WheelController wheelController)
        {
            yield return wheelController.RevertLastMovement();
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
            SetActions(3);

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
                _busQueue.EnqueueAction(TryExecuteBotAction());
                yield return new WaitForSeconds(3f);
            }
        }

        private IEnumerator TryExecuteBotAction()
        {
            if (IsPlayerTurn()) yield break;
            if (_actions <=0)
                yield break;
            yield return botControlWheel.TurnTowardsDirection(Random.Range(0, 2) == 1);
        }

        private IEnumerator StartPlayerTurn()
        {
            foreach (var spell in spellViews)
            {
                spell.ReduceCooldown();
            }

            yield return turnMessage.Show(true);
            enemyWheelController.LockWheel();
            playerWheelController.UnlockWheel();
        }

        [UsedImplicitly]
        public void SkipTurn()
        {
            if (acting) return;
            if (!IsPlayerTurn()) return;
            SetActions(_actions - 1);
            if (CompletedActions())
                _busQueue.EnqueueAction(ChangeTurn());
        }


        [UsedImplicitly]
        public void Spin()
        {
            if (acting) return;
            if (IsPlayerTurn())
                return;
            // _busQueue.EnqueueAction(ChangeTurn());
        }

        [UsedImplicitly]
        public void Shuffle()
        {
            if (acting) return;
            if (!IsPlayerTurn()) return;
            SetActions(_actions - 1);
            _busQueue.EnqueueAction(ShuffleCoroutine());
            if (CompletedActions())
                _busQueue.EnqueueAction(ChangeTurn());
            ;
        }

        [UsedImplicitly]
        public void UseSkill(int skillIndex)
        {
            if (acting) return;
            if (!IsPlayerTurn()) return;
            if (_actions < spellViews[skillIndex - 1].actionCost) return;
            spellViews[skillIndex - 1].Activate();
            if (skillIndex == 1)
            {
                foreach (var card in enemyWheelController.Cards)
                {
                    card.AddEffect(Ability.Burn);
                    card.AddEffect(Ability.Burn);
                    card.AddEffect(Ability.Burn);
                }
            }
            else if (skillIndex == 2)
            {
                foreach (var card in enemyWheelController.Cards)
                {
                    var totalBurns = card.Effects.Count(x => x == Ability.Burn);
                    for (int i = 0; i < totalBurns; i++)
                    {
                        card.AddEffect(Ability.Burn);
                    }
                }
            }
            else if (skillIndex == 3)
            {
                _busQueue.EnqueueAction(WheelOfDeath());
            }

            SetActions(_actions - spellViews[skillIndex - 1].actionCost);
            if (CompletedActions())
                _busQueue.EnqueueAction(ChangeTurn());
        }

        private IEnumerator WheelOfDeath()
        {
            acting = true;
            playerWheelController.LockWheel();
            var burns = enemyWheelController.Cards.Max(x => x.Effects.Count(x => x == Ability.Burn));
            while (burns > 0 && !enemyWheelController.AllUnitsDead())
            {
                if (enemyWheelController.AllUnitsDead()) yield break;
                yield return enemyWheelController.RotateRight(burns);
                burns = enemyWheelController.Cards.Max(x => x.Effects.Count(x => x == Ability.Burn));
            }
            playerWheelController.UnlockWheel();
            acting = false;
        }

        private IEnumerator ShuffleCoroutine()
        {
            acting = true;
            var slots = playerWheelController.Cards.Count;
            var cards = DrawCards(slots - 1, ref _playerBattleDeck, ref _playerDiscardPile);
            var cardsInWheel = playerWheelController.Cards.Select(x => x.GetCard()).ToList();
            cardsInWheel.Remove(_heroCard);
            cards = new List<RunCard>() { _heroCard }.Concat(cards).ToList();
            _playerDiscardPile = _playerDiscardPile.Concat(cardsInWheel).ToList();
            for (int i = 0; i < slots; i++)
            {
                yield return playerWheelController.Cards[i].SetCard(cards[i]);
            }

            acting = false;
        }

        private bool CompletedActions() => _actions <= 0;

        private void SetActions(int amount)
        {
            _actions = amount;
            if (_actions < 0)
                _actions = Mathf.Min(_actions, 0);
            actionsView.ShowRemaining(_actions);
            foreach (var spellView in spellViews)
            {
                spellView.SetActivateable(spellView.actionCost <= _actions);
            }
        }
    }
}