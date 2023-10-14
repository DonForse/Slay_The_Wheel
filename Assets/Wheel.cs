using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    public float Radius;
    [HideInInspector] public int Size;
    public InPlayCard inPlayCardPrefab;
    [HideInInspector] public List<InPlayCard> Cards = new();
    [HideInInspector] public List<Vector2> Positions;
    private IControlWheel input;

    private List<RunCard> _cardsToAdd;
    private int frontCardIndex;
    private bool isPlayer;
    public event EventHandler<InPlayCard> Acted;
    private void Awake() => input = GetComponent<IControlWheel>();

    public void InitializeWheel(bool player)
    {
        Positions = new();

        CalculatePositions();
        SetRunCards(player);

        frontCardIndex = 0;
        input.TurnRight += OnTurnRight;
        input.TurnLeft += OnTurnLeft;
        input.Enable();
        isPlayer = player;
    }

    public void LockWheel() => input.Disable();

    public void UnlockWheel() => input.Enable();

    public InPlayCard GetFrontCard() => Cards[frontCardIndex];

    public void SetSize(int value)
    {
        Size = value;
        AddInPlayCardsToWheel();
    }

    public void SetCards(List<RunCard> deck) => _cardsToAdd = deck;

    public bool AllUnitsDead()
    {
        return Cards.All(x => x.IsDead);
    }

    public IEnumerator PutAliveUnitAtFront()
    {
        if (Cards.All(x => x.IsDead))
            yield break;
        
        while (Cards[frontCardIndex].IsDead)
        {
            DecrementFrontCardIndex();
            yield return StartCoroutine(input.TurnLeftWithoutNotifying());
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

    private void AddInPlayCardsToWheel()
    {
        for (var i = 0; i < Size; i++)
        {
            // Create and position the object
            var go = Instantiate(inPlayCardPrefab, this.transform);
            Cards.Add(go);
        }
    }

    private void OnTurnLeft(object sender, EventArgs e)
    {
        Debug.Log("TurnLeft");
        IncrementFrontCardIndex();

        Acted?.Invoke(this, Cards[frontCardIndex]);
    }

    private void OnTurnRight(object sender, EventArgs e)
    {
        Debug.Log("TurnRight");
        DecrementFrontCardIndex();

        Acted?.Invoke(this, Cards[frontCardIndex]);
    }

    private void SetRunCards(bool player)
    {
        var amountToSet = Mathf.Min(Size, _cardsToAdd.Count);
        for (int i = 0; i < amountToSet; i++)
        {
            Cards[i].SetCard(_cardsToAdd[i], player);
            Cards[i].transform.localPosition = Positions[i];
        }

        _cardsToAdd = _cardsToAdd.Skip(amountToSet).ToList();
    }

    private void CalculatePositions()
    {
        var angleOffset = Mathf.PI / 2; // Offset to start at the top of the circle

        for (var i = 0; i < Size; i++)
        {
            // Calculate spherical coordinates with angle offset
            var theta = 2 * Mathf.PI * i / Size + angleOffset;
            var x = Radius * Mathf.Cos(theta);
            var y = Radius * Mathf.Sin(theta);

            var position = new Vector2(x, y);
            Positions.Add(position);
        }
    }
}