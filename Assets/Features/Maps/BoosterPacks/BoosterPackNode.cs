using System;
using System.Collections.Generic;
using Features.Cards;
using Features.Maps.Shop.Packs;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Features.Maps.Shop
{
    public class BoosterPackNode : MonoBehaviour
    {
        private const int AmountOfCards = 5;
        [SerializeField][Range(0,8)] private int amountOfPacks;
        [SerializeField] private GameObject packSelectionCanvas;
        
        [SerializeField] private GameObject packSelectionContainer;
        
        [SerializeField] private BoosterPack boosterPackPrefab;
        [SerializeField] private GameObject revealCardCanvas;
        [SerializeField] private List<CardReveal> cardReveals;
        [SerializeField] private Button continueButton;
        private CardPackScriptableObject _packSelected;
        private List<BaseCardScriptableObject> _cardsObtained;
        public event EventHandler<List<BaseCardScriptableObject>> PackSelected;

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
            this.gameObject.SetActive(true);
            continueButton.enabled = false;
            foreach (Transform child in packSelectionContainer.transform)
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < amountOfPacks; i++)
            {
                var shopPack  =Instantiate(boosterPackPrefab, packSelectionContainer.transform);
                shopPack.Set(packs[Random.Range(0, packs.Count)]);
                shopPack.OnClick += OnPackSelected;
            }

            packSelectionCanvas.SetActive(true);
        }

        public void Hide()
        {
            this.packSelectionCanvas.SetActive(false);
            this.revealCardCanvas.SetActive(false);
            this.gameObject.SetActive(false);
        }

        private void OnPackSelected(object sender, CardPackScriptableObject e)
        {
            packSelectionCanvas.SetActive(false);
            _packSelected = e;
            revealCardCanvas.SetActive(true);

            _cardsObtained = new List<BaseCardScriptableObject>();
            for (int i = 0; i < AmountOfCards; i++)
            {
                var random= Random.Range(0f, 1f);

                BaseCardScriptableObject selectedCard = null;
                foreach (var item in e.Cards)
                {
                    if (random > item.dropRatePercentage)
                        continue;
                    selectedCard = item.card;
                    break;
                }

                if (selectedCard == null)
                    selectedCard = e.defaultCard;

                _cardsObtained.Add(selectedCard);
                cardReveals[i].Set(selectedCard);
            }
            continueButton.enabled = true;
        }

        private void SendPackSelected()
        {
            PackSelected?.Invoke(this, _cardsObtained);
        }
    }
}