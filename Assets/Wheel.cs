using System;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    public float Radius;
    [HideInInspector]public int Size;
    public InPlayCard inPlayCardPrefab;
    [HideInInspector]public List<InPlayCard> Cards = new();
    [HideInInspector]public List<Vector2> Positions = new();
    private IControlWheel input;


    private List<RunCard> _cardsToAdd;
    private int frontCardIndex;
    public event EventHandler<InPlayCard> Acted;
    private void Awake() => input = GetComponent<IControlWheel>();

    public void InitializeWheel(bool player)
    {
        float angleOffset = Mathf.PI / 2; // Offset to start at the top of the circle

        for (var i = 0; i < Size; i++)
        {
            // Calculate spherical coordinates with angle offset
            var theta = 2 * Mathf.PI * i / Size + angleOffset;
            var x = Radius * Mathf.Cos(theta);
            var y = Radius * Mathf.Sin(theta);

            var position = new Vector2(x, y);
            Positions.Add(position);

            // Create and position the object
            var go = Instantiate(inPlayCardPrefab, this.transform);
            go.SetCard(_cardsToAdd[i], player);
            go.transform.localPosition = position;
            Cards.Add(go);
        }

        frontCardIndex = 0;
        input.TurnRight += OnTurnRight;
        input.TurnLeft += OnTurnLeft;
        input.Enable();
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

    public void LockWheel()
    {
        input.Disable();
    }

    public void UnlockWheel()
    {
        input.Enable();
    }

    public InPlayCard GetFrontCard() => Cards[frontCardIndex];

    public void SetSize(int value)
    {
        Size = value;
    }

    public void SetCards(List<RunCard> deck)
    {
        _cardsToAdd = deck;
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
}
