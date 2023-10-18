using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Features.Cards;
using UnityEngine;

namespace Features.Maps
{
    public class Map : MonoBehaviour
    {
        [SerializeField] List<GameObject> levels;
        [SerializeField] CinemachineVirtualCamera virtualCamera;
        [SerializeField] private Shop shop;
        [SerializeField] private BaseCardsScriptableObject cardsDb;
    
    
        public event EventHandler<BaseCardScriptableObject> SelectedUpgradeCard;

        public void Initialize(int currentLevel)
        {
            virtualCamera.Follow = levels[currentLevel].transform;
            shop.Show(cardsDb.cards.Distinct().ToList());
            shop.CardSelected += OnCardSelected;
        }

        private void OnCardSelected(object sender, BaseCardScriptableObject card)
        {
            SelectedUpgradeCard?.Invoke(this, card);
        }

    }
}