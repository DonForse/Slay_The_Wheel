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

        private int _actions;
        private int turn = 0;
        private bool _acting;
        private List<RunCard> _playerBattleDeck;
        private List<RunCard> _playerDiscardPile;
        private List<RunCard> _enemiesDeck;
        private bool _applyingDamage;
        public event EventHandler<bool> BattleFinished;

        // Start is called before the first frame update
        public IEnumerator Initialize(List<RunCard> deck, List<RunCard> enemies, int playerWheelSize,
            int enemyWheelSize)
        {
            _playerBattleDeck = deck.ToList();
            _playerDiscardPile = new();
            _enemiesDeck = enemies.Skip(enemyWheelSize).ToList();
            var cards = DrawCards(playerWheelSize,ref _playerBattleDeck,ref _playerDiscardPile);
            yield return playerWheelController.InitializeWheel(true, playerWheelSize, cards);
            yield return enemyWheelController.InitializeWheel(false, enemyWheelSize, enemies);

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

        private List<RunCard> DrawCards(int amountToDraw,ref List<RunCard> deck, ref List<RunCard> discardPile)
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
            StartCoroutine(SpinWheel(enemyWheelController));
        }

        private void OnPlayerWheelMoved(object sender, InPlayCard e)
        {
            StartCoroutine(SpinWheel(playerWheelController));
        }

        private void OnEnemyActed(object sender, InPlayCard attacker)
        {
            _actions++;
            StartCoroutine(Act(attacker,
                enemyWheelController,
                playerWheelController));
        }

        private void OnPlayerActed(object sender, InPlayCard attacker)
        {
            _actions++;
            StartCoroutine(Act(attacker,
                playerWheelController,
                enemyWheelController));
        }

        private IEnumerator Act(InPlayCard attacker, WheelController attackerWheelController,
            WheelController defenderWheelController)
        {
            _acting = true;
            attackerWheelController.LockWheel();
            var attackerCard = attacker.GetCard();

            yield return ApplyWheelMovementEffect(attackerWheelController);
            yield return WaitSpinning();
            if (attacker.IsDead) //own unit dead on movement
            {
                if (attackerWheelController.AllUnitsDead())
                    yield return EndBattle(attackerWheelController);
                if (_actions == 3)
                {
                    yield return WaitSpinning();
                    ChangeTurn();
                    _acting = false;
                    yield break;
                }

                _acting = false;
                yield break;
            }

            ApplyFrontCardEffect(attackerWheelController, defenderWheelController);
            yield return ApplyFrontCardAttack(attacker, defenderWheelController);
            yield return WaitSpinning();
            yield return ApplyAfterHitEffect(attackerCard, defenderWheelController);
            yield return WaitSpinning();
            if (_actions == 3)
            {
                ChangeTurn();
                _acting = false;
                yield break;
            }

            attackerWheelController.UnlockWheel();
            _acting = false;
        }

        private IEnumerator WaitSpinning()
        {
            yield return new WaitUntil(() => !playerWheelController.IsSpinning);
            yield return new WaitUntil(() => !enemyWheelController.IsSpinning);
            yield return new WaitForSeconds(0.3f);
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
                    yield return defenderWheelController.RotateRight();
                if (ability == Ability.RotateLeft)
                    yield return defenderWheelController.RotateLeft();
            }
        }

        private IEnumerator ApplyFrontCardAttack(InPlayCard attackerCard, WheelController defenderWheelController)
        {
            if (attackerCard.GetCard().AttackType == AttackType.Front)
            {
                var defender = defenderWheelController.GetFrontCard();
                yield return ApplyDamage(attackerCard.Attack, defender, defenderWheelController, null);
            }
            else if (attackerCard.GetCard().AttackType == AttackType.All)
            {
                foreach (var defender in defenderWheelController.Cards)
                    yield return ApplyDamage(attackerCard.Attack, defender, defenderWheelController, null);
            }
            else if (attackerCard.GetCard().AttackType == AttackType.FrontAndSides)
            {
                var defenders = defenderWheelController.GetFrontNeighborsCards(0, 2).ToList();
                foreach (var defender in defenders)
                    yield return ApplyDamage(attackerCard.Attack, defender, defenderWheelController, null);
            }
        }

        private IEnumerator ApplyDamage(int damage, InPlayCard defender, WheelController defenderWheelController,
            Ability? source)
        {
            yield return WaitApplyDamage();

            _applyingDamage = true;

            Debug.Log($"Damage: <color=yellow>{defender.CardName}.{source}</color>");

            var defenderCard = defender.GetCard();
            defender.PlayGetHitAnimation(damage, source);
            defenderCard.Hp -= damage;
            if (defenderCard.Hp > 0)
            {
                Debug.Log($"Damage Alive: <color=red>{defender.CardName}</color>");
                _applyingDamage = false;
                yield break;
            }

            Debug.Log($"Damage Dead: <color=orange>{defender.CardName}</color>");
            defender.SetDead();

            yield return defenderWheelController.PutAliveUnitAtFront(true);
            yield return WaitSpinning();
            Debug.Log($"Damage Spin Complete: <color=orange>{defender.CardName}</color>");
            var deck = defenderWheelController == enemyWheelController ? _enemiesDeck : _playerBattleDeck;
            var discardPile = defenderWheelController == enemyWheelController
                ? new List<RunCard>()
                : _playerDiscardPile;
            if (deck.Count > 0)
            {
                var cards = DrawCards(1, ref deck, ref discardPile);
                if (cards == null || cards.Count == 0)
                    yield return EndBattle(defenderWheelController);
                else
                    yield return defender.SetCard(cards.First());
            }

            if (deck.Count == 0 && discardPile.Count == 0 && defenderWheelController.AllUnitsDead())
            {
                yield return EndBattle(defenderWheelController);
            }

            Debug.Log($"Damage Complete: <color=green>{defender.CardName}</color>");
            _applyingDamage = false;
        }

        private IEnumerator WaitApplyDamage()
        {
            yield return new WaitUntil(() => !_applyingDamage);
        }

        private IEnumerator EndBattle(WheelController defenderWheelController)
        {
            _acting = false;
            _applyingDamage = false;
            yield return new WaitForSeconds(.5f);
            BattleFinished?.Invoke(this, defenderWheelController == enemyWheelController);
        }

        private static void ApplyFrontCardEffect(WheelController attackerWheelController,
            WheelController defenderWheelController)
        {
            var attackerCard = attackerWheelController.GetFrontCard().GetCard();
            foreach (var ability in attackerCard.Abilities)
            {
                if (ability == Ability.Burn)
                {
                    var affectedCard = defenderWheelController.GetFrontCard();
                    if (!affectedCard.IsDead)
                        affectedCard.Effects.Add(Ability.Burn);
                }

                if (ability == Ability.BurnAll)
                {
                    foreach (var card in defenderWheelController.Cards)
                    {
                        if (!card.IsDead)
                            card.Effects.Add(Ability.Burn);
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
        }

        private IEnumerator ApplyWheelMovementEffect(WheelController wheelController)
        {
            foreach (var card in wheelController.Cards)
            {
                var cardActiveEffects = card.Effects;
                var burns = cardActiveEffects.Count(a => a == Ability.Burn);
                if (burns > 0)
                {
                    card.Effects.Remove(Ability.Burn);
                    yield return ApplyDamage(burns, card, wheelController, Ability.Burn);
                }
            }
        }

        private void ChangeTurn()
        {
            turn++;
            _actions = 0;

            if (IsPlayerTurn())
                StartPlayerTurn();
            else
                PlayBotTurn();
        }

        private bool IsPlayerTurn() => turn % 2 == 0;

        private void PlayBotTurn()
        {
            playerWheelController.LockWheel();
            enemyWheelController.UnlockWheel();
            StartCoroutine(BotAction());
        }

        private IEnumerator BotAction()
        {
            for (int i = 0; i < 3; i++)
            {
                yield return WaitSpinning();
                yield return WaitApplyDamage();
                while (_actions < i || _acting)
                {
                    yield return WaitSpinning();
                    yield return WaitApplyDamage();
                }

                yield return botControlWheel.TurnTowardsDirection(Random.Range(0, 2) == 1);
                yield return WaitSpinning();
                yield return WaitApplyDamage();
            }
        }

        private void StartPlayerTurn()
        {
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
                ChangeTurn();
        }

        [UsedImplicitly]
        public void Shuffle()
        {
            if (!IsPlayerTurn()) return;
            _actions++;
            StartCoroutine(ShuffleCoroutine());
            if (_actions == 3)
                ChangeTurn();
        }

        private IEnumerator ShuffleCoroutine()
        {
            var slots = playerWheelController.Cards.Count;
            var cards = DrawCards(slots, ref _playerBattleDeck,ref  _playerDiscardPile);
            var cardsInWheel = playerWheelController.Cards.Select(x => x.GetCard());
            _playerDiscardPile=_playerDiscardPile.Concat(cardsInWheel).ToList();
            for (int i = 0; i < slots; i++)
            {
                yield return playerWheelController.Cards[i].SetCard(cards[i]);
            }
        }
    }
}