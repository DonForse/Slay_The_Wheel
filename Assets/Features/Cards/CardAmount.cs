using System;

namespace Features.Cards
{
    [Serializable]
    public class CardAmount
    {
        public int Amount;
        public BaseCardScriptableObject card;
    }
}