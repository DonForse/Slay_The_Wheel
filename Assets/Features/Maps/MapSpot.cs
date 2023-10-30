using System;
using System.Collections;
using System.Collections.Generic;
using Features.Maps;
using UnityEngine;
using UnityEngine.UI;

public class MapSpot : MonoBehaviour
{
    public List<MapSpot> PossibleNextPositions;
    [SerializeField]private MapSpotType mapSpotType;
    [SerializeField]private Button button;
    [SerializeField] private Image spotImage;
    [SerializeField] private Sprite[] icons;
    public EventHandler<MapSpotType> Selected;

    private void Awake()
    {
        SetImage();
    }

    private void SetImage()
    {
        spotImage.sprite = icons[(int)mapSpotType];
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(OnButtonPressed);
    }

    private void OnButtonPressed()
    {
        Selected?.Invoke(this,mapSpotType);
    }

    public void SetAvailable()
    {
        button.interactable = true;
        button.onClick.AddListener(OnButtonPressed);
    }
}