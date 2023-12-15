using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Features.Cards;
using Features.Maps.ChestReward;
using Features.Maps.Shop;
using Features.Maps.Shop.Packs;
using UnityEngine;
using UnityEngine.Serialization;
using UnityPackages.Slay_The_Spire_Map.Scripts;

namespace Features.Maps
{
    public class Map : MonoBehaviour
    {
        [FormerlySerializedAs("shopNode")] [SerializeField] private BoosterPackNode boosterPackNode;
        [SerializeField] private List<CardPackScriptableObject> packs;
        [SerializeField] private MapPlayerTracker mapPlayerTracker;
        [FormerlySerializedAs("relicsNode")] [SerializeField] private ChestRewardNode chestRewardNode;
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
            boosterPackNode.PackSelected += OnPackSelected;
            chestRewardNode.RelicSelected += OnChestRewardSelected;
        }

        private void OnDestroy()
        {
            mapPlayerTracker.NodeSelected -= OnNodeSelected;
            boosterPackNode.PackSelected -= OnPackSelected;
            chestRewardNode.RelicSelected += OnChestRewardSelected;
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

                    chestRewardNode.Show();
                    // mapPlayerTracker.Locked = false;
                    break;
                case NodeType.Store:
                    boosterPackNode.Show(packs.ToList());
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

        private void OnPackSelected(object sender, List<BaseCardScriptableObject> cards)
        {
            boosterPackNode.Hide();
            mapPlayerTracker.Locked = false;
            SelectedPack?.Invoke(this, cards);
        }

        private void OnChestRewardSelected(object sender, Relic e)
        {
            chestRewardNode.Hide();
            mapPlayerTracker.Locked = false;
            SelectedRelic?.Invoke(this, e);
        }
    }
}