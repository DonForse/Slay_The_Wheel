using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Features.Maps.Shop
{
    public class Shop : MonoBehaviour
    {
        [SerializeField][Range(0,8)] private int amountOfPacks;
        [SerializeField] private GameObject container;
        [SerializeField] private ShopPack shopPackPrefab;
        [SerializeField] private GameObject revealCardContainer;
        [SerializeField] private List<CardReveal> cardReveals;
        [SerializeField] private Button continueButton;
        private CardPackScriptableObject _packSelected;
        public event EventHandler<CardPackScriptableObject> PackSelected;

        private void OnEnable()
        {
            continueButton.onClick.AddListener(SendPackSelected);
        }

        private void OnDisable()
        {
            continueButton.onClick.RemoveListener(SendPackSelected);
        }

        public void Show(List<CardPackScriptableObject> packs)
        {
            continueButton.enabled = false;
            foreach (Transform child in container.transform)
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < amountOfPacks; i++)
            {
                var shopPack  =Instantiate(shopPackPrefab, container.transform);
                shopPack.Set(packs[Random.Range(0, packs.Count)]);
                shopPack.OnClick += OnPackSelected;
            }

            container.SetActive(true);
        }

        public void Hide()
        {
            this.container.SetActive(false);
            this.revealCardContainer.SetActive(false);
            this.gameObject.SetActive(false);
        }

        private void OnPackSelected(object sender, CardPackScriptableObject e)
        {
            container.SetActive(false);
            _packSelected = e;
            revealCardContainer.SetActive(true);
            for (int i = 0; i < e.Cards.Count; i++)
            {
                cardReveals[i].Set(e.Cards[i]);
            }
            continueButton.enabled = true;
        }

        private void SendPackSelected()
        {
            PackSelected?.Invoke(this, _packSelected);
        }
    }
}