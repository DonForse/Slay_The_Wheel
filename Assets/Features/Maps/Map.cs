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
        public event EventHandler MinorEnemySelected;
        public event EventHandler SelectedRest;

        private void Awake()
        {
            mapPlayerTracker.NodeSelected += OnNodeSelected;
            mapPlayerTracker.Locked = false;
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
                    MinorEnemySelected?.Invoke(this, null);
                    mapPlayerTracker.Locked = false;
                    break;
                case NodeType.EliteEnemy:
                    MinorEnemySelected?.Invoke(this, null);
                    mapPlayerTracker.Locked = false;
                    break;
                case NodeType.RestSite:
                    SelectedRest?.Invoke(this, null);
                    mapPlayerTracker.Locked = false;
                    break;
                case NodeType.Treasure:
                    mapPlayerTracker.Locked = false;
                    break;
                case NodeType.Store:
                    shop.Show(packs.ToList());
                    break;
                case NodeType.Boss:
                    MinorEnemySelected?.Invoke(this, null);
                    mapPlayerTracker.Locked = false;
                    break;
                case NodeType.Mystery:
                    mapPlayerTracker.Locked = false;

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(e), e, null);
            }
        }

        private void OnPackSelected(object sender, CardPackScriptableObject pack)
        {
            shop.Hide();
            mapPlayerTracker.Locked = false;
            SelectedPack?.Invoke(this, pack.Cards);
        }
    }
}