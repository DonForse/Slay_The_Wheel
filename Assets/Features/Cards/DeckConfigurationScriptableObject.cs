using System.Collections.Generic;
using UnityEngine;

namespace Features.Cards
{
    [CreateAssetMenu(fileName = "DeckConfiguration", menuName = "Cards/Base Deck")]
    public class DeckConfigurationScriptableObject : ScriptableObject
    {
        public BaseCardScriptableObject hero;
        public List<CardAmount> cards = new();
        //public List<Skills> cards = new();
    }
}