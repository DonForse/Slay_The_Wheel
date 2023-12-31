using System.Collections.Generic;
using System.Linq;
using Features.Battles.Wheel;
using Features.Cards;
using Features.Cards.InPlay;

namespace Features.Battles
{
    public class BattleDeckManager
    {
        private List<InPlayCardScriptableObject> _deck;
        private List<InPlayCardScriptableObject> _discardPile;

        public void Initialize(List<RunCardScriptableObject> cards, PlayerController owner)
        {
            _deck = cards.Select(card => new InPlayCardScriptableObject(card, owner)).ToList();
            _discardPile = new();

        }

        public List<InPlayCardScriptableObject> DrawCards(int amountToDraw)
        {
            _discardPile = _discardPile.Where(x => !x.IsDead).ToList();
            if (_deck.Count < amountToDraw)
            {
                _deck = _deck.Concat(_discardPile).ToList();
                _discardPile.Clear();
            }

            var cards = _deck.Take(amountToDraw).ToList();
            _deck = _deck.Skip(amountToDraw).ToList();
            return cards;
        }


        public bool HasCards() => _deck.Count + _discardPile.Count > 0;

        public void DiscardCards(List<InPlayCardScriptableObject> cards)
        {
            _discardPile.AddRange(cards);
        }
    }
}