using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Features.Cards;
using UnityEngine;
using UnityPackages.Slay_The_Spire_Map.Scripts;

namespace Features.Maps
{
    public class Map : MonoBehaviour
    {
        [SerializeField] CinemachineVirtualCamera virtualCamera;
        [SerializeField] private Shop.Shop shop;
        [SerializeField] private List<CardPackScriptableObject> packs;
        [SerializeField] private MapPlayerTracker mapPlayerTracker;
        private List<MapSpot> _possibleNextPositions;

        public event EventHandler<List<BaseCardScriptableObject>> SelectedPack;
        public event EventHandler<int> MinorEnemySelected;
        public event EventHandler SelectedRest;

        private void Awake()
        {
            mapPlayerTracker.NodeSelected += OnNodeSelected;
            shop.PackSelected += OnPackSelected;
        }

        private void OnDestroy()
        {
            mapPlayerTracker.NodeSelected -= OnNodeSelected;
            shop.PackSelected -= OnPackSelected;
        }
        
        private void OnNodeSelected(object sender, NodeType e)
        {
            switch (e)
            {
                case NodeType.MinorEnemy:
                    MinorEnemySelected?.Invoke(this, 1);
                    break;
                case NodeType.EliteEnemy:
                    MinorEnemySelected?.Invoke(this, 4);
                    break;
                case NodeType.RestSite:
                    SelectedRest?.Invoke(this, null);
                    break;
                case NodeType.Treasure:
                    break;
                case NodeType.Store:
                    shop.Show(packs.ToList());
                    break;
                case NodeType.Boss:
                    MinorEnemySelected?.Invoke(this, 10);
                    break;
                case NodeType.Mystery:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(e), e, null);
            }
        }

        private void OnPackSelected(object sender, CardPackScriptableObject pack)
        {
            shop.Hide();
            SelectedPack?.Invoke(this, pack.Cards);
        }
    }
}