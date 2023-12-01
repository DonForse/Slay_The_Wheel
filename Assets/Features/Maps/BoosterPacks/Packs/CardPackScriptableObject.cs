using System.Collections.Generic;
using Features.Cards;
using UnityEngine;

namespace Features.Maps.Shop.Packs
{
    [CreateAssetMenu(fileName = "Pack", menuName = "Shop/Pack")]
    public class CardPackScriptableObject : ScriptableObject
    {
        public List<CardPackItem> Cards;
        public BaseCardScriptableObject defaultCard;
        public string PackName;
        public Sprite Image;
    }
}