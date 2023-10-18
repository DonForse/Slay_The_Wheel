using System;
using Features.Cards;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Maps
{
    public class ShopCard : MonoBehaviour
    {
        private BaseCardScriptableObject _card;
        [SerializeField] private Button button;
        [SerializeField] private TMP_Text cardNameText;
        public void Set(BaseCardScriptableObject baseCardScriptableObject)
        {
            _card = baseCardScriptableObject;
            cardNameText.text = baseCardScriptableObject.cardName;
            button.onClick.AddListener(Selected);
        }

        private void Selected()
        {
            OnClick?.Invoke(this, _card);
        }

        public event EventHandler<BaseCardScriptableObject> OnClick;
    }
}
