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
        [SerializeField] private Shop.Shop shop;
        [SerializeField] private List<CardPackScriptableObject> packs;
    
    
        public event EventHandler<BaseCardScriptableObject> SelectedUpgradeCard;

        public void Initialize(int currentLevel)
        {
            virtualCamera.Follow = levels[currentLevel].transform;
            shop.Show(packs.ToList());
            shop.PackSelected += OnPackSelected;
        }

        private void OnPackSelected(object sender, CardPackScriptableObject pack)
        {
            SelectedUpgradeCard?.Invoke(this, pack.Cards.First());
        }

    }
}