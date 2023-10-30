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
        [SerializeField] List<MapSpot> mapSpots;
        [SerializeField] CinemachineVirtualCamera virtualCamera;
        [SerializeField] private Shop.Shop shop;
        [SerializeField] private List<CardPackScriptableObject> packs;
        [SerializeField] private MapLine mapLine;
        private List<MapSpot> _possibleNextPositions;

        public event EventHandler<List<BaseCardScriptableObject>> SelectedPack;
        public event EventHandler<int> LevelCompleted;
        public event EventHandler SelectedRest;

        public void Initialize()
        {
            var current = PlayerPrefs.GetInt("CurrentLevel",0);
            virtualCamera.Follow = mapSpots[current].transform;
            shop.PackSelected += OnPackSelected;
            
            var levelsToSetLine = new List<Transform>();
            var levelsIndex = PlayerPrefs.GetString("LevelsCompleted","0").Split(',').Select(int.Parse);
            foreach (var index in levelsIndex)
            {
                levelsToSetLine.Add(mapSpots[index].transform);
            }

            SetLevel(mapSpots[current].PossibleNextPositions);
// shop.LevelSelected += OnLevelSelected;
            mapLine.SetLine(levelsToSetLine.ToArray());
        }

        private void SetLevel(List<MapSpot> possibleNextPositions)
        {
            _possibleNextPositions = possibleNextPositions;
            foreach (var spot in _possibleNextPositions)
            {
                spot.Selected += OnSpotSelected;
                spot.SetAvailable();
            }
        }

        private void OnSpotSelected(object sender, MapSpotType e)
        {
            var mapSpotSelected = (MapSpot)sender;
            foreach (var spot in _possibleNextPositions)
            {
                spot.Selected -= OnSpotSelected;
            }

            var levelIndex = mapSpots.IndexOf(mapSpotSelected);
            var levelsIndex = PlayerPrefs.GetString("LevelsCompleted").Split().Select(int.Parse).ToList();
            levelsIndex.Add(levelIndex);
            PlayerPrefs.SetString("LevelsCompleted", string.Join(',', levelsIndex));
            PlayerPrefs.SetInt("CurrentLevel", levelIndex);
            
            switch (e)
            {
                case MapSpotType.Shop:
                    shop.Show(packs.ToList());
                    break;
                case MapSpotType.Battle:
                    LevelCompleted?.Invoke(this, levelIndex);
                    break;
                case MapSpotType.Rest:
                    SelectedRest?.Invoke(this, null);
                    break;
            }
        }

        private void OnPackSelected(object sender, CardPackScriptableObject pack)
        {
            shop.Hide();
            SelectedPack?.Invoke(this, pack.Cards);
        }

    }
}