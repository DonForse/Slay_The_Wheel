using System.Linq;
using Features.Cards;
using Features.Common;
using UnityEngine;
using UnityPackages.Slay_The_Spire_Map.Scripts;

namespace Features.Maps.Shops
{
    public class ShopNode : MonoBehaviour
    {
        [SerializeField] private int amountOfCards;
        [SerializeField] private int amountOfRelics;
        [SerializeField] private ShopItemView shopCardPrefab;
        [SerializeField] private Transform cardsContainer;
        [SerializeField] private Transform relicsContainer;
        [SerializeField] private ShopItemView shopRelicPrefab;
        [SerializeField] private RelicsScriptableObject relicsScriptableObject;
        [SerializeField] private BaseCardsScriptableObject cardsScriptableObject;

        public void Show()
        {
            var cardsToAdd = cardsScriptableObject.cards.ToList();
            var playerRelics = Provider.PlayerPrefsRelicsRepository().Get();
            var goodRelics = relicsScriptableObject.relics.Where(relic =>
                    relic.Spectrum == RelicSpectrumType.Good
                    && !playerRelics.Contains(relic.id))
                .OrderBy(x => UnityEngine.Random.Range(0, 100))
                .ToList();
            for (int i = 0; i < amountOfCards; i++)
            {
                var randomId = Random.Range(0, cardsToAdd.Count);
                var card = cardsToAdd[randomId];
                cardsToAdd.Remove(card);
                var go = Instantiate(shopCardPrefab, cardsContainer);
                
                go.Set(card.cardName, card.cardSprite, card.goldCost);
            }
            
            for (int i = 0; i < amountOfRelics; i++)
            {
                var randomId = Random.Range(0, goodRelics.Count);
                var relic = goodRelics[randomId];
                goodRelics.Remove(relic);
                var go= Instantiate(shopRelicPrefab, relicsContainer);
                
                go.Set(relic.name, relic.sprite, relic.goldCost);
            }
        }
    }
}