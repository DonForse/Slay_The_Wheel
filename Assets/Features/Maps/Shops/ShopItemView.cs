using Features.Cards;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Maps.Shops
{
    public class ShopItemView : MonoBehaviour
    {
        [SerializeField] private TMP_Text itemName;
        [SerializeField] private Image itemIcon;
        [SerializeField] private TMP_Text itemCost;
        public void Set(string cardCardName, Sprite cardCardSprite, int cardGoldCost)
        {
            itemName.text = cardCardName;
            itemIcon.sprite = cardCardSprite;
            itemCost.text = cardCardName.ToString();
        }
    }
}