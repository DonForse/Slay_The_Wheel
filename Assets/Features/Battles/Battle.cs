using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Features.Battles.Actions;
using Features.Battles.Core;
using Features.Battles.Core.Abilities;
using Features.Battles.Core.Attacks;
using Features.Battles.Spells;
using Features.Battles.Wheel;
using Features.Cards;
using Features.Cards.InPlay;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Features.Battles
{
    public class Battle : MonoBehaviour
    {
        [SerializeField] private PlayerController playerController;
        [SerializeField] private PlayerController enemyController;
        [SerializeField] private BotControlWheel botControlWheel;
        [SerializeField] private BusQueue.BusQueue _busQueue;
        [SerializeField] private TurnMessage turnMessage;
        [SerializeField] private ActionsView actionsView;

        [SerializeField] private SpellView[] spellViews;

        // [SerializeField] private BotPlayer botPlayer;
        // public DamageSystem damageSystem;
        private int _actions;
        private int turn = 0;
        private List<InPlayCardScriptableObject> _playerBattleDeck;
        private List<InPlayCardScriptableObject> _playerDiscardPile;
        private List<InPlayCardScriptableObject> _enemiesDeck;
        private InPlayCardScriptableObject _heroCardScriptableObject;
        private bool acting;
        public event EventHandler<bool> BattleFinished;
        public Turn Turn => IsPlayerTurn() ? Turn.Player : Turn.Enemy;
        public int Actions => _actions;

        private List<IOnApplyAbilityStrategy> _applyAbilityStrategies = new();
        private List<IAttackStrategy> _attackStrategies = new();

        public IEnumerator Initialize(List<RunCardScriptableObject> deck, List<RunCardScriptableObject> enemies, int playerWheelSize,
            int enemyWheelSize, RunCardScriptableObject heroCardScriptableObject)
        {
            _applyAbilityStrategies = new()
            {
                new AddBurnOnApplyAbilityStrategy(),
                new RotateLeftOnApplyAbilityStrategy(),
                new RotateRightOnApplyAbilityStrategy(),
                new DealAttackDamageOnApplyAbilityStrategy(this), 
                new MultiAttackOnApplyAbilityStrategy(this),
                new GainShieldOnApplyAbilityStrategy(),
                new GainAttackOnApplyAbilityStrategy()
            };
            _attackStrategies = new()
            {
                new FrontAttackStrategy(this),
                new FrontAndSidesAttackStrategy(this),
                new AllAttackStrategy(this)
            };

            _playerBattleDeck = deck.Select(card=> new InPlayCardScriptableObject(card)).ToList();
            _playerDiscardPile = new();
            _enemiesDeck = enemies.Skip(enemyWheelSize).Select(card =>new InPlayCardScriptableObject(card)).ToList();
            _heroCardScriptableObject = new InPlayCardScriptableObject(heroCardScriptableObject);
            var enemiesTemp = enemies.Select(card => new InPlayCardScriptableObject(card)).ToList();
            
            var cards = DrawCards(playerWheelSize - 1, ref _playerBattleDeck, ref _playerDiscardPile);
            cards = new List<InPlayCardScriptableObject>() { _heroCardScriptableObject }.Concat(cards).ToList();
            
            StartCoroutine(enemyController.InitializeWheel(false, enemyWheelSize, enemiesTemp));
            yield return playerController.InitializeWheel(true, playerWheelSize, cards);

            enemyController.LockWheel();
            playerController.UnlockWheel();

            playerController.Acted += OnPlayerActed;
            enemyController.Acted += OnEnemyActed;
            playerController.SetWheelMovedCallback(OnPlayerWheelMoved);
            enemyController.SetWheelMovedCallback(OnEnemyWheelMoved);

            SetActions(3);
        }

        private void OnDestroy()
        {
            playerController.Acted -= OnPlayerActed;
            enemyController.Acted -= OnEnemyActed;
        }

        private void OnEnemyActed(object sender, InPlayCard attacker)
        {
            var attackerCard = attacker.GetCard();
            var actCost = attackerCard.ActCost;

            if (_actions > 0 && actCost > _actions)
            {
                _busQueue.EnqueueAction(RevertAct(enemyController));
                return;
            }

            _busQueue.EnqueueAction(Act(attacker,
                enemyController,
                playerController, ActDirection.Right));
        }

        private void OnPlayerActed(object sender, InPlayCard attacker)
        {
            var attackerCard = attacker.GetCard();
            var actCost = attackerCard.ActCost;

            if (_actions > 0 && actCost > _actions)
            {
                _busQueue.EnqueueAction(RevertAct(playerController));
                return;
            }

            _busQueue.EnqueueAction(Act(attacker,
                playerController,
                enemyController, ActDirection.Right));
        }

        private IEnumerator RevertAct(PlayerController playerController)
        {
            _busQueue.EnqueueAction(ActStartCoroutine(playerController));
            _busQueue.EnqueueAction(RevertWheelPosition(playerController));
            _busQueue.EnqueueAction(ActEndCoroutine(playerController));
            yield break;
        }

        private IEnumerator ProcessAct(InPlayCard attacker, PlayerController attackerPlayerController,
            PlayerController defenderPlayerController, ActDirection fromRight)
        {
            if (attacker.IsDead)
            {
                _busQueue.EnqueueAction(attackerPlayerController.PutAliveUnitAtFront(fromRight));
                _busQueue.EnqueueAction(ProcessAttackerDiedActing(attackerPlayerController));
                yield break;
            }

            _busQueue.EnqueueAction(ApplyOnActAbilitiesEffects(attackerPlayerController, defenderPlayerController));
            _busQueue.EnqueueAction(ApplyFrontCardAttack(attacker, defenderPlayerController));
            if (CompletedActions())
            {
                _busQueue.EnqueueAction(ChangeTurn());
                yield break;
            }

            _busQueue.EnqueueAction(ActEndCoroutine(attackerPlayerController));
        }


        private IEnumerator ActEndCoroutine(PlayerController attackerPlayerController)
        {
            acting = false;
            Debug.Log($"<color=green>{"ACT-END"}</color>");
            attackerPlayerController.UnlockWheel();
            yield return null;
        }

        private List<InPlayCardScriptableObject> DrawCards(int amountToDraw, ref List<InPlayCardScriptableObject> deck, ref List<InPlayCardScriptableObject> discardPile)
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

        private IEnumerator ActStartCoroutine(PlayerController attackerPlayerController)
        {
            acting = true;
            attackerPlayerController.LockWheel();
            yield return null;
        }

        private IEnumerator ProcessAttackerDiedActing(PlayerController attackerPlayerController)
        {
            yield return attackerPlayerController.PutAliveUnitAtFront(ActDirection.Right);

            if (attackerPlayerController.AllUnitsDead())
                yield return EndBattle(attackerPlayerController);
            if (CompletedActions())
            {
                _busQueue.EnqueueAction(ActEndCoroutine(attackerPlayerController));
                _busQueue.EnqueueAction(ChangeTurn());
                yield break; // yield break;
            }

            yield break;
        }

        private IEnumerator Act(InPlayCard attacker, PlayerController attackerPlayerController,
            PlayerController defenderPlayerController, ActDirection actDirection)
        {
            var attackerCard = attacker.GetCard();
            _busQueue.EnqueueAction(ActStartCoroutine(attackerPlayerController));
            SetActions(_actions - (attackerCard.IsDead ? 1 : attackerCard.ActCost));

            if (attackerCard.IsDead)
            {
                _busQueue.EnqueueAction(attackerPlayerController.RepeatActMove());
                yield break;
            }

            _busQueue.EnqueueAction(ApplyWheelSpinEffects(attackerPlayerController));
            _busQueue.EnqueueAction(
                ProcessAct(attacker, attackerPlayerController, defenderPlayerController, actDirection));
            yield break;
        }

        public IEnumerator ApplyFrontCardAttack(InPlayCard attackerCard, PlayerController defender)
        {
            foreach (var attackStrategy in _attackStrategies)
            {
                if (attackStrategy.IsValid(attackerCard.GetCard().AttackType))
                    yield return attackStrategy.Execute(attackerCard, defender);
            }

            yield return ApplyOnAttackAbilitiesEffects(attackerCard.OwnerPlayer, defender);
            yield return defender.PutAliveUnitAtFront(ActDirection.Right);
        }

        public IEnumerator ApplyDamage(int damage, InPlayCard damageReceiver, [CanBeNull] InPlayCard damageDealer,
            PlayerController defenderPlayerController,
            AbilityEnum? source)
        {
            var defenderCard = damageReceiver.GetCard();
            StartCoroutine(damageReceiver.PlayGetHitAnimation(damage, source));
            var difDamage = Mathf.Max(damage - damageReceiver.GetCard().Armor, 0);

            damageReceiver.GetCard().Armor = Mathf.Max(0, damageReceiver.GetCard().Armor - damage);
            if (difDamage > 0)
            {
                if (damageDealer != null)
                    yield return ApplyOnDealDamageAbilities(damageDealer, damageReceiver);
                defenderCard.Health -= damage;
                //apply receive damage.
            }

            if (defenderCard.Health > 0)
                yield break;

            yield return damageReceiver.SetDead();

            if (defenderPlayerController == enemyController)
            {
                if (_enemiesDeck.Count > 0)
                {
                    var runCards = new List<InPlayCardScriptableObject>();
                    var cards = DrawCards(1, ref _enemiesDeck, ref runCards);
                    if (cards == null || cards.Count == 0)
                        _busQueue.EnqueueInterruptAction(EndBattle(defenderPlayerController));
                    else
                        _busQueue.EnqueueInterruptAction(
                            damageReceiver.SetCard(cards.First(), defenderPlayerController));
                }

                if (_enemiesDeck.Count == 0 && defenderPlayerController.AllUnitsDead())
                {
                    _busQueue.EnqueueInterruptAction(EndBattle(defenderPlayerController));
                }
            }
            else
            {
                if (_heroCardScriptableObject.IsDead)
                {
                    _busQueue.EnqueueInterruptAction(EndBattle(defenderPlayerController));
                    yield break;
                    ;
                }

                if (_playerBattleDeck.Count > 0)
                {
                    var cards = DrawCards(1, ref _playerBattleDeck, ref _playerDiscardPile);
                    if (cards == null || cards.Count == 0)
                        _busQueue.EnqueueInterruptAction(EndBattle(defenderPlayerController));
                    else
                        _busQueue.EnqueueInterruptAction(
                            damageReceiver.SetCard(cards.First(), defenderPlayerController));
                }

                if (_playerBattleDeck.Count == 0 && _playerDiscardPile.Count == 0 &&
                    defenderPlayerController.AllUnitsDead())
                {
                    _busQueue.EnqueueInterruptAction(EndBattle(defenderPlayerController));
                }
            }
        }

        private IEnumerator EndBattle(PlayerController defenderPlayerController)
        {
            _busQueue.Clear();
            yield return new WaitForSeconds(.5f);
            BattleFinished?.Invoke(this, defenderPlayerController == enemyController);
        }

        private IEnumerator RevertWheelPosition(PlayerController playerController)
        {
            yield return playerController.RevertLastMovement();
        }

        private IEnumerator ChangeTurn()
        {
            var playerTurn = IsPlayerTurn();
            yield return ApplyEndTurnAbilities(playerTurn ? playerController : enemyController,
                playerTurn ? enemyController : playerController);
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


        private IEnumerator StartPlayerTurn()
        {
            yield return ApplyStartTurnAbilities(playerController, enemyController);
            foreach (var spell in spellViews)
            {
                spell.ReduceCooldown();
            }

            yield return turnMessage.Show(true);
            enemyController.LockWheel();
            playerController.UnlockWheel();
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

        private IEnumerator OnEnemyWheelMoved()
        {
            Debug.Log($"<color=yellow>{"OnEnemyWheelMoved"}</color>");
            yield return ApplyWheelSpinEffects(enemyController);
        }

        private IEnumerator OnPlayerWheelMoved()
        {
            Debug.Log($"<color=yellow>{"OnPlayerWheelMoved"}</color>");
            yield return ApplyWheelSpinEffects(playerController);
        }

        private IEnumerator PlayBotTurn()
        {
            yield return ApplyStartTurnAbilities(enemyController, playerController);
            yield return turnMessage.Show(false);
            playerController.LockWheel();
            enemyController.UnlockWheel();
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
            if (_actions <= 0)
                yield break;
            yield return botControlWheel.TurnTowardsDirection(Random.Range(0, 2) == 1);
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
                foreach (var card in enemyController.Cards)
                {
                    card.UpdateEffect(EffectEnum.Fire, 3);
                }
            }
            else if (skillIndex == 2)
            {
                foreach (var card in enemyController.Cards)
                {
                    var totalBurns = card.Effects.Count(x => x.Type == EffectEnum.Fire);
                    card.UpdateEffect(EffectEnum.Fire, totalBurns);
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
            playerController.LockWheel();
            var burns = enemyController.Cards.Max(x => x.Effects.Count(x => x.Type == EffectEnum.Fire));
            while (burns > 0 && !enemyController.AllUnitsDead())
            {
                if (enemyController.AllUnitsDead()) yield break;
                yield return enemyController.Rotate(ActDirection.Right, burns);
                burns = enemyController.Cards.Max(x =>
                    x.Effects.Count(ability => ability.Type == EffectEnum.Fire));
            }

            playerController.UnlockWheel();
            acting = false;
        }

        private IEnumerator ShuffleCoroutine()
        {
            acting = true;
            var slots = playerController.Cards.Count;
            var cards = DrawCards(slots - 1, ref _playerBattleDeck, ref _playerDiscardPile);
            var cardsInWheel = playerController.Cards.Select(x => x.GetCard()).ToList();
            cardsInWheel.Remove(_heroCardScriptableObject);
            cards = new List<InPlayCardScriptableObject>() { _heroCardScriptableObject }.Concat(cards).ToList();
            _playerDiscardPile = _playerDiscardPile.Concat(cardsInWheel).ToList();
            for (int i = 0; i < slots; i++)
            {
                yield return playerController.Cards[i].SetCard(cards[i], playerController);
            }

            acting = false;
        }

        private IEnumerator ApplyWheelSpinEffects(PlayerController controller)
        {
            foreach (var card in controller.Cards)
            {
                var cardActiveEffects = card.Effects;
                var burns = cardActiveEffects.FirstOrDefault(a => a.Type == EffectEnum.Fire);
                if (burns != null)
                {
                    card.UpdateEffect(EffectEnum.Fire, -1);
                    yield return ApplyDamage(burns.Amount, card, null, controller, AbilityEnum.Burn);
                }
            }

            yield break;
        }

        private IEnumerator ApplyEndTurnAbilities(PlayerController attacker, PlayerController defender)
        {
            foreach (var card in attacker.Cards)
            {
                foreach (var ability in card.GetCard().OnTurnEndAbilities)
                {
                    foreach (var strategy in _applyAbilityStrategies)
                    {
                        if (strategy.IsValid(ability.Type))
                            yield return strategy.Execute(ability, card,  defender, attacker);
                    }
                }
            }
        }

        private IEnumerator ApplyStartTurnAbilities(PlayerController attacker, PlayerController defender)
        {
            foreach (var card in attacker.Cards)
            {
                foreach (var ability in card.GetCard().OnTurnStartAbilities)
                {
                    foreach (var strategy in _applyAbilityStrategies)
                    {
                        if (strategy.IsValid(ability.Type))
                            yield return strategy.Execute(ability, card,  defender, attacker);
                    }
                }
            }
        }

        private IEnumerator ApplyOnDealDamageAbilities(InPlayCard damageDealerCard, InPlayCard damageReceiverCard)
        {
            var damageDealerRunCard = damageDealerCard.GetCard();
            foreach (var ability in damageDealerRunCard.OnDealDamageAbilities)
            {
                foreach (var strategy in _applyAbilityStrategies)
                {
                    if (strategy.IsValid(ability.Type))
                        yield return strategy.Execute(ability, damageDealerCard,  damageReceiverCard.OwnerPlayer,
                            damageDealerCard.OwnerPlayer);
                }
            }

            yield return null;
        }

        private IEnumerator ApplyOnActAbilitiesEffects(PlayerController attackerPlayerController,
            PlayerController defenderPlayerController)
        {
            foreach (var card in attackerPlayerController.Cards)
            {
                foreach (var ability in card.GetCard().OnActAbilities)
                {
                    foreach (var strategy in _applyAbilityStrategies)
                    {
                        if (strategy.IsValid(ability.Type))
                            yield return strategy.Execute(ability, card, 
                                defenderPlayerController, attackerPlayerController);
                    }
                }
            }

            yield return null;
        }

        private IEnumerator ApplyOnAttackAbilitiesEffects(PlayerController attackerPlayerController,
            PlayerController defenderPlayerController)
        {
            var attackerCard = attackerPlayerController.GetFrontCard();
            var attackerRunCard = attackerPlayerController.GetFrontCard().GetCard();
            foreach (var ability in attackerRunCard.OnAttackAbilities)
            {
                foreach (var strategy in _applyAbilityStrategies)
                {
                    if (strategy.IsValid(ability.Type))
                        yield return strategy.Execute(ability, attackerCard, 
                            defenderPlayerController, attackerPlayerController);
                }
            }

            yield return null;
        }
    }
}