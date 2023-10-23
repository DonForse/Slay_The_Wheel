using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Maps.Shop
{
    public class ShopPack : MonoBehaviour
    {
        private CardPackScriptableObject _pack;
        [SerializeField] private Button button;
        [SerializeField] private Image image;
        [SerializeField] private TMP_Text cardNameText;
        public void Set(CardPackScriptableObject pack)
        {
            _pack = pack;
            cardNameText.text = pack.PackName;
            image.sprite = pack.Image;
            button.onClick.AddListener(Selected);
        }

        private void Selected()
        {
            OnClick?.Invoke(this, _pack);
        }

        public event EventHandler<CardPackScriptableObject> OnClick;
    }
}
