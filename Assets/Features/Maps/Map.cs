using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Features.Cards;
using Features.Maps.Relics;
using Features.Maps.Shop;
using UnityEngine;
using UnityPackages.Slay_The_Spire_Map.Scripts;

namespace Features.Maps
{
    public class Map : MonoBehaviour
    {
        [SerializeField] private ShopNode shopNode;
        [SerializeField] private List<CardPackScriptableObject> packs;
        [SerializeField] private List<RelicScriptableObject> relics;
        [SerializeField] private MapPlayerTracker mapPlayerTracker;
        [SerializeField] private RelicsNode relicsNode;
        private List<MapSpot> _possibleNextPositions;

        public event EventHandler<List<BaseCardScriptableObject>> SelectedPack;
        public event EventHandler MinorEnemySelected;
        public event EventHandler BossEnemySelected;
        public event EventHandler MajorEnemySelected;
        public event EventHandler SelectedRest;
        public event EventHandler<Relic> SelectedRelic;


        private void Awake()
        {
            mapPlayerTracker.NodeSelected += OnNodeSelected;
            mapPlayerTracker.Locked = false;
            shopNode.PackSelected += OnPackSelected;
            relicsNode.RelicSelected += OnRelicSelected;
        }

        private void OnDestroy()
        {
            mapPlayerTracker.NodeSelected -= OnNodeSelected;
            shopNode.PackSelected -= OnPackSelected;
            relicsNode.RelicSelected += OnRelicSelected;
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
                    MajorEnemySelected?.Invoke(this, null);
                    mapPlayerTracker.Locked = false;
                    break;
                case NodeType.RestSite:
                    SelectedRest?.Invoke(this, null);
                    mapPlayerTracker.Locked = false;
                    break;
                case NodeType.Treasure:

                    relicsNode.Show(relics.ToList());
                    // mapPlayerTracker.Locked = false;
                    break;
                case NodeType.Store:
                    shopNode.Show(packs.ToList());
                    break;
                case NodeType.Boss:
                    BossEnemySelected?.Invoke(this, null);
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
            shopNode.Hide();
            mapPlayerTracker.Locked = false;
            SelectedPack?.Invoke(this, pack.Cards);
        }

        private void OnRelicSelected(object sender, Relic e)
        {
            relicsNode.Hide();
            mapPlayerTracker.Locked = false;
            SelectedRelic?.Invoke(this, e);
        }
    }
}