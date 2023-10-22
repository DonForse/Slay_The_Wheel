using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Features.Cards;
using UnityEngine;

namespace Features.Battles.Wheel
{
    public class WheelController : MonoBehaviour
    {
        public WheelData WheelData = new();

        public InPlayCard inPlayCardPrefab;
        [HideInInspector] public List<InPlayCard> Cards = new();
        [HideInInspector] public List<Vector2> Positions;
        private IControlWheel input;
        private AutomaticControlWheel wheelMovement;

        // private List<RunCard> _cardsToAdd;
        private int frontCardIndex;
        public event EventHandler<InPlayCard> Acted;
        public event EventHandler<InPlayCard> WheelTurn;

        private void Awake()
        {
            input = GetComponent<IControlWheel>();
            wheelMovement = GetComponent<AutomaticControlWheel>();
        }

        public IEnumerator InitializeWheel(bool player, int enemyWheelSize, List<RunCard> cards)
        {
            Positions = new();
            SetSize(enemyWheelSize);
            CalculatePositions();
            yield return SetRunCards(player, cards);

            frontCardIndex = 0;
            input.SetTurnRightAction(OnTurnRightAction);
            input.SetTurnLeftAction(OnTurnLeftAction);

            // input.TurnRight += OnTurnRightAction;
            // input.TurnLeft += OnTurnLeftAction;
            input.Enable();
            wheelMovement.TurnRight += OnTurnRight;
            wheelMovement.TurnLeft += OnTurnLeft;
        }

        public void LockWheel() => input.Disable();

        public void UnlockWheel() => input.Enable();

        public InPlayCard GetFrontCard() => Cards[frontCardIndex];

        private void SetSize(int value)
        {
            WheelData.Size = value;
        }
    
        public bool AllUnitsDead() => Cards.All(x => x.IsDead);

        public IEnumerator PutAliveUnitAtFront(bool toTheRight)
        {
            while (Cards[frontCardIndex].IsDead)
            {
                if (AllUnitsDead())
                {
                    yield break;
                }
                yield return wheelMovement.TurnTowardsDirection(toTheRight);
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
            IncrementFrontCardIndex();
            yield return PutAliveUnitAtFront(false);
            Acted?.Invoke(this, Cards[frontCardIndex]);
        }

        private IEnumerator OnTurnRightAction()
        {
            if (AllUnitsDead())
                yield break;
            DecrementFrontCardIndex();
            yield return PutAliveUnitAtFront(true);
            Acted?.Invoke(this, Cards[frontCardIndex]);
        }

        private void OnTurnLeft(object sender, EventArgs e)
        {
            if (AllUnitsDead())
                return;
            IncrementFrontCardIndex();
            WheelTurn?.Invoke(this, Cards[frontCardIndex]);
        }

        private void OnTurnRight(object sender, EventArgs e)
        {
            if (AllUnitsDead())
                return;
            DecrementFrontCardIndex();
            WheelTurn?.Invoke(this, Cards[frontCardIndex]);
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
                yield return inPlayCard.SetCard(cards[i]);
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

        public IEnumerator RotateRight()
        {
            yield return wheelMovement.TurnTowardsDirection(true);
            yield return PutAliveUnitAtFront(true);
            yield return new WaitForSeconds(0.1f);
        }

        public IEnumerator RotateLeft()
        {
            yield return wheelMovement.TurnTowardsDirection(false);
            yield return PutAliveUnitAtFront(false);
            yield return new WaitForSeconds(0.1f);
        }
    }
}