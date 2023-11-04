using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Features.Battles;
using Features.Cards;
using UnityEngine;

public class DebugBattle : MonoBehaviour
{
    [SerializeField] private BaseCardsScriptableObject heroesDb;
    [SerializeField] private BaseCardsScriptableObject unitsDb;
    [SerializeField] private BaseCardsScriptableObject enemiesDb;

    [SerializeField] private Battle battle;

    private List<RunCard> _deck;
    private RunCard _heroCard;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        
        _deck = new List<RunCard>();
        var heroCardDb = heroesDb.cards.FirstOrDefault(x => x.cardName.Contains("Kael Fireforge"));
        _heroCard = new RunCard(heroCardDb);
        var recruits = unitsDb.cards.FirstOrDefault(x => x.cardName.Contains("Fireborn Recruit"));
        var warrior = unitsDb.cards.FirstOrDefault(x => x.cardName.Contains("Fireborn Warrior"));
        var sorceress = unitsDb.cards.FirstOrDefault(x => x.cardName.Contains("Firestorm Sorceress"));
        var slime = enemiesDb.cards.FirstOrDefault(x => x.cardName.Contains("Slime"));
        
        for (var i = 0; i < 12; i++)
        {
            var playerUnit = new RunCard(recruits);
            _deck.Add(playerUnit);
        }

        for (var i = 0; i < 3; i++)
        {
            var playerUnit = new RunCard(warrior);
            _deck.Add(playerUnit);
        }

        for (var i = 0; i < 2; i++)
        {
            var playerUnit = new RunCard(sorceress);
            _deck.Add(playerUnit);
        }

        yield return battle.Initialize(_deck,
            new List<RunCard>() { new RunCard(slime), new RunCard(slime), new RunCard(slime) }, 5, 3, _heroCard);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
