using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Features.Cards;
using UnityEngine;

namespace Features.Battles
{
    public class DebugBattle : MonoBehaviour
    {
        [SerializeField] private DeckConfigurationScriptableObject _deckConfigurationScriptableObject;
        [SerializeField] private BaseCardsScriptableObject enemiesDb;

        [SerializeField] private Battle battle;

        private List<RunCardScriptableObject> _deck;
        private RunCardScriptableObject _heroCardScriptableObject;

        // Start is called before the first frame update
        IEnumerator Start()
        {
        
            _deck = new List<RunCardScriptableObject>();
            var heroCardDb = _deckConfigurationScriptableObject.hero;
            _heroCardScriptableObject = new RunCardScriptableObject(heroCardDb);
            foreach (var card in _deckConfigurationScriptableObject.cards)
            {
                for (int i = 0; i < card.Amount; i++)
                    _deck.Add(new RunCardScriptableObject(card.card));
            }

            var slime = enemiesDb.cards.FirstOrDefault(x => x.cardName.Contains("Slime"));
            _deck = _deck.OrderBy(x=>Random.Range(0, 100)).ToList();
            yield return battle.Initialize(_deck,
                new List<RunCardScriptableObject>() { new RunCardScriptableObject(slime), new RunCardScriptableObject(slime), new RunCardScriptableObject(slime) }, 5, 3, _heroCardScriptableObject);
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
