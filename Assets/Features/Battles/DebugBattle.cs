using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Features.Battles;
using Features.Cards;
using UnityEngine;

public class DebugBattle : MonoBehaviour
{
    [SerializeField] private DeckConfigurationScriptableObject _deckConfigurationScriptableObject;
    [SerializeField] private BaseCardsScriptableObject enemiesDb;

    [SerializeField] private Battle battle;

    private List<RunCard> _deck;
    private RunCard _heroCard;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        
        _deck = new List<RunCard>();
        var heroCardDb = _deckConfigurationScriptableObject.hero;
        _heroCard = new RunCard(heroCardDb);
        foreach (var card in _deckConfigurationScriptableObject.cards)
        {
            for (int i = 0; i < card.Amount; i++)
                _deck.Add(new RunCard(card.card));
        }

        var slime = enemiesDb.cards.FirstOrDefault(x => x.cardName.Contains("Slime"));
        _deck = _deck.OrderBy(x=>Random.Range(0, 100)).ToList();
        yield return battle.Initialize(_deck,
            new List<RunCard>() { new RunCard(slime), new RunCard(slime), new RunCard(slime) }, 5, 3, _heroCard);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
