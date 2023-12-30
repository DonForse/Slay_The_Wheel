using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Features.Battles.Core;
using Features.Cards.InPlay;
using UnityEngine;

namespace Features.Battles.Wheel
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private InPlayCard inPlayCardPrefab;
        [SerializeField] private WheelController wheelController;
        [HideInInspector] public List<InPlayCard> Cards => _slots.Select(x => x.GetCard()).ToList();
        public int frontCardIndex;
        private Func<IEnumerator> _wheelMovedCallback;
        private WheelRotation _lastActionDirection;
        private WheelRotation _lastRotation;
        public event EventHandler<InPlayCard> Acted;
        public event EventHandler<InPlayCard> WheelTurn;
        public event EventHandler<InPlayCard> ActiveCardChanged;

        private List<WheelSlot> _slots = new();
        
        public void InitializeWheel(bool player, int wheelSize, List<InPlayCardScriptableObject> cards)
        {
            // SetRunCards(player, cards);

            frontCardIndex = 0;
            
            _slots = wheelController.Initialize(wheelSize, cards);
            wheelController.RotatedLeft += OnTurnLeftAction;
            wheelController.RotatedRight += OnTurnRightAction;
            // input.SetTurnRightAction(OnTurnRightAction);
            // input.SetTurnLeftAction(OnTurnLeftAction);
            // wheelMovement.SetTurnLeftAction(OnTurnLeft);
            // wheelMovement.SetTurnRightAction(OnTurnRight);
            // input.Enable();
            // void SetRunCards(bool player, List<InPlayCardScriptableObject>cards)
            // {
            //     var amountToSet = Mathf.Min(wheelSize, cards.Count);
            //     for (int i = 0; i < amountToSet; i++)
            //     {
            //         var inPlayCard = Instantiate(inPlayCardPrefab, this.transform);
            //         Cards.Add(inPlayCard);
            //         inPlayCard.SetPlayer(player);
            //         inPlayCard.SetCard(cards[i], this);
            //     }
            // }
        }

        private void OnTurnRightAction(object sender, EventArgs e)
        {
            wheelController.LockPlayerInput();
            if (AllUnitsDead())
                return;

            _lastActionDirection = WheelRotation.Right; 
            _lastRotation = WheelRotation.Right;
            UpdateIndex(WheelRotation.Right);
            Acted?.Invoke(this, Cards[frontCardIndex]);
            
        }

        private void OnTurnLeftAction(object sender, EventArgs e)
        {
            wheelController.LockPlayerInput();
            if (AllUnitsDead())
                return;

            _lastActionDirection = WheelRotation.Left; 
            _lastRotation = WheelRotation.Left;
            UpdateIndex(WheelRotation.Left);
            Acted?.Invoke(this, Cards[frontCardIndex]);
        }

        public InPlayCard GetFrontCard() => Cards[frontCardIndex];
        
        public bool AllUnitsDead() => Cards.All(x => x.IsDead);

        public IEnumerator PutAliveUnitAtFront(WheelRotation direction)
        {
            while (Cards[frontCardIndex].IsDead)
            {
                if (AllUnitsDead())
                {
                    yield break;
                }
                yield return wheelController.TurnTowardsDirection(direction);
                UpdateIndex(direction);
                yield return new WaitForSeconds(0.1f);
            }
        }

        public IEnumerator RepeatRotate(int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return wheelController.TurnTowardsDirection(_lastRotation);
                UpdateIndex(_lastRotation);
            }
        }

        public IEnumerator Rotate(WheelRotation direction, int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return wheelController.TurnTowardsDirection(direction);
                UpdateIndex(direction);
            }

            yield return PutAliveUnitAtFront(direction);
        }

        public IEnumerator RevertLastMovement()
        {
            yield return wheelController.TurnTowardsDirection(_lastActionDirection == WheelRotation.Left ? WheelRotation.Right : WheelRotation.Left);
            UpdateIndex(_lastActionDirection.Inverse());
        }

        public IEnumerator RepeatActMove()
        {
            yield return wheelController.TurnTowardsDirection(_lastActionDirection);

            UpdateIndex(_lastActionDirection);
            Acted?.Invoke(this, Cards[frontCardIndex]);
        }

        public List<InPlayCard> GetNeighborsCards(InPlayCard executor, int startLevel, int finishLevel)
        {
            var cards = new List<InPlayCard>();
            var cardIndex = Cards.IndexOf(executor);
            for (int i = startLevel; i < finishLevel; i++)
            {
                var currentNegativeIndex = cardIndex - i;
                if (currentNegativeIndex < 0)
                    currentNegativeIndex += Cards.Count;
                var currentPositiveIndex = cardIndex + i;
                if (currentPositiveIndex > Cards.Count - 1)
                    currentPositiveIndex -= Cards.Count();

                cards.Add(Cards[currentNegativeIndex]);
                cards.Add(Cards[currentPositiveIndex]);
            }

            return cards.Distinct().ToList();
        }

        public IList<InPlayCard> GetFrontNeighborsCards(int startLevel, int finishLevel)
        {
            var cards = new List<InPlayCard>();
            for (int i = startLevel; i < finishLevel; i++)
            {
                var currentNegativeIndex = frontCardIndex - i;
                if (currentNegativeIndex < 0)
                    currentNegativeIndex += Cards.Count;
                var currentPositiveIndex = frontCardIndex + i;
                if (currentPositiveIndex > Cards.Count - 1)
                    currentPositiveIndex -= Cards.Count();

                cards.Add(Cards[currentNegativeIndex]);
                cards.Add(Cards[currentPositiveIndex]);
            }

            return cards.Distinct().ToList();
        }

        public IEnumerator ShowCards()
        {
            foreach (var card in Cards)
            {
                yield return card.PlayOnAppearFeedback();
            }
        }

        private void UpdateIndex(WheelRotation direction)
        {
            if (direction == WheelRotation.Left)
                IncrementFrontCardIndex();
            else
                DecrementFrontCardIndex();
        }
        
        private void DecrementFrontCardIndex()
        {
            frontCardIndex = (frontCardIndex - 1 + Cards.Count) % Cards.Count;
            ActiveCardChanged?.Invoke(this,Cards[frontCardIndex]);
            WheelTurn?.Invoke(this, Cards[frontCardIndex]);
        }

        private void IncrementFrontCardIndex()
        {
            frontCardIndex = (frontCardIndex + 1) % Cards.Count;
            ActiveCardChanged?.Invoke(this,Cards[frontCardIndex]);
            WheelTurn?.Invoke(this, Cards[frontCardIndex]);
        }

        public void LockInput()
        {
            wheelController.LockPlayerInput();
            //spells.block
            //cardsInHand.block
        }

        public void UnlockInput()
        {
            wheelController.UnlockPlayerInput();
            //spells.unlock
            //cardsInHand.unlock
        }
    }
}