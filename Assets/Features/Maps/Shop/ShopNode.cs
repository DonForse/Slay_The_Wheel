using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Features.Maps.Shop
{
    public class ShopNode : MonoBehaviour
    {
        [SerializeField][Range(0,8)] private int amountOfPacks;
        [SerializeField] private GameObject packSelectionCanvas;
        
        [SerializeField] private GameObject packSelectionContainer;
        
        [SerializeField] private ShopPack shopPackPrefab;
        [SerializeField] private GameObject revealCardCanvas;
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
            this.gameObject.SetActive(true);
            continueButton.enabled = false;
            foreach (Transform child in packSelectionContainer.transform)
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < amountOfPacks; i++)
            {
                var shopPack  =Instantiate(shopPackPrefab, packSelectionContainer.transform);
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