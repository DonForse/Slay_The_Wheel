using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Features.Cards;
using UnityEngine;
using UnityEngine.Serialization;

namespace Features.Battles.Wheel
{
    public class PlayerController : MonoBehaviour
    {
        public WheelData WheelData = new();
        public InPlayCard inPlayCardPrefab;
        [HideInInspector] public List<InPlayCard> Cards = new();
        [HideInInspector] public List<Vector2> Positions;
        private IControlWheel input;
        private ControlWheel[] wheelControllers;
        private AutomaticControlWheel wheelMovement;
        [SerializeField] Animator animator;

        // private List<RunCard> _cardsToAdd;
        private int frontCardIndex;
        private Func<IEnumerator> _wheelMovedCallback;
        private ActDirection _lastActionDirection;
        private static readonly int OnMoved = Animator.StringToHash("on_moved");
        public event EventHandler<InPlayCard> Acted;
        public event EventHandler<InPlayCard> WheelTurn;

        private void Awake()
        {
            input = GetComponent<IControlWheel>();
            wheelControllers = GetComponents<ControlWheel>();
            wheelMovement = GetComponent<AutomaticControlWheel>();
        }

        public IEnumerator InitializeWheel(bool player, int wheelSize, List<RunCard> cards)
        {
            Positions = new();
            WheelData.Size = wheelSize;
            CalculatePositions();
            yield return SetRunCards(player, cards);

            frontCardIndex = 0;
            input.SetTurnRightAction(OnTurnRightAction);
            input.SetTurnLeftAction(OnTurnLeftAction);
            wheelMovement.SetTurnLeftAction(OnTurnLeft);
            wheelMovement.SetTurnRightAction(OnTurnRight);
            foreach (var controller in wheelControllers)
            {
                controller.SetOnBeforeRotation(OnBeforeRotation);
            }
            input.Enable();
        }

        public void LockWheel() => input.Disable();

        public void UnlockWheel() => input.Enable();

        public InPlayCard GetFrontCard() => Cards[frontCardIndex];
        
        public bool AllUnitsDead() => Cards.All(x => x.IsDead);

        public IEnumerator PutAliveUnitAtFront(ActDirection toTheRight)
        {
            while (Cards[frontCardIndex].IsDead)
            {
                if (AllUnitsDead())
                {
                    yield break;
                }
                yield return wheelMovement.TurnTowardsDirection(toTheRight, true);
                yield return new WaitForSeconds(0.1f);
            }
        }

        private void DecrementFrontCardIndex()
        {
            frontCardIndex--;
            if (frontCardIndex < 0)
                frontCardIndex = Cards.Count - 1;
        }

        private void IncrementFrontCardIndex()
        {
            frontCardIndex++;
            if (frontCardIndex > Cards.Count - 1)
                frontCardIndex = 0;
        }

        private IEnumerator OnTurnLeftAction()
        {
            if (AllUnitsDead())
                yield break;
            _lastActionDirection = ActDirection.Left;
            IncrementFrontCardIndex();
            animator.SetTrigger(OnMoved);
            yield return new WaitForSeconds(0.3f);
            Acted?.Invoke(this, Cards[frontCardIndex]);
        }

        private IEnumerator OnTurnRightAction()
        {
            if (AllUnitsDead())
                yield break;

            _lastActionDirection = ActDirection.Right; 
            
            DecrementFrontCardIndex();
            animator.SetTrigger(OnMoved);
            yield return new WaitForSeconds(0.3f);
            Acted?.Invoke(this, Cards[frontCardIndex]);
        }

        private IEnumerator OnTurnLeft()
        {
            if (AllUnitsDead())
                yield break;
            
            IncrementFrontCardIndex();
            yield return _wheelMovedCallback.Invoke();
        }

        private IEnumerator OnTurnRight()
        {
            if (AllUnitsDead())
                yield break;
            DecrementFrontCardIndex();
            yield return _wheelMovedCallback.Invoke();
        }
        
        private void OnBeforeRotation(TurningOrientation turningOrientation)
        {
            animator.SetTrigger($"Rotate_{turningOrientation}");
        }

        private IEnumerator SetRunCards(bool player, List<RunCard>cards)
        {
            var amountToSet = Mathf.Min(WheelData.Size, cards.Count);
            for (int i = 0; i < amountToSet; i++)
            {
                var inPlayCard = Instantiate(inPlayCardPrefab, this.transform);
                Cards.Add(inPlayCard);
                inPlayCard.SetPlayer(player);
                inPlayCard.transform.localPosition = Positions[i];
                yield return inPlayCard.SetCard(cards[i], this);
            }
        }

        private void CalculatePositions()
        {
            var angleOffset = Mathf.PI / 2; // Offset to start at the top of the circle

            for (var i = 0; i < WheelData.Size; i++)
            {
                // Calculate spherical coordinates with angle offset
                var theta = 2 * Mathf.PI * i / WheelData.Size + angleOffset;
                var x = WheelData.Radius * Mathf.Cos(theta);
                var y = WheelData.Radius * Mathf.Sin(theta);

                var position = new Vector2(x, y);
                Positions.Add(position);
            }
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

        public IEnumerator Rotate(ActDirection direction, int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return wheelMovement.TurnTowardsDirection(direction, true);
            }

            yield return PutAliveUnitAtFront(direction);
        }

        public void SetWheelMovedCallback(Func<IEnumerator> onPlayerWheelMoved)
        {
            _wheelMovedCallback = onPlayerWheelMoved;
        }

        public IEnumerator RevertLastMovement()
        {
            yield return wheelMovement.TurnTowardsDirection(_lastActionDirection == ActDirection.Left ? ActDirection.Right : ActDirection.Left, false);
            if (_lastActionDirection == ActDirection.Left)
                DecrementFrontCardIndex();
            else
                IncrementFrontCardIndex();
        }

        public IEnumerator RepeatActMove()
        {
            yield return wheelMovement.TurnTowardsDirection(_lastActionDirection, true);

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
    }
}